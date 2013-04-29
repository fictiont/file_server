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
            while (true)
            {
                if (clientScktEngine.socket.Available != 0)
                {
                    string message = "";
                    message = clientScktEngine.receiveMessage();

                    switch (message)
                    {
                        case "registration":
                            try
                            {
                                clientScktEngine.sendMessage("ready to registration");
                                currentUser.Receive(clientScktEngine);
                                string[] loginCheck = myBD.takeValue("password", "login", currentUser.login);
                                if (loginCheck.Length == 0)
                                {
                                    if (myBD.addValue(currentUser.login, myCrypt.EncryptStringAES(currentUser.password, currentUser.login)) == true)
                                        clientScktEngine.sendMessage("successfully registration");
                                    else
                                    {
                                        clientScktEngine.sendMessage("Error when adding data to BD. Check your connection");
                                    }
                                }
                                else
                                {
                                    clientScktEngine.sendMessage("This login is already exist");
                                }
                            }
                            catch(Exception ex)
                            {
                                clientScktEngine.sendMessage(ex.Message);
                            }
                        break;

                        case "accept user":
                            try
                            {
                                clientScktEngine.sendMessage("ready to accept user");
                                currentUser.Receive(clientScktEngine);
                                clientScktEngine.sendMessage("successfully accept user");
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
                                        if (Directory.Exists(currentUser.login + "/TEMP") == false)
                                            Directory.CreateDirectory(currentUser.login + "/TEMP");
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
                                if (clientScktEngine.receiveMessage().CompareTo("ready to take") == 0)
                                    currentUser.Send(clientScktEngine);
                            }
                            catch (Exception ex)
                            {
                                clientScktEngine.sendMessage(ex.Message);
                            }

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
                                    string[] takePrivileges = myBD.takeValue("privileges", "login", Path.GetFileName(dirName));
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
                        case "miniatures":
                        try
                        {
                            miniatures minis = new miniatures();
                            minis.updateMiniatures(currentUser.login);
                            clientScktEngine.sendMessage("take miniatures");
                            if (clientScktEngine.receiveMessage().CompareTo("start to take") == 0)
                            {
                                minis.sendMiniatures(clientScktEngine);
                                clientScktEngine.sendMessage("successfully sended");
                            }
                            else
                                clientScktEngine.sendMessage("error when updating miniatures");
                        }
                        catch (Exception ex)
                        {
                            clientScktEngine.sendMessage(ex.Message);
                        }
                        break;
                        case "file list":
                            try
                            {
                                files filesS = new files();
                                filesS.updateFileList(currentUser.login);
                                clientScktEngine.sendMessage("take file list");
                                if (clientScktEngine.receiveMessage().CompareTo("ready to receive") == 0)
                                {
                                    filesS.sendFileList(clientScktEngine);
                                    clientScktEngine.sendMessage("successfull file list");
                                }
                                else
                                    clientScktEngine.sendMessage("failed to update file list");
                            }
                            catch (Exception ex)
                            {
                                clientScktEngine.sendMessage("Error when updating file list: " + ex.Message);
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