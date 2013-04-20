using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class socketEngine
{
    #region private variables

        private Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private IPEndPoint Addr = null;
        private int clientsN = 10;
        private Mutex mtx = new Mutex();

    #endregion

    #region public variables

        /// <summary>
        /// Set mutex for multithreads applications
        /// </summary>
        public Mutex mutex
        {
            set
            {
                mtx = value;
            }
        }

        /// <summary>
        /// Returns or obtain number of available connections.
        /// </summary>
        public int clientsNumber
        {
            get
            {
                return clientsN;
            }
            set
            {
                if (value >= 0)
                    clientsN = value;
                else
                    clientsN = 10;
            }
        }

        /// <summary>
        /// Return a used socket.
        /// </summary>
        public Socket socket
        {
            get
            {
                return s;
            }
            set
            {
                s = value;
            }
        }

    #endregion

    #region public functions

        public void ConsoleWrite(string stringToWrite)
        {
            if (mtx != null)
            {
                mtx.WaitOne();
                Console.WriteLine(stringToWrite);
                mtx.ReleaseMutex();
            }
            else
                throw new ArgumentException("Specify a mutex please");
        }

        ~socketEngine()
        {
            s.Close();
        }

        /// <summary>
        /// Creates new exemplar with a default IPAddress
        /// </summary>
        public socketEngine()
        {
            try
            {
                Addr = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3128);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("IP address is not valid: " + ex.Message);
            }
        }

        /// <summary>
        /// Creates new exemplar with specified IPAddress
        /// </summary>
        /// <param name="ipAddress">Ip address</param>
        public socketEngine(IPAddress ipAddress)
        {
            try
            {
                Addr = new IPEndPoint(ipAddress, 3128);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Can not use specified IPaddress: " + ipAddress.ToString() + " becuse of: " + ex.Message);
            }
        }

        /// <summary>
        /// Starts server.
        /// </summary>
        public void start()
        {
            try
            {
                s.Bind(Addr);
                s.Listen(clientsN);
            }
            catch(Exception ex)
            {
                throw new ArgumentException("Can not connect to specified IPAddress: " + ex.Message);
            }
        }

        /// <summary>
        /// Connect to started server.
        /// </summary>
        public void connect()
        {
            try
            {
                s.Connect(Addr);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Can not connect to specified IPAddress: " + ex.Message);
            }
        }

        /// <summary>
        /// Wait for clients to connect.
        /// </summary>
        /// <returns>
        /// Socket of connected client;
        /// </returns>
        public Socket waitForClient()
        {
            try
            {
                return s.Accept();
            }
            catch(Exception ex)
            {
                throw new ArgumentException("Can not accept client: " + ex.Message);
            }
        }

        /// <summary>
        /// Receive message from specified socket.
        /// </summary>
        /// <returns>
        /// String of received message
        /// </returns>
        public string receiveMessage()
        {
            string result = String.Empty;
            if (s.Poll(3000000, SelectMode.SelectRead) == true)
            {
                if (s.Available == 0)
                    return result;
                int buffCnt = s.Available;
                int recCnt = 0;
                byte[] msg = new byte[buffCnt];
                try
                {
                    while (recCnt != buffCnt)
                        recCnt += s.Receive(msg, recCnt, buffCnt - recCnt, SocketFlags.None);
                    result = Encoding.UTF8.GetString(msg);
                    return result;
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("Connection aborted, receiving failed: " + ex.Message);
                }
            }
            else
                throw new ArgumentException("Connection is not available");
        }

        /// <summary>
        /// Send specified message to specified socket.
        /// </summary>
        /// <param name="message">
        /// Message to send.
        /// </param>
        public void sendMessage(string message)
        {
            if (s.Poll(3000000, SelectMode.SelectWrite) == true)
            {
                int messageLength = -1;
                try
                {
                    while (s.Available != 0)
                        receiveMessage();
                    byte[] msgToSent = Encoding.UTF8.GetBytes(message);
                    while (messageLength != msgToSent.Length)
                        messageLength = s.Send(msgToSent, 0, msgToSent.Length, SocketFlags.None);
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("Connection aborted, receiving failed: " + ex.Message);
                }
            }
            else
                throw new ArgumentException("Connection is not available");
        }

    #endregion
}