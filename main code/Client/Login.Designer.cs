namespace Client
{
    partial class Authentication
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.login = new System.Windows.Forms.TextBox();
            this.password = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.captchPicture = new System.Windows.Forms.PictureBox();
            this.loginBtn = new System.Windows.Forms.Button();
            this.regBtn = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.captchaText = new System.Windows.Forms.TextBox();
            this.connectProgress = new System.Windows.Forms.ProgressBar();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.captchPicture)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // login
            // 
            this.login.AcceptsTab = true;
            this.login.Location = new System.Drawing.Point(54, 25);
            this.login.MaxLength = 18;
            this.login.Name = "login";
            this.login.Size = new System.Drawing.Size(160, 20);
            this.login.TabIndex = 1;
            // 
            // password
            // 
            this.password.AcceptsTab = true;
            this.password.Location = new System.Drawing.Point(54, 51);
            this.password.MaxLength = 18;
            this.password.Name = "password";
            this.password.PasswordChar = '═';
            this.password.Size = new System.Drawing.Size(160, 20);
            this.password.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.login);
            this.groupBox1.Controls.Add(this.password);
            this.groupBox1.Location = new System.Drawing.Point(5, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(276, 89);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Login/register data";
            // 
            // captchPicture
            // 
            this.captchPicture.Location = new System.Drawing.Point(6, 19);
            this.captchPicture.Name = "captchPicture";
            this.captchPicture.Size = new System.Drawing.Size(137, 50);
            this.captchPicture.TabIndex = 3;
            this.captchPicture.TabStop = false;
            this.captchPicture.Click += new System.EventHandler(this.captchPicture_Click);
            // 
            // loginBtn
            // 
            this.loginBtn.Location = new System.Drawing.Point(180, 196);
            this.loginBtn.Name = "loginBtn";
            this.loginBtn.Size = new System.Drawing.Size(101, 23);
            this.loginBtn.TabIndex = 4;
            this.loginBtn.Text = "Login";
            this.loginBtn.UseVisualStyleBackColor = true;
            this.loginBtn.Click += new System.EventHandler(this.loginBtn_Click);
            // 
            // regBtn
            // 
            this.regBtn.Location = new System.Drawing.Point(2, 196);
            this.regBtn.Name = "regBtn";
            this.regBtn.Size = new System.Drawing.Size(101, 23);
            this.regBtn.TabIndex = 5;
            this.regBtn.Text = "Register";
            this.regBtn.UseVisualStyleBackColor = true;
            this.regBtn.Click += new System.EventHandler(this.regBtn_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.captchaText);
            this.groupBox2.Controls.Add(this.captchPicture);
            this.groupBox2.Location = new System.Drawing.Point(2, 111);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(279, 79);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Check for human";
            // 
            // captchaText
            // 
            this.captchaText.Location = new System.Drawing.Point(149, 34);
            this.captchaText.Name = "captchaText";
            this.captchaText.Size = new System.Drawing.Size(121, 20);
            this.captchaText.TabIndex = 3;
            // 
            // connectProgress
            // 
            this.connectProgress.Location = new System.Drawing.Point(2, 225);
            this.connectProgress.Name = "connectProgress";
            this.connectProgress.Size = new System.Drawing.Size(279, 23);
            this.connectProgress.TabIndex = 6;
            // 
            // Authentication
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 249);
            this.Controls.Add(this.connectProgress);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.regBtn);
            this.Controls.Add(this.loginBtn);
            this.Controls.Add(this.groupBox1);
            this.Name = "Authentication";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Authentication";
            this.Load += new System.EventHandler(this.Authentication_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.captchPicture)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox login;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox captchPicture;
        private System.Windows.Forms.Button loginBtn;
        private System.Windows.Forms.Button regBtn;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox captchaText;
        private System.Windows.Forms.ProgressBar connectProgress;
    }
}