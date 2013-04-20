using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Drawing;
using System.Text;


public static class memoryStatic
{
    #region static variables
        
        static public int BLOCK_SIZE = 512;
        static public string login;
        static public socketEngine sock;
        static public int privileges = 5;

    #endregion

    #region static functions

        static public byte[] serialize(object serObj)
        {
            byte[] result;
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            try
            {
                bf.Serialize(ms, serObj);
                result = ms.ToArray();
                ms.Close();
            }
            catch (Exception ex)
            {
                ms.Close();
                throw new ArgumentException("Serialization failed: " + ex.Message);
            }
            return result;
        }

        /// <summary>
        /// send a serialized object structure to specified socket.
        /// </summary>
        /// <param name="s">
        /// socket to send
        /// </param>
        /// <param name="ToSend">
        /// object to send
        /// </param>
        static public void Send(socketEngine s, object ToSend)
        {
            byte[] buffer = new byte[] { byte.MinValue };
            int sendCount = 0;
            buffer = memoryStatic.serialize(ToSend);
            int length = buffer.Length;
            try
            {
                s.sendMessage(length.ToString());
                if (s.socket.Poll(3000000, SelectMode.SelectWrite) == true)
                {
                    MemoryStream ms = new MemoryStream(buffer);
                    buffer = new byte[memoryStatic.BLOCK_SIZE];
                    for (int i = 0; i <= length; i+=memoryStatic.BLOCK_SIZE)
                    {
                        ms.Read(buffer, 0, memoryStatic.BLOCK_SIZE);
                        sendCount = s.socket.Send(buffer, 0, memoryStatic.BLOCK_SIZE, SocketFlags.None);
                    }
                    ms.Close();
                }
                else
                    throw new ArgumentException("Error when send an object: error with socket");
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error when send an object: " + ex.Message);
            }
        }

        /// <summary>
        /// receive a object structure from specified socket
        /// </summary>
        /// <param name="s">
        /// socket to receive
        /// </param>
        /// <returns>
        /// array of received bytes
        /// </returns>
        static public byte[] Receive(socketEngine s)
        {
            byte[] buffer;
            int recCount = 0;
            try
            {
                string answer = s.receiveMessage();
                int length = Convert.ToInt32(answer);
                if (s.socket.Poll(3000000, SelectMode.SelectRead) == true)
                {

                    if (s.socket.Available > 0)
                    {
                        MemoryStream ms = new MemoryStream(memoryStatic.BLOCK_SIZE);
                        buffer = new byte[memoryStatic.BLOCK_SIZE];
                        for (int i = 0; i <= length; i += memoryStatic.BLOCK_SIZE)
                        {
                            recCount += s.socket.Receive(buffer, 0, memoryStatic.BLOCK_SIZE, SocketFlags.None);
                            ms.Write(buffer, 0, memoryStatic.BLOCK_SIZE);
                        }

                        buffer = ms.ToArray();
                        ms.Close();
                        return buffer;
                    }
                    else
                        throw new ArgumentException("No data to receive");
                }
                else
                    throw new ArgumentException("Error when receive an object: error with socket");
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error when receive an object: " + ex.Message);
            }
        }

        /// <summary>
        /// Deserialize a byte array to object
        /// </summary>
        /// <param name="buffer">
        /// Buffer to deserialize
        /// </param>
        static public object deSerialize(byte[] buffer)
        {
            MemoryStream ms = new MemoryStream(buffer);
            BinaryFormatter bd = new BinaryFormatter();
            try
            {
                object res = bd.Deserialize(ms);
                ms.Close();
                return res;
            }
            catch (Exception ex)
            {
                ms.Close();
                throw new ArgumentException("Deserialization error: " + ex.Message);
            }
        }

    #endregion
}


[Serializable]
public struct user
{
    public string login;
    public string password;
    public string status;
    public bool checkUser;
    public int priveleges;
}

[Serializable]
public struct miniature
{
    public string name;
    public int length;
    public byte[] file;
}

[Serializable]
public struct file_list
{
    public string[] fileNames;
    public int[] downloadNumbers;
}

public class userInfo
{
    #region public variables

        user currentUser = new user();
        engineCrypt crypt = new engineCrypt();
        public string login
        {
            get
            {
                if (currentUser.login != String.Empty)
                    return currentUser.login;
                else
                    throw new ArgumentException("Login is not specified");
            }
            set
            {
                if (value != String.Empty)
                    currentUser.login = value;
                else
                    throw new ArgumentException("Login can not be empty");
            }
        }
        public string password
        {
            get
            {
                if (currentUser.password != String.Empty)
                {
                    if (currentUser.login != String.Empty)
                    {
                        try
                        {
                            return crypt.DecryptStringAES(currentUser.password, currentUser.login);
                        }
                        catch (Exception ex)
                        {
                            throw new ArgumentException("Can not crypt password: " + ex.Message);
                        }
                    }
                    else
                        throw new ArgumentException("Login is not specified");
                }
                else
                    throw new ArgumentException("Password is not specified");
            }
            set
            {
                if (value != String.Empty)
                {
                    if (currentUser.login != String.Empty)
                    {
                        try
                        {
                            currentUser.password = crypt.EncryptStringAES(value, currentUser.login);
                        }
                        catch (Exception ex)
                        {
                            throw new ArgumentException("Can not decrypt password: " + ex.Message);
                        }
                    }
                    else
                        throw new ArgumentException("Login is not specified");
                }
                else
                    throw new ArgumentException("Password is not specified");
            }
        }
        public string status
        {
            get
            {
                if (currentUser.status != String.Empty)
                    return currentUser.status;
                else
                    throw new ArgumentException("Status is not specified");
            }
            set
            {
                if (value != String.Empty)
                    currentUser.status = value;
                else
                    throw new ArgumentException("Status can not be empty");
            }
        }
        public bool checkUser
        {
            get
            {
                return currentUser.checkUser;
            }
            set
            {
                currentUser.checkUser = value;
            }
        }
        public int priveleges
        {
            get
            {
                return currentUser.priveleges;
            }
            set
            {
                if (value >= 0)
                    currentUser.priveleges = value;
                else
                    currentUser.priveleges = 5;
            }
        }

    #endregion

    #region constructors/destructors

        public userInfo()
        {
            currentUser.login = String.Empty;
            currentUser.password = String.Empty;
            currentUser.status = String.Empty;
            currentUser.checkUser = false;
            currentUser.priveleges = 5;
        }

    #endregion

    #region public functions
       
        public void Send(socketEngine s)
        {
            memoryStatic.Send(s, currentUser);
        }

        public void Receive(socketEngine s)
        {
            byte[] buffer = memoryStatic.Receive(s);
            if (buffer.Length > 0)
                currentUser =(user)memoryStatic.deSerialize(buffer);
            else
                throw new ArgumentException("Error when receive User Info: can not obtain data");
        }
        
    #endregion
}

public class miniatures
{
    #region public variables

        public miniature[] listOfMinis;
        public int count;

    #endregion

    #region public functions

    /// <summary>
    /// Creates a miniature from specified file
    /// </summary>
    /// <param name="filePath">path to file</param>
    /// <returns>1 if its success and 0 if else</returns>
    public int createMini (string filePath, string pathToSmall)
    {
        try
        {
            string extension = Path.GetExtension(filePath).ToLower();
            if (extension.Contains(".bmp") || extension.Contains(".jpg") ||
                extension.Contains(".jpeg") || extension.Contains(".png"))
            {
                Bitmap imageToConv = new Bitmap(filePath);
                int newHeight = 0;
                int newWidth = 0;
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
                if (imageToConv.Height == imageToConv.Width)
                {
                    newHeight = 234; newWidth = 365;
                }

                Bitmap bm = new Bitmap(imageToConv, newWidth, newHeight);
                imageToConv.Dispose();
                string pathToSave = pathToSmall;
                if (filePath.ToLower().Contains(".bmp"))
                    bm.Save(pathToSave, System.Drawing.Imaging.ImageFormat.Bmp);
                if (filePath.ToLower().Contains(".jpg") || filePath.Contains(".jpeg"))
                    bm.Save(pathToSave, System.Drawing.Imaging.ImageFormat.Jpeg);
                if (filePath.ToLower().Contains(".png"))
                    bm.Save(pathToSave, System.Drawing.Imaging.ImageFormat.Png);
            }
            return 1;
        }
        catch(Exception ex)
        {
            throw new ArgumentException("Error when creating miniature: " + ex.Message);
        }
    }

    /// <summary>
    /// generate or update miniatures in specified directory
    /// </summary>
    /// <param name="directory">directory with images to update</param>
    public void updateMiniatures(string directory)
    {
        try
        {
            string[] files = Directory.GetFiles(directory);
            count = 0;
            foreach (string str in files)
            {
                string pathToSmall = directory + "/TEMP/" + Path.GetFileName(str) + ".smll";
                if (File.Exists(directory + "/TEMP/" + Path.GetFileName(str) + ".smll") == false)
                {
                    count += createMini(str, pathToSmall);
                }
                else
                    count++;
            }

            listOfMinis = new miniature[count];
            files = Directory.GetFiles(directory + "/TEMP/");
            int index = 0;
            foreach (string str in files)
            {
                if (Path.GetExtension(str).Contains(".smll") == true)
                {
                    listOfMinis[index].name = Path.GetFileName(str);

                    FileStream st = File.OpenRead(str);
                    listOfMinis[index].length = Convert.ToInt32(st.Length);
                    listOfMinis[index].file = new byte[listOfMinis[index].length];
                    st.Read(listOfMinis[index].file, 0, listOfMinis[index].length);
                    st.Close();
                    index++;
                }
            }
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Error when updating miniatures: " + ex.Message);
        }
    }

    public void sendMiniatures(socketEngine s)
    {
        try
        {
            memoryStatic.Send(s, listOfMinis);
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Error when send miniatures: " + ex.Message);
        }
    }

    public void receiveMiniatures(socketEngine s)
    {
        byte[] buffer;
        listOfMinis = null;
        try
        {
            buffer = memoryStatic.Receive(s);
            if (buffer.Length > 0)
                listOfMinis = (miniature[])memoryStatic.deSerialize(buffer);
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Error when receive minaitures: " + ex.Message);
        }
    }

    public void saveMiniatures(string path)
    {
        string copyPath = path;
        for (int i = 0; i < listOfMinis.Length; i++)
        {
            if (listOfMinis[i].name != String.Empty)
            {
                path = copyPath + listOfMinis[i].name;
                FileStream fs = File.OpenWrite(path);
                fs.Write(listOfMinis[i].file, 0, listOfMinis[i].file.Length);
                fs.Close();
            }
        }
    }

    #endregion
}

public class files
{
    public file_list listOfFiles;

    /// <summary>
    /// save all names of files to listOfFiles from specified path
    /// </summary>
    /// <param name="path">path to check</param>
    public void updateFileList(string path)
    {
        try
        {
            string[] files = Directory.GetFiles(path);
            {
                listOfFiles.fileNames = new string[files.Length];
                listOfFiles.downloadNumbers = new int[files.Length];
                int i = 0;
                foreach (string file in files)
                {
                    listOfFiles.fileNames[i] = Path.GetFileName(file);
                    string pathToInf = path + "/TEMP/" + listOfFiles.fileNames[i] + ".finf";
                    if (File.Exists(pathToInf) == true)
                    {
                        FileStream fs = File.OpenRead(pathToInf);
                        byte[] buffer = new byte[Convert.ToInt32(fs.Length)];
                        fs.Read(buffer, 0, Convert.ToInt32(fs.Length));
                        listOfFiles.downloadNumbers[i] = Convert.ToInt32(Encoding.UTF8.GetString(buffer));
                        fs.Close();
                    }
                    else
                    {
                        listOfFiles.downloadNumbers[i] = 0;
                    }
                    i++;
                }
            }
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Error when updating file list: " + ex.Message);
        }
    }

    public void sendFileList(socketEngine s)
    {
        try
        {
            memoryStatic.Send(s, listOfFiles);
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Error when sending file list: " + ex.Message);
        }
    }

    public void receiveFileList(socketEngine s)
    {
        try
        {
            byte[] buffer = memoryStatic.Receive(s);
            listOfFiles = (file_list)memoryStatic.deSerialize(buffer);
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Error when receiving file list: " + ex.Message);
        }
    }
}