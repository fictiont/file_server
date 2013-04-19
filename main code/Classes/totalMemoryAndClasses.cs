using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using System.Net;
using System.Net.Sockets;

public static class memoryStatic
{
    #region static variables

        static public string login;
        static public socketEngine sock;
        static public int privileges = 5;

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

        /// <summary>
        /// serialize a User Info structure 
        /// </summary>
        /// <returns>
        /// array of serialized bytes
        /// </returns>
        public byte[] serialize()
        {
            byte[] result;
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            try
            {
                bf.Serialize(ms, currentUser);
                result = ms.ToArray();
                ms.Close();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Serialization failed: " + ex.Message);
            }
            return result;
        }

        /// <summary>
        /// Deserialize a byte array to structure of Users Infos
        /// </summary>
        /// <param name="buffer">
        /// Buffer to deserialize
        /// </param>
        public void deSerialize(byte[] buffer)
        {
            MemoryStream ms = new MemoryStream(buffer);
            BinaryFormatter bd = new BinaryFormatter();
            try
            {
                currentUser = (user)bd.Deserialize(ms);
            }
            catch(Exception ex)
            {
                throw new ArgumentException("Deserialization error: " + ex.Message);
            }
        }
        
        /// <summary>
        /// send a serialized User Info structure to specified socket.
        /// </summary>
        /// <param name="s">
        /// socket to send
        /// </param>
        public void Send(Socket s)
        {
            byte[] buffer = new byte[]{byte.MinValue};
            int sendCount = 0;
            try
            {
                while (s.Available > 0)
                    s.Receive(buffer);
                if (s.Poll(3000000, SelectMode.SelectWrite) == true)
                {
                    buffer = serialize();

                    while (sendCount != buffer.Length)
                    {
                        sendCount = s.Send(buffer, sendCount, buffer.Length - sendCount, SocketFlags.None);
                    }
                }
                else
                    throw new ArgumentException("Error when send a User Info: error with socket");
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error when send a User Info: " + ex.Message);
            }
        }
        
        /// <summary>
        /// receive a User Info structure from specified socket
        /// </summary>
        /// <param name="s">
        /// socket to receive
        /// </param>
        public void Receive(Socket s)
        {
            byte[] buffer = new byte[]{byte.MinValue};
            int recCount = 0;
            try
            {
                if (s.Poll(3000000, SelectMode.SelectRead) == true)
                {
                    if (s.Available > 0)
                    {
                        int available = s.Available;
                        buffer = new byte[s.Available];
                        while (recCount != available)
                        {
                            recCount = s.Receive(buffer, recCount, s.Available, SocketFlags.None);
                        }
                        deSerialize(buffer);
                    }
                    else
                        throw new ArgumentException("No data to receive");
                }
                else
                    throw new ArgumentException("Error when receive a User Info: error with socket");
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error when receive a User Info: " + ex.Message);
            }
        }

    #endregion
}
