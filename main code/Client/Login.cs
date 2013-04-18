using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace Client
{
    public partial class Authentication : Form
    {
        int BLOCK_SIZE = 2048;
        socketEngine mySock = new socketEngine();
        captcha myCaptch = new captcha();
        userInfo currentUser = new userInfo();

        public Authentication()
        {
            InitializeComponent();
        }

        private void updateMiniatures(int count)
        {
            connectProgress.Value++;
            mySock.sendMessage("name");
            string path = mySock.receiveMessage();
            mySock.sendMessage("length");
            long length = 0;
            try
            {
                length = Convert.ToInt64(mySock.receiveMessage());
            }
            catch
            {
                MessageBox.Show("Ошибка при передаче");
            }
            mySock.sendMessage("file");
            FileStream fl = File.OpenWrite(path);
            Console.WriteLine(length.ToString());
            for (int i = 0; i < length; i += BLOCK_SIZE)
            {
                byte[] bt = new byte[BLOCK_SIZE];
                mySock.socket.Receive(bt, 0, BLOCK_SIZE, System.Net.Sockets.SocketFlags.None);
                fl.Write(bt, 0, BLOCK_SIZE);
            }
            fl.Close();
        }

        private void loginBtn_Click(object sender, EventArgs e)
        {
            loginBtn.Enabled = false;
            regBtn.Enabled = false;
            sendLoginData();
            connectUser();
            loginBtn.Enabled = true;
            regBtn.Enabled = true;
            /*int count = 0;
            if (captchaText.Text == myCaptch.text)
            {
                if (mySock.socket.Connected == false)
                {
                    try
                    {
                        mySock.connect();
                    }
                    catch
                    {
                    }
                }
                if (mySock.socket.Connected == true)
                {
                    string message = "";
                    mySock.sendMessage("authentication");
                    while (message.Contains("confirmed") == false && message.Contains("failed") == false)
                    {
                        message = mySock.receiveMessage();
                        switch (message)
                        {
                            case "login":
                                mySock.sendMessage(login.Text);
                                break;
                            case "password":
                                mySock.sendMessage(password.Text);
                                break;
                            case "confirmed":
                                mySock.sendMessage("connect");
                                if (Directory.Exists(login.Text + "/TEMP") == false)
                                    Directory.CreateDirectory(login.Text + "/TEMP");
                                while (message.Contains("connected") == false)
                                {
                                    message = mySock.receiveMessage();
                                    switch (message)
                                    {
                                        case "privileges":
                                            mySock.sendMessage("OK");
                                            memoryStatic.privileges = Convert.ToInt32(mySock.receiveMessage());
                                            mySock.sendMessage("OK");
                                            Console.WriteLine(memoryStatic.privileges.ToString());
                                        break;
                                        case "name":
                                            mySock.sendMessage(login.Text);
                                            break;
                                        case "miniature":
                                            updateMiniatures(count);
                                        break;
                                        case "count":
                                            mySock.sendMessage("OK");
                                            count = Convert.ToInt32(mySock.receiveMessage());
                                            mySock.sendMessage("OK");
                                            connectProgress.Maximum = count;
                                        break;
                                        case "connected":
                                            ClientWindow client = new ClientWindow();
                                            memoryStatic.login = login.Text;
                                            memoryStatic.sock = mySock;
                                            this.Visible = false;
                                            client.ShowDialog();
                                            login.Text = String.Empty;
                                            password.Text = String.Empty;
                                            captchaText.Text = String.Empty;
                                            captchPicture.Image = myCaptch.CreateImage(captchPicture.Width, captchPicture.Height);
                                            connectProgress.Value = 0;
                                            this.Visible = true;
                                            break;
                                    }
                                }
                                message = "confirmed";
                                break;
                            case "failed":
                                mySock.sendMessage("why");
                                MessageBox.Show(mySock.receiveMessage());
                                break;
                        }
                    }
                }
                else
                    MessageBox.Show("Не получилось подключиться к серверу, проверьте ваше соединение");
            }
            else
            {
                MessageBox.Show("Неправильно введен код с картинки");
                captchPicture.Image = myCaptch.CreateImage(captchPicture.Width, captchPicture.Height);
            }
            loginBtn.Enabled = true;
            regBtn.Enabled = true; */
        }

        private void sendLoginData()
        {
            try
            {
                if (mySock.socket.Connected == false)
                    mySock.connect();
                currentUser.login = login.Text;
                currentUser.password = password.Text;
                mySock.sendMessage("accept user");
                if (mySock.receiveMessage().CompareTo("ready to accept user") == 0)
                {
                    currentUser.Send(mySock.socket);
                    string answer = mySock.receiveMessage();
                    if (answer.CompareTo("successfully accept user") != 0)
                        MessageBox.Show(answer);
                }
                else
                {
                    MessageBox.Show("Error: Check your network connection");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error when login: " + ex.Message);
            }
        }

        private void connectUser()
        {
            try
            {
                if (mySock.socket.Connected == false)
                {
                    mySock.connect();
                }
                mySock.sendMessage("check user");
                string answer = mySock.receiveMessage();
                if (answer.CompareTo("take user info") == 0)
                {
                    currentUser.Receive(mySock.socket);
                    if (currentUser.checkUser == true)
                    {
                        openClient();
                    }
                    else
                    {
                        MessageBox.Show(currentUser.status);
                    }
                }
                else
                {
                    MessageBox.Show(answer);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error when checking user: " + ex.Message);
            }

        }

        private void openClient()
        {
            try
            {
                if (Directory.Exists(currentUser.login + "/TEMP") == false)
                    Directory.CreateDirectory(currentUser.login + "/TEMP");
                ClientWindow client = new ClientWindow();
                memoryStatic.login = currentUser.login;
                memoryStatic.sock = mySock;
                this.Visible = false;
                client.ShowDialog();
                login.Text = String.Empty;
                password.Text = String.Empty;
                captchaText.Text = String.Empty;
                captchPicture.Image = myCaptch.CreateImage(captchPicture.Width, captchPicture.Height);
                connectProgress.Value = 0;
                this.Visible = true;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error when opening client: " + ex.Message);
            }
        }

        private void Authentication_Load(object sender, EventArgs e)
        {
            Console.WriteLine("CREATED");
            captchPicture.Image = myCaptch.CreateImage(captchPicture.Width, captchPicture.Height);
        }

        private void captchPicture_Click(object sender, EventArgs e)
        {
            captchPicture.Image = myCaptch.CreateImage(captchPicture.Width, captchPicture.Height);
        }

        private void regBtn_Click(object sender, EventArgs e)
        {
            if (password.Text.Length < 4)
                MessageBox.Show("Пароль должен содержать более 4-х символов");
            else
            {
                if (login.Text.Length < 4)
                    MessageBox.Show("Логин должен содержать более 4-х символов");
                else
                {
                    if (login.Text.IndexOfAny(Path.GetInvalidFileNameChars()) == -1)
                    {
                        if (captchaText.Text == myCaptch.text)
                        {
                            if (mySock.socket.Connected == false)
                            {
                                try
                                {
                                    mySock.connect();
                                }
                                catch
                                {
                                    MessageBox.Show("Не получилось подключиться к серверу, проверьте ваше соединение");
                                }
                            }
                            if (mySock.socket.Connected == true)
                            {
                                mySock.sendMessage("registration");
                                string msg = "";
                                while (msg.Contains("end") != true)
                                {
                                    msg = mySock.receiveMessage();
                                    switch (msg)
                                    {
                                        case "login":
                                            mySock.sendMessage(login.Text);
                                            break;
                                        case "pwd":
                                            mySock.sendMessage(password.Text);
                                            break;
                                        case "failed":
                                            mySock.sendMessage("why");
                                            MessageBox.Show(mySock.receiveMessage());
                                            break;
                                        case "success":
                                            MessageBox.Show("Регистрация прошла успешно. Теперь вы можете зайти под своим логином");
                                            break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Неправильно введен код с картинки");
                            captchPicture.Image = myCaptch.CreateImage(captchPicture.Width, captchPicture.Height);
                        }
                    }
                    else
                        MessageBox.Show("Логин не может содержать специальные символы");
                }
            }
        }

    }
}
