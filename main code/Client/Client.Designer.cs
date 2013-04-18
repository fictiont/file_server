namespace Client
{
    partial class ClientWindow
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupServerControls = new System.Windows.Forms.GroupBox();
            this.update = new System.Windows.Forms.Button();
            this.fileMode = new System.Windows.Forms.TabControl();
            this.Download = new System.Windows.Forms.TabPage();
            this.searchText = new System.Windows.Forms.TextBox();
            this.serverFileTree = new System.Windows.Forms.DataGridView();
            this.Files = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.downloadNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.saveFileControls = new System.Windows.Forms.GroupBox();
            this.testLabel = new System.Windows.Forms.Label();
            this.downloadProgress = new System.Windows.Forms.ProgressBar();
            this.uploadFileSave = new System.Windows.Forms.Button();
            this.chooseDir = new System.Windows.Forms.Button();
            this.savePath = new System.Windows.Forms.TextBox();
            this.Upload = new System.Windows.Forms.TabPage();
            this.groupFileControls = new System.Windows.Forms.GroupBox();
            this.sendStatus1 = new System.Windows.Forms.Label();
            this.uploadProgress = new System.Windows.Forms.ProgressBar();
            this.uploadFile = new System.Windows.Forms.Button();
            this.uploadFileName = new System.Windows.Forms.TextBox();
            this.chooseFile = new System.Windows.Forms.Button();
            this.chooseUploadFile = new System.Windows.Forms.OpenFileDialog();
            this.saveDownloadFile = new System.Windows.Forms.FolderBrowserDialog();
            this.previewPicture = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.eventLog1 = new System.Diagnostics.EventLog();
            this.name = new System.Windows.Forms.Label();
            this.privileges = new System.Windows.Forms.Label();
            this.Administrator = new System.Windows.Forms.TabPage();
            this.filesDelBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.filesDelTxt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupServerControls.SuspendLayout();
            this.fileMode.SuspendLayout();
            this.Download.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.serverFileTree)).BeginInit();
            this.saveFileControls.SuspendLayout();
            this.Upload.SuspendLayout();
            this.groupFileControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.previewPicture)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.eventLog1)).BeginInit();
            this.Administrator.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupServerControls
            // 
            this.groupServerControls.Controls.Add(this.privileges);
            this.groupServerControls.Controls.Add(this.name);
            this.groupServerControls.Controls.Add(this.update);
            this.groupServerControls.Location = new System.Drawing.Point(480, 32);
            this.groupServerControls.Name = "groupServerControls";
            this.groupServerControls.Size = new System.Drawing.Size(389, 83);
            this.groupServerControls.TabIndex = 1;
            this.groupServerControls.TabStop = false;
            this.groupServerControls.Text = "Серверные опции";
            // 
            // update
            // 
            this.update.Location = new System.Drawing.Point(6, 45);
            this.update.Name = "update";
            this.update.Size = new System.Drawing.Size(159, 23);
            this.update.TabIndex = 9;
            this.update.Text = "Обновить список файлов";
            this.update.UseVisualStyleBackColor = true;
            this.update.Click += new System.EventHandler(this.update_Click);
            // 
            // fileMode
            // 
            this.fileMode.Controls.Add(this.Download);
            this.fileMode.Controls.Add(this.Upload);
            this.fileMode.Controls.Add(this.Administrator);
            this.fileMode.Location = new System.Drawing.Point(12, 10);
            this.fileMode.Name = "fileMode";
            this.fileMode.SelectedIndex = 0;
            this.fileMode.Size = new System.Drawing.Size(462, 395);
            this.fileMode.TabIndex = 2;
            // 
            // Download
            // 
            this.Download.Controls.Add(this.searchText);
            this.Download.Controls.Add(this.serverFileTree);
            this.Download.Controls.Add(this.saveFileControls);
            this.Download.Location = new System.Drawing.Point(4, 22);
            this.Download.Name = "Download";
            this.Download.Padding = new System.Windows.Forms.Padding(3);
            this.Download.Size = new System.Drawing.Size(454, 369);
            this.Download.TabIndex = 0;
            this.Download.Text = "Download";
            this.Download.UseVisualStyleBackColor = true;
            // 
            // searchText
            // 
            this.searchText.Location = new System.Drawing.Point(6, 15);
            this.searchText.Name = "searchText";
            this.searchText.Size = new System.Drawing.Size(448, 20);
            this.searchText.TabIndex = 6;
            this.searchText.Text = "Text to search...";
            this.searchText.TextChanged += new System.EventHandler(this.searchText_TextChanged);
            this.searchText.Enter += new System.EventHandler(this.searchText_Enter);
            this.searchText.Leave += new System.EventHandler(this.searchText_Leave);
            // 
            // serverFileTree
            // 
            this.serverFileTree.AllowUserToAddRows = false;
            this.serverFileTree.AllowUserToDeleteRows = false;
            this.serverFileTree.AllowUserToResizeColumns = false;
            this.serverFileTree.AllowUserToResizeRows = false;
            this.serverFileTree.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.serverFileTree.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.serverFileTree.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Files,
            this.downloadNumber});
            this.serverFileTree.Cursor = System.Windows.Forms.Cursors.Hand;
            this.serverFileTree.GridColor = System.Drawing.SystemColors.HotTrack;
            this.serverFileTree.Location = new System.Drawing.Point(6, 35);
            this.serverFileTree.MultiSelect = false;
            this.serverFileTree.Name = "serverFileTree";
            this.serverFileTree.ReadOnly = true;
            this.serverFileTree.RowHeadersVisible = false;
            this.serverFileTree.Size = new System.Drawing.Size(448, 202);
            this.serverFileTree.TabIndex = 5;
            this.serverFileTree.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.serverFileTree_CellClick);
            this.serverFileTree.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.serverFileTree_RowEnter);
            // 
            // Files
            // 
            this.Files.HeaderText = "Files";
            this.Files.Name = "Files";
            this.Files.ReadOnly = true;
            this.Files.Width = 317;
            // 
            // downloadNumber
            // 
            this.downloadNumber.HeaderText = "Downloads number";
            this.downloadNumber.Name = "downloadNumber";
            this.downloadNumber.ReadOnly = true;
            this.downloadNumber.Width = 109;
            // 
            // saveFileControls
            // 
            this.saveFileControls.Controls.Add(this.testLabel);
            this.saveFileControls.Controls.Add(this.downloadProgress);
            this.saveFileControls.Controls.Add(this.uploadFileSave);
            this.saveFileControls.Controls.Add(this.chooseDir);
            this.saveFileControls.Controls.Add(this.savePath);
            this.saveFileControls.Enabled = false;
            this.saveFileControls.Location = new System.Drawing.Point(6, 243);
            this.saveFileControls.Name = "saveFileControls";
            this.saveFileControls.Size = new System.Drawing.Size(442, 120);
            this.saveFileControls.TabIndex = 4;
            this.saveFileControls.TabStop = false;
            this.saveFileControls.Text = "Save File Controls";
            // 
            // testLabel
            // 
            this.testLabel.AutoSize = true;
            this.testLabel.Location = new System.Drawing.Point(189, 51);
            this.testLabel.Name = "testLabel";
            this.testLabel.Size = new System.Drawing.Size(0, 13);
            this.testLabel.TabIndex = 5;
            this.testLabel.Tag = "";
            this.testLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // downloadProgress
            // 
            this.downloadProgress.Location = new System.Drawing.Point(7, 75);
            this.downloadProgress.Name = "downloadProgress";
            this.downloadProgress.Size = new System.Drawing.Size(429, 39);
            this.downloadProgress.TabIndex = 4;
            // 
            // uploadFileSave
            // 
            this.uploadFileSave.Enabled = false;
            this.uploadFileSave.Location = new System.Drawing.Point(321, 46);
            this.uploadFileSave.Name = "uploadFileSave";
            this.uploadFileSave.Size = new System.Drawing.Size(114, 23);
            this.uploadFileSave.TabIndex = 1;
            this.uploadFileSave.Text = "Загрузить";
            this.uploadFileSave.UseVisualStyleBackColor = true;
            this.uploadFileSave.Click += new System.EventHandler(this.downloadFileSave_Click);
            // 
            // chooseDir
            // 
            this.chooseDir.Location = new System.Drawing.Point(398, 16);
            this.chooseDir.Name = "chooseDir";
            this.chooseDir.Size = new System.Drawing.Size(38, 23);
            this.chooseDir.TabIndex = 3;
            this.chooseDir.Text = "...";
            this.chooseDir.UseVisualStyleBackColor = true;
            this.chooseDir.Click += new System.EventHandler(this.chooseDir_Click);
            // 
            // savePath
            // 
            this.savePath.Location = new System.Drawing.Point(6, 19);
            this.savePath.Name = "savePath";
            this.savePath.Size = new System.Drawing.Size(385, 20);
            this.savePath.TabIndex = 2;
            this.savePath.Text = "Path to save file...";
            // 
            // Upload
            // 
            this.Upload.Controls.Add(this.groupFileControls);
            this.Upload.Location = new System.Drawing.Point(4, 22);
            this.Upload.Name = "Upload";
            this.Upload.Padding = new System.Windows.Forms.Padding(3);
            this.Upload.Size = new System.Drawing.Size(454, 369);
            this.Upload.TabIndex = 1;
            this.Upload.Text = "Upload";
            this.Upload.UseVisualStyleBackColor = true;
            // 
            // groupFileControls
            // 
            this.groupFileControls.Controls.Add(this.sendStatus1);
            this.groupFileControls.Controls.Add(this.uploadProgress);
            this.groupFileControls.Controls.Add(this.uploadFile);
            this.groupFileControls.Controls.Add(this.uploadFileName);
            this.groupFileControls.Controls.Add(this.chooseFile);
            this.groupFileControls.Location = new System.Drawing.Point(6, 6);
            this.groupFileControls.Name = "groupFileControls";
            this.groupFileControls.Size = new System.Drawing.Size(390, 176);
            this.groupFileControls.TabIndex = 4;
            this.groupFileControls.TabStop = false;
            this.groupFileControls.Text = "File controls";
            // 
            // sendStatus1
            // 
            this.sendStatus1.AutoSize = true;
            this.sendStatus1.Location = new System.Drawing.Point(144, 89);
            this.sendStatus1.Name = "sendStatus1";
            this.sendStatus1.Size = new System.Drawing.Size(0, 13);
            this.sendStatus1.TabIndex = 4;
            this.sendStatus1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uploadProgress
            // 
            this.uploadProgress.Location = new System.Drawing.Point(6, 122);
            this.uploadProgress.Name = "uploadProgress";
            this.uploadProgress.Size = new System.Drawing.Size(378, 48);
            this.uploadProgress.TabIndex = 3;
            // 
            // uploadFile
            // 
            this.uploadFile.Enabled = false;
            this.uploadFile.Location = new System.Drawing.Point(230, 84);
            this.uploadFile.Name = "uploadFile";
            this.uploadFile.Size = new System.Drawing.Size(154, 23);
            this.uploadFile.TabIndex = 2;
            this.uploadFile.Text = "Закачать файл на сервер";
            this.uploadFile.UseVisualStyleBackColor = true;
            this.uploadFile.Click += new System.EventHandler(this.uploadFile_Click);
            // 
            // uploadFileName
            // 
            this.uploadFileName.Location = new System.Drawing.Point(6, 29);
            this.uploadFileName.Name = "uploadFileName";
            this.uploadFileName.Size = new System.Drawing.Size(378, 20);
            this.uploadFileName.TabIndex = 1;
            // 
            // chooseFile
            // 
            this.chooseFile.Location = new System.Drawing.Point(230, 54);
            this.chooseFile.Name = "chooseFile";
            this.chooseFile.Size = new System.Drawing.Size(154, 23);
            this.chooseFile.TabIndex = 0;
            this.chooseFile.Text = "Выбрать файл";
            this.chooseFile.UseVisualStyleBackColor = true;
            this.chooseFile.Click += new System.EventHandler(this.chooseFile_Click);
            // 
            // chooseUploadFile
            // 
            this.chooseUploadFile.FileName = "Choose file to upload...";
            this.chooseUploadFile.RestoreDirectory = true;
            this.chooseUploadFile.FileOk += new System.ComponentModel.CancelEventHandler(this.chooseUploadFile_FileOk);
            // 
            // previewPicture
            // 
            this.previewPicture.Location = new System.Drawing.Point(15, 19);
            this.previewPicture.Name = "previewPicture";
            this.previewPicture.Size = new System.Drawing.Size(365, 234);
            this.previewPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.previewPicture.TabIndex = 3;
            this.previewPicture.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.previewPicture);
            this.groupBox1.Location = new System.Drawing.Point(483, 130);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(386, 259);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Превью";
            // 
            // eventLog1
            // 
            this.eventLog1.SynchronizingObject = this;
            // 
            // name
            // 
            this.name.AutoSize = true;
            this.name.Location = new System.Drawing.Point(135, 22);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(39, 13);
            this.name.TabIndex = 10;
            this.name.Text = "default";
            // 
            // privileges
            // 
            this.privileges.AutoSize = true;
            this.privileges.Location = new System.Drawing.Point(6, 22);
            this.privileges.Name = "privileges";
            this.privileges.Size = new System.Drawing.Size(80, 13);
            this.privileges.TabIndex = 11;
            this.privileges.Text = "Пользователь";
            // 
            // Administrator
            // 
            this.Administrator.Controls.Add(this.label2);
            this.Administrator.Controls.Add(this.filesDelTxt);
            this.Administrator.Controls.Add(this.label1);
            this.Administrator.Controls.Add(this.filesDelBtn);
            this.Administrator.Location = new System.Drawing.Point(4, 22);
            this.Administrator.Name = "Administrator";
            this.Administrator.Size = new System.Drawing.Size(454, 369);
            this.Administrator.TabIndex = 2;
            this.Administrator.Text = "Administrator";
            this.Administrator.UseVisualStyleBackColor = true;
            // 
            // filesDelBtn
            // 
            this.filesDelBtn.Location = new System.Drawing.Point(14, 22);
            this.filesDelBtn.Name = "filesDelBtn";
            this.filesDelBtn.Size = new System.Drawing.Size(75, 23);
            this.filesDelBtn.TabIndex = 0;
            this.filesDelBtn.Text = "Удалить";
            this.filesDelBtn.UseVisualStyleBackColor = true;
            this.filesDelBtn.Click += new System.EventHandler(this.filesDel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(95, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(233, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "все файлы, которые не скачивались более: ";
            // 
            // filesDelTxt
            // 
            this.filesDelTxt.Location = new System.Drawing.Point(323, 22);
            this.filesDelTxt.MaxLength = 5;
            this.filesDelTxt.Name = "filesDelTxt";
            this.filesDelTxt.Size = new System.Drawing.Size(53, 20);
            this.filesDelTxt.TabIndex = 2;
            this.filesDelTxt.Text = "1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(382, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "дней";
            // 
            // ClientWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(887, 410);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.fileMode);
            this.Controls.Add(this.groupServerControls);
            this.Name = "ClientWindow";
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Client";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClientWindow_FormClosing);
            this.Load += new System.EventHandler(this.ClientWindow_Load);
            this.groupServerControls.ResumeLayout(false);
            this.groupServerControls.PerformLayout();
            this.fileMode.ResumeLayout(false);
            this.Download.ResumeLayout(false);
            this.Download.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.serverFileTree)).EndInit();
            this.saveFileControls.ResumeLayout(false);
            this.saveFileControls.PerformLayout();
            this.Upload.ResumeLayout(false);
            this.groupFileControls.ResumeLayout(false);
            this.groupFileControls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.previewPicture)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.eventLog1)).EndInit();
            this.Administrator.ResumeLayout(false);
            this.Administrator.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupServerControls;
        private System.Windows.Forms.TabControl fileMode;
        private System.Windows.Forms.TabPage Download;
        private System.Windows.Forms.TabPage Upload;
        private System.Windows.Forms.GroupBox groupFileControls;
        private System.Windows.Forms.TextBox uploadFileName;
        private System.Windows.Forms.Button chooseFile;
        private System.Windows.Forms.Button uploadFile;
        private System.Windows.Forms.ProgressBar uploadProgress;
        private System.Windows.Forms.OpenFileDialog chooseUploadFile;
        private System.Windows.Forms.Label sendStatus1;
        private System.Windows.Forms.Button chooseDir;
        private System.Windows.Forms.TextBox savePath;
        private System.Windows.Forms.Button uploadFileSave;
        private System.Windows.Forms.GroupBox saveFileControls;
        private System.Windows.Forms.ProgressBar downloadProgress;
        private System.Windows.Forms.FolderBrowserDialog saveDownloadFile;
        private System.Windows.Forms.Label testLabel;
        private System.Windows.Forms.Button update;
        private System.Windows.Forms.DataGridView serverFileTree;
        private System.Windows.Forms.DataGridViewTextBoxColumn Files;
        private System.Windows.Forms.DataGridViewTextBoxColumn downloadNumber;
        private System.Windows.Forms.PictureBox previewPicture;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox searchText;
        private System.Diagnostics.EventLog eventLog1;
        private System.Windows.Forms.Label privileges;
        private System.Windows.Forms.Label name;
        private System.Windows.Forms.TabPage Administrator;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox filesDelTxt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button filesDelBtn;
    }
}

