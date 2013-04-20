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
        socketEngine mySock = new socketEngine();
        captcha myCaptch = new captcha();
        userInfo currentUser = new userInfo();

        public Authentication()
        {
            InitializeComponent();
        }

        private void Authentication_Load(object sender, EventArgs e)
        {
            captchPicture.Image = myCaptch.CreateImage(captchPicture.Width, captchPicture.Height);
        }

        private void captchPicture_Click(object sender, EventArgs e)
        {
            captchPicture.Image = myCaptch.CreateImage(captchPicture.Width, captchPicture.Height);
        }

        private void loginBtn_Click(object sender, EventArgs e)
        {
            loginBtn.Enabled = false;
            regBtn.Enabled = false;
            if (captchaText.Text == myCaptch.text)
            {
                Login();
            }
            else
            {
                MessageBox.Show("Неправильно введен код с картинки");
                captchPicture.Image = myCaptch.CreateImage(captchPicture.Width, captchPicture.Height);
            }
            loginBtn.Enabled = true;
            regBtn.Enabled = true;
        }

        /// <summary>
        /// connect to server, check user and open client window
        /// </summary>
        private void Login()
        {
            try
            {
                if (mySock.socket.Connected == false)
                    mySock.connect();
                currentUser.login = login.Text;
                currentUser.password = password.Text;
                mySock.sendMessage("accept user");
                string answer = mySock.receiveMessage();
                if (answer.CompareTo("ready to accept user") == 0)
                {
                    currentUser.Send(mySock);
                    answer = mySock.receiveMessage();
                    if (answer.CompareTo("successfully accept user") == 0)
                        checkUser(); //check login and password
                    else
                        MessageBox.Show(answer);
                }
                else
                {
                    MessageBox.Show("Error:" + answer);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error when login: " + ex.Message);
            }
        }

        /// <summary>
        /// check user login and password
        /// </summary>
        private void checkUser()
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
                    mySock.sendMessage("ready to take");
                    currentUser.Receive(mySock);
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
                    MessageBox.Show("Error: " + answer);
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
                memoryStatic.privileges = currentUser.priveleges;
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

        private void regBtn_Click(object sender, EventArgs e)
        {
            if (password.Text.Length < 4)
                MessageBox.Show("Password must contains more than 4 symbols");
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
                            try
                            {
                                if (mySock.socket.Connected == false)
                                {
                                    mySock.connect();
                                }
                                mySock.sendMessage("registration");
                                string answer = mySock.receiveMessage();
                                if (answer.CompareTo("ready to registration") == 0)
                                {
                                    currentUser.login = login.Text;
                                    currentUser.password = password.Text;
                                    currentUser.Send(mySock);
                                    answer = mySock.receiveMessage();
                                    if (answer.CompareTo("successfully registration") == 0)
                                    {
                                        MessageBox.Show("You registered successfully. Now you can login.");
                                    }
                                    else
                                    {
                                        MessageBox.Show("Error: " + answer);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Error: " + answer);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error when register: " + ex.Message);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Incorrect captcha");
                            captchPicture.Image = myCaptch.CreateImage(captchPicture.Width, captchPicture.Height);
                        }
                    }
                    else
                        MessageBox.Show("Login cannot contains special chars");
                }
            }
        }

    }
}
