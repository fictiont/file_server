//Сокеты
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Timers;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Drawing;
class Program
{
    static int BLOCK_SIZE = 2048;
    static Mutex mtx = new Mutex();
    static engineBD myBD = new engineBD();
    static engineCrypt myCrypt = new engineCrypt();
    static userInfo currentUser = new userInfo();

    static void Main(string[] args)
    {
        socketEngine scktEngine = new socketEngine(IPAddress.Parse(Console.ReadLine()));
        scktEngine.mutex = mtx;
        scktEngine.start();
        Console.Title = "Server";
        Console.WriteLine("Ждем коннекта...");

        while(true)
        {
            socketEngine clientScktEngine = new socketEngine();
            clientScktEngine.socket = scktEngine.waitForClient();
            Thread ClientThread = new Thread(delegate() { ProcessClient(clientScktEngine); });
            ClientThread.Start();
        }

    }
    
    static void ProcessClient(socketEngine clientScktEngine)
    {
        clientScktEngine.mutex = mtx;
        try
        {
            string name = "default";
            my myC = new my();
            while (true) //Пока не нажата клавиша
            {
                if (clientScktEngine.socket.Available != 0)
                {
                    string message = "";
                    message = clientScktEngine.receiveMessage();

                    switch (message)
                    {
                        case "registration":
                            clientScktEngine.sendMessage("login");
                            string login = clientScktEngine.receiveMessage();
                            try
                            {
                                string[] loginCheck = myBD.takeValue("password", "login", login);
                                if (loginCheck.Length == 0)
                                {
                                    clientScktEngine.sendMessage("pwd");
                                    string pwd = clientScktEngine.receiveMessage();
                                    if (myBD.addValue(login, myCrypt.EncryptStringAES(pwd, login)) == true)
                                        clientScktEngine.sendMessage("success");
                                    else
                                    {
                                        clientScktEngine.sendMessage("failed");
                                        if (clientScktEngine.receiveMessage().Contains("why"))
                                            clientScktEngine.sendMessage("Ошибка добавления данных в базу. Проверьте сетевое подключение");
                                    }
                                    clientScktEngine.sendMessage("end");
                                }
                                else
                                {
                                    clientScktEngine.sendMessage("failed");
                                    if (clientScktEngine.receiveMessage().Contains("why"))
                                        clientScktEngine.sendMessage("Такой логин уже существует.");
                                    clientScktEngine.sendMessage("end");
                                }
                            }
                            catch
                            {
                                clientScktEngine.sendMessage("failed");
                                if (clientScktEngine.receiveMessage().Contains("why"))
                                    clientScktEngine.sendMessage("Такой логин уже существует.");
                                clientScktEngine.sendMessage("end");
                            }
                            break;
                        case "accept user":
                            try
                            {
                                clientScktEngine.sendMessage("ready to accept user");
                                currentUser.Receive(clientScktEngine.socket);
                                clientScktEngine.sendMessage("successfully accept user");
                                clientScktEngine.ConsoleWrite(currentUser.password);
                            }
                            catch (Exception ex)
                            {
                                clientScktEngine.sendMessage(ex.Message);
                            }
                        break;
                        case "check user":
                            try
                            {
                                string[] loginCheck = myBD.takeValue("password", "login", currentUser.login);
                                if (loginCheck.Length > 0)
                                {
                                    if (currentUser.password == myCrypt.DecryptStringAES(loginCheck[0], currentUser.login))
                                    {
                                        currentUser.checkUser = true;
                                        currentUser.priveleges = Convert.ToInt32(myBD.takeValue("privileges", "login", currentUser.login)[0]);
                                    }
                                    else
                                    {
                                        currentUser.checkUser = false;
                                        currentUser.status = "Wrong password or login";
                                    }
                                }
                                else
                                {
                                    currentUser.checkUser = false; 
                                    currentUser.status = ("Wrong password or login");
                                }
                                clientScktEngine.sendMessage("take user info");
                                currentUser.Send(clientScktEngine.socket);
                            }
                            catch (Exception ex)
                            {
                                clientScktEngine.sendMessage(ex.Message);
                            }

                        break;
                        case "connect":
                            clientScktEngine.sendMessage("name");
                            name = clientScktEngine.receiveMessage();
                            try
                            {
                                if (Directory.Exists(name) == false)
                                    Directory.CreateDirectory(name);
                            }
                            catch
                            {
                                Directory.CreateDirectory("default");
                                name = "default";
                            }
                            if (Directory.Exists(name + "/TEMP") == false)
                                Directory.CreateDirectory(name + "/TEMP");
                            myC.updateMiniatures(name, clientScktEngine);
                            string[] takePrivileges = myBD.takeValue("privileges", "login", name);
                            clientScktEngine.sendMessage("privileges");
                            clientScktEngine.receiveMessage();
                            clientScktEngine.sendMessage(takePrivileges[0].ToString());
                            clientScktEngine.receiveMessage();
                            clientScktEngine.sendMessage("connected");
                            break;
                        case "delete":
                            clientScktEngine.sendMessage("ready to delete files");
                            int days = Convert.ToInt32(clientScktEngine.receiveMessage());
                            string[] dirs = Directory.GetDirectories("./");
                            try
                            {
                                foreach (string dirName in dirs)
                                {
                                    string[] fileInfos = new string[] { "" };
                                    Console.WriteLine(Path.GetFileName(dirName));
                                    takePrivileges = myBD.takeValue("privileges", "login", Path.GetFileName(dirName));
                                    if (takePrivileges.Length > 0)
                                    {
                                        try
                                        {
                                            fileInfos = Directory.GetFiles("./" + dirName);
                                        }
                                        catch
                                        {
                                            myBD.CloseConnection();
                                        }
                                        foreach (string fileInfoName in fileInfos)
                                        {
                                            FileInfo fl = new FileInfo(fileInfoName);
                                            TimeSpan lastAcc = new TimeSpan(fl.LastAccessTime.Ticks);
                                            TimeSpan now = new TimeSpan(DateTime.Now.Ticks);
                                            if ((now.TotalSeconds - lastAcc.TotalSeconds) > days)
                                            {
                                                fl.Delete();
                                                if (File.Exists("./" + dirName + "/TEMP/" + Path.GetFileName(fileInfoName) + ".smll") == true)
                                                {
                                                    fl = new FileInfo("./" + dirName + "/TEMP/" + Path.GetFileName(fileInfoName) + ".smll");
                                                    fl.Delete();
                                                }
                                                if (File.Exists("./" + dirName + "/TEMP/" + Path.GetFileName(fileInfoName) + ".finf") == true)
                                                {
                                                    fl = new FileInfo("./" + dirName + "/TEMP/" + Path.GetFileName(fileInfoName) + ".finf");
                                                    fl.Delete();
                                                }

                                            }
                                        }
                                        clientScktEngine.sendMessage("success");
                                    }
                                    else
                                    {
                                        Console.WriteLine("To delete: " + dirName);
                                        DirectoryInfo dir = new DirectoryInfo(dirName);
                                        dir.Delete(true);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                clientScktEngine.sendMessage("failed");
                                if (clientScktEngine.receiveMessage().Contains("why"))
                                    clientScktEngine.sendMessage(ex.Message);
                            }
                            break;
                        case "authentication":
                            clientScktEngine.sendMessage("login");
                            login = clientScktEngine.receiveMessage();
                            clientScktEngine.sendMessage("password");
                            string password = clientScktEngine.receiveMessage();
                            try
                            {
                                string[] loginCheck = myBD.takeValue("password", "login", login);
                                if (loginCheck.Length > 0)
                                {
                                    if (password == myCrypt.DecryptStringAES(loginCheck[0], login))
                                        clientScktEngine.sendMessage("confirmed");
                                    else
                                    {
                                        clientScktEngine.sendMessage("failed");
                                        if (clientScktEngine.receiveMessage().Contains("why"))
                                            clientScktEngine.sendMessage("Wrong password or login");
                                    }
                                }
                                else
                                {
                                    clientScktEngine.sendMessage("failed");
                                    if (clientScktEngine.receiveMessage().Contains("why"))
                                        clientScktEngine.sendMessage("Wrong password or login");
                                }
                            }
                            catch (Exception ex)
                            {
                                clientScktEngine.sendMessage("failed");
                                if (clientScktEngine.receiveMessage().Contains("why"))
                                    clientScktEngine.sendMessage(ex.Message);
                            }
                            break;
                        case "file list":
                            clientScktEngine.sendMessage("miniatures");
                            if (clientScktEngine.receiveMessage().Contains("ready to obtain miniatures"))
                                myC.updateMiniatures(currentUser.login, clientScktEngine);
                            clientScktEngine.sendMessage("end");
                            string[] fils = Directory.GetFiles(name);
                            int count = 0;
                            foreach (string fl in fils)
                                if (Path.GetExtension(fl).Contains("finf") == false &&
                                    Path.GetExtension(fl).Contains("smll") == false)
                                    count++;
                            string[] files = new string[count];
                            count = 0;
                            foreach (string fl in fils)
                                if (Path.GetExtension(fl).Contains("finf") == false &&
                                    Path.GetExtension(fl).Contains("smll") == false)
                                {
                                    files[count] = fl;
                                    count++;
                                }
                            clientScktEngine.receiveMessage();
                            clientScktEngine.sendMessage("file list");
                            if (clientScktEngine.receiveMessage().Contains("length"))
                                clientScktEngine.sendMessage(files.Length.ToString());
                            if (clientScktEngine.receiveMessage().Contains("files"))
                                for (int i = 0; i < files.Length; i++)
                                {
                                    files[i] = Path.GetFileName(files[i]);

                                    if (File.Exists(currentUser.login + "/TEMP/" + Path.GetFileName(files[i]) + ".finf") == false)
                                    {
                                        clientScktEngine.sendMessage("0");
                                    }
                                    else
                                    {
                                        Byte[] bt = new Byte[4];
                                        FileStream fl = File.OpenRead(currentUser.login + "/TEMP/" + Path.GetFileName(files[i]) + ".finf");
                                        fl.Read(bt, 0, 4);
                                        clientScktEngine.sendMessage(Encoding.UTF8.GetString(bt));
                                        fl.Close();
                                    }
                                    if (clientScktEngine.receiveMessage().Contains("file name"))
                                        clientScktEngine.sendMessage(files[i]);
                                    if (clientScktEngine.receiveMessage().Contains("OK"))
                                        continue;
                                }
                            break;
                        case "disconnect":
                            Thread.CurrentThread.Abort();
                            break;
                        case "file sent":
                            clientScktEngine.sendMessage("name");
                            string buffer = "";
                            buffer = clientScktEngine.receiveMessage();
                            string fileName = buffer;
                            clientScktEngine.sendMessage("length");
                            buffer = clientScktEngine.receiveMessage();
                            long fileLength = 0;
                            try
                            {
                                fileLength = Convert.ToInt64(buffer);
                            }
                            catch
                            {
                                fileLength = 0;
                            }
                            clientScktEngine.sendMessage("file");
                            try
                            {
                                FileStream outputStream = File.OpenWrite(currentUser.login + "/" + fileName);
                                byte[] buf = new byte[BLOCK_SIZE];
                                for (long i = 0; i <= fileLength; i += BLOCK_SIZE)
                                {
                                    clientScktEngine.socket.Receive(buf, 0, BLOCK_SIZE, SocketFlags.None);
                                    outputStream.Write(buf, 0, BLOCK_SIZE);
                                }
                                outputStream.Close();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                            myC.generatePreview(currentUser.login + "/" + fileName, name);
                            FileInfo fileInf = new FileInfo(currentUser.login + "/" + fileName);
                            fileInf.LastAccessTime = DateTime.Now;
                            break;
                        case "file download":
                            int downloadsCount = 0;
                            string flName = "";
                            clientScktEngine.sendMessage("file"); //send confirm
                            message = clientScktEngine.receiveMessage();
                            if (message.Contains("name") == true)
                            {
                                clientScktEngine.sendMessage("OK");
                                flName = clientScktEngine.receiveMessage();
                                clientScktEngine.sendMessage("OK");
                            }
                            if (File.Exists(currentUser.login + "/TEMP/" + Path.GetFileName(flName) + ".finf") == false)
                            {
                                byte[] bt = new byte[4];
                                bt = Encoding.UTF8.GetBytes("1");
                                FileStream fl = File.OpenWrite(currentUser.login + "/TEMP/" + Path.GetFileName(flName) + ".finf");
                                fl.Write(bt, 0, bt.Length);
                                fl.Close();
                            }
                            else
                            {
                                byte[] bt = new byte[4];
                                FileStream fl = File.OpenRead(currentUser.login + "/TEMP/" + Path.GetFileName(flName) + ".finf");
                                fl.Read(bt, 0, 4);
                                fl.Close();
                                fl = File.OpenWrite(currentUser.login + "/TEMP/" + Path.GetFileName(flName) + ".finf");
                                try
                                {
                                    downloadsCount = Convert.ToInt32(Encoding.UTF8.GetString(bt));
                                }
                                catch
                                {
                                    downloadsCount = 0;
                                }
                                downloadsCount++;
                                bt = Encoding.UTF8.GetBytes(downloadsCount.ToString());
                                fl.Write(bt, 0, bt.Length);
                                fl.Close();
                            }
                            using (FileStream inputStream = File.OpenRead(currentUser.login + "/" + flName))
                            {
                                long length = inputStream.Length;
                                if (clientScktEngine.receiveMessage().Contains("length"))
                                    clientScktEngine.sendMessage(length.ToString()); //send file length
                                byte[] buf = new byte[BLOCK_SIZE];
                                if (clientScktEngine.receiveMessage().Contains("file"))
                                    for (long i = 0; i <= length; i += BLOCK_SIZE)
                                    {
                                        inputStream.Read(buf, 0, BLOCK_SIZE);
                                        clientScktEngine.socket.Send(buf, 0, BLOCK_SIZE, SocketFlags.None);
                                    }
                                inputStream.Close();
                            }
                            fileInf = new FileInfo(currentUser.login + "/" + flName);
                            fileInf.LastAccessTime = DateTime.Now;
                            break;
                    }
                }

            }
        }
        catch (Exception ex)
        {
            myBD.CloseConnection();
            MessageBox.Show(ex.Message);
        }
    }

}
class my
{
    public int BLOCK_SIZE = 2048;
    public void updateMiniatures(string name, socketEngine clientScktEngine)
    {
        string[] files = Directory.GetFiles(name);
        int count = 0;
        foreach (string str in files)
        {
            if (File.Exists(str + ".smll") == false)
            {
                count += generatePreview(str, name);
            }
            else
                count++;
        }
        clientScktEngine.sendMessage("count");
        clientScktEngine.receiveMessage();
        clientScktEngine.sendMessage(count.ToString());
        clientScktEngine.receiveMessage();
        files = Directory.GetFiles(name + "/TEMP/");
        foreach (string str in files)
            if (Path.GetExtension(str).Contains(".smll"))
            {
                string strr = str;
                clientScktEngine.sendMessage("miniature");
                if (clientScktEngine.receiveMessage().Contains("name"))
                    clientScktEngine.sendMessage(strr);
                FileStream fl = File.OpenRead(strr);
                if (clientScktEngine.receiveMessage().Contains("length"))
                    clientScktEngine.sendMessage(fl.Length.ToString());
                if (clientScktEngine.receiveMessage().Contains("file"))
                {
                    for (long i = 0; i < fl.Length; i += BLOCK_SIZE)
                    {
                        Byte[] bt = new Byte[BLOCK_SIZE];
                        fl.Read(bt, 0, BLOCK_SIZE);
                        clientScktEngine.socket.Send(bt, 0, BLOCK_SIZE, SocketFlags.None);
                    }
                }
                fl.Close();
            }
    }

    public int generatePreview(string fileName, string name)
    {
        string extension = Path.GetExtension(fileName).ToLower();
        if (extension.Contains(".bmp") || extension.Contains(".jpg") ||
            extension.Contains(".jpeg") || extension.Contains(".png"))
        {
            Bitmap imageToConv = new Bitmap(fileName);
            int newHeight = 0;
            int newWidth = 0;
            if (imageToConv.Height == imageToConv.Width)
            {
                newHeight = 234; newWidth = 234;
            }
            if (imageToConv.Height > imageToConv.Width)
            {
                if (imageToConv.Height > 234 && imageToConv.Width > 365)
                {
                    newHeight = 234;
                    newWidth = (int)(imageToConv.Width / ((float)imageToConv.Height / 234));
                }
                else
                {
                    newHeight = imageToConv.Height; newWidth = imageToConv.Width;
                }
            }
            if (imageToConv.Width > imageToConv.Height)
            {
                if (imageToConv.Height > 234 && imageToConv.Width > 365)
                {
                    newWidth = 365;
                    newHeight = (int)(imageToConv.Height / ((float)imageToConv.Width / 365));
                }
                else
                {
                    newHeight = imageToConv.Height; newWidth = imageToConv.Width;
                }
            }

            Bitmap bm = new Bitmap(imageToConv, newWidth, newHeight);
            if (fileName.ToLower().Contains(".bmp"))
                bm.Save(name + "/TEMP/" + Path.GetFileName(fileName) + ".smll", System.Drawing.Imaging.ImageFormat.Bmp);
            if (fileName.ToLower().Contains(".jpg") || fileName.Contains(".jpeg"))
                bm.Save(name + "/TEMP/" + Path.GetFileName(fileName) + ".smll", System.Drawing.Imaging.ImageFormat.Jpeg);
            if (fileName.ToLower().Contains(".png"))
                bm.Save(name + "/TEMP/" + Path.GetFileName(fileName) + ".smll", System.Drawing.Imaging.ImageFormat.Png);
            imageToConv.Dispose();
            return 1;
        }
        return 0;
    }
}