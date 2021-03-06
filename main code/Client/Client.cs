﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Timers;

namespace Client
{
    public partial class ClientWindow : Form
    {
        static bool workFlag = false;
        static int BLOCK_SIZE = 2048;
        files filesC;
        delegate void twoStringsList(string[] list, string[] list2);
        delegate void stringInt(string[] list, int[] list2);
        delegate void stringList(string[] list);
        delegate void stringOne(string str);
        delegate void integerVal(int Value);
        delegate void empty();
        socketEngine scktEngine;
        Bitmap notAv = new Bitmap("previewNotAvailable.bmp");
        string VuploadFileName, VuploadFilePath, VdownloadFileName, VdownloadFilePath;

    
        public ClientWindow()
        {
            InitializeComponent();
        }

        private void updateFileList()
        {
            try
            {
                miniatures minis = new miniatures();
                scktEngine.sendMessage("miniatures");
                string answer = scktEngine.receiveMessage();
                if (answer.CompareTo("take miniatures") == 0)
                {
                    scktEngine.sendMessage("start to take");
                    minis.receiveMiniatures(scktEngine);
                    answer = scktEngine.receiveMessage();
                    if (answer.CompareTo("successfully sended") == 0)
                        minis.saveMiniatures(memoryStatic.login + "/TEMP/");
                    else
                        MessageBox.Show(answer);
                }
                else
                    MessageBox.Show(answer);
                scktEngine.sendMessage("file list");
                filesC = new files();
                answer = scktEngine.receiveMessage();
                if (answer.CompareTo("take file list") == 0)
                {
                    scktEngine.sendMessage("ready to receive");
                    filesC.receiveFileList(scktEngine);
                    if (scktEngine.receiveMessage().CompareTo("successfull file list") != 0)
                    {
                        MessageBox.Show(answer);
                    }
                    else
                    {
                        stringInt lst = (files, numbers) =>
                        {
                            serverFileTree.Rows.Clear();
                            for (int i = 0; i < files.Length; i++)
                            {
                                serverFileTree.Rows.Add(1);
                                serverFileTree.Rows[i].Cells[0].Value = files[i];
                                serverFileTree.Rows[i].Cells[1].Value = numbers[i];
                            }
                            serverFileTree.Cursor = Cursors.Hand;
                        };

                        serverFileTree.Invoke(lst, filesC.listOfFiles.fileNames, filesC.listOfFiles.downloadNumbers);
                        empty enbl = () => { update.Enabled = true; };
                        update.Invoke(enbl);
                        empty sort = () =>
                        {
                            serverFileTree.Sort(serverFileTree.Columns[1], ListSortDirection.Descending);
                            update.Enabled = true;
                        };
                        serverFileTree.Invoke(sort);
                        workFlag = false;
                    }
                }
                else
                    MessageBox.Show(scktEngine.receiveMessage());
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error to update file list: " + ex.Message);
            }
            Thread.CurrentThread.Abort();
        }

        private void chooseFile_Click(object sender, EventArgs e)
        {
            chooseUploadFile.CheckFileExists = true;
            chooseUploadFile.ShowDialog();
        }

        private void uploadFile_Click(object sender, EventArgs e)
        {
            Thread uploadFileThread = new Thread(uploadFileF);
            workFlag = true;
            update.Enabled = false;
            uploadFileThread.Start();
            uploadFile.Enabled = false;
            saveFileControls.Enabled = false;
        }

        private void chooseUploadFile_FileOk(object sender, CancelEventArgs e)
        {
            VuploadFilePath = uploadFileName.Text = chooseUploadFile.FileName;
            VuploadFileName = Path.GetFileName(VuploadFilePath);
            uploadFile.Enabled = true;
        }

        private void uploadFileF()
        {
            byte[] buffer = new byte[BLOCK_SIZE];

            using (FileStream inputStream = File.OpenRead(VuploadFilePath))
            {

                long uploadFileLength = inputStream.Length;
                scktEngine.sendMessage("file sent");
                if (scktEngine.receiveMessage().Contains("name") == true)
                    scktEngine.sendMessage(VuploadFileName);
                if (scktEngine.receiveMessage().Contains("length") == true)
                    scktEngine.sendMessage(uploadFileLength.ToString());
                if (scktEngine.receiveMessage().Contains("file") == true)
                {
                    empty val = () => { uploadProgress.Value = 0; };
                    uploadProgress.Invoke(val);
                        for (long i = 0; i <= uploadFileLength; i += BLOCK_SIZE)
                        {
                            String val1 = i.ToString() + " / " + uploadFileLength.ToString();
                            stringOne txt = (Val) => { sendStatus1.Text = Val; };
                            sendStatus1.Invoke(txt, val1);
                            int Value = (int)(Math.Round(Convert.ToDouble(i) / Convert.ToDouble(uploadFileLength) * 100));
                            integerVal prgrss = (Val) => { uploadProgress.Value = Val; };
                            uploadProgress.Invoke(prgrss, Value);
                            inputStream.Read(buffer, 0, BLOCK_SIZE);
                            scktEngine.socket.Send(buffer, 0, BLOCK_SIZE, SocketFlags.None);
                        }
                        inputStream.Close();
                    String val2 = uploadFileLength.ToString() + " / " + uploadFileLength.ToString();
                    stringOne txt2 = (Val2) => { sendStatus1.Text = Val2; };
                    sendStatus1.Invoke(txt2, val2);
                }
            }
            empty enbld = () => {
                update.Enabled = true;
                saveFileControls.Enabled = true; };
            saveFileControls.Invoke(enbld);
            workFlag = false;
            Thread.CurrentThread.Abort();
        }

        private void chooseDir_Click(object sender, EventArgs e)
        {
            if (saveDownloadFile.ShowDialog() == DialogResult.OK)
            {
                uploadFileSave.Enabled = true;
                VdownloadFilePath = saveDownloadFile.SelectedPath + "\\" + name.Text + "\\" + VdownloadFileName;
                savePath.Text = VdownloadFilePath;
            }
        }

        private void downloadFileSave_Click(object sender, EventArgs e)
        {
            workFlag = true;
            update.Enabled = false;
            uploadFileSave.Enabled = false;
            serverFileTree.Enabled = false;
            groupFileControls.Enabled = false;
            Thread dwnl = new Thread(downloadFile);
            dwnl.Start();
        }
        private void downloadFile()
        {
            try
            {
                string msg = "";
                long fileLength = -1;
                scktEngine.sendMessage("file download");
                if (scktEngine.receiveMessage().Contains("file") == true)
                {
                    scktEngine.sendMessage("name"); //send command
                    if (scktEngine.receiveMessage().Contains("OK") == true)
                        scktEngine.sendMessage(VdownloadFileName); //send name
                    using (FileStream outputStream = File.OpenWrite(VdownloadFilePath))
                    {
                        byte[] buffer = new byte[BLOCK_SIZE];
                        if (scktEngine.receiveMessage().Contains("OK") == true)
                            scktEngine.sendMessage("length");
                        msg = scktEngine.receiveMessage(); //obtain file length;

                        try
                        {
                            fileLength = Convert.ToInt64(msg);
                        }
                        catch
                        {
                            fileLength = 0;
                        }
                        empty val = () => { downloadProgress.Value = 0; };
                        downloadProgress.Invoke(val);
                        scktEngine.sendMessage("file");
                        for (long i = 0; i <= fileLength; i += BLOCK_SIZE)
                        {
                            int Value = (int)(Math.Round(Convert.ToDouble(i) / Convert.ToDouble(fileLength) * 100));
                            integerVal prgrss = (Val) => { downloadProgress.Value = Val; };
                            downloadProgress.Invoke(prgrss, Value);
                            scktEngine.socket.Receive(buffer);
                            outputStream.Write(buffer, 0, BLOCK_SIZE);
                            string status = i.ToString() + " / " + fileLength.ToString();
                            stringOne txt = (str) => { testLabel.Text = str; };
                            testLabel.Invoke(txt, status);
                        }
                        outputStream.Close();
                        string status2 = fileLength.ToString() + " / " + fileLength.ToString(); ;
                        stringOne txt2 = (str) => { testLabel.Text = str; };
                        testLabel.Invoke(txt2, status2);
                        empty enbld = () => { uploadFileSave.Enabled = true; };
                        uploadFileSave.Invoke(enbld);
                        empty enbld3 = () => { groupFileControls.Enabled = true; };
                        groupFileControls.Invoke(enbld3);
                        empty enbld4 = () => { serverFileTree.Enabled = true;
                        update.Enabled = true;
                        };
                        serverFileTree.Invoke(enbld4);
                    }
                }
            }
            catch (FormatException ex)
            {
                string error = ex.Source;
                stringOne txt3 = (err) => { testLabel.Text = err; };
                testLabel.Invoke(txt3, error);
                Thread.CurrentThread.Abort();
            }
            workFlag = false;
            Thread.CurrentThread.Abort();
        }

        private void update_Click(object sender, EventArgs e)
        {
            Console.WriteLine(memoryStatic.login);
            workFlag = true;
            update.Enabled = false;
            Thread lst = new Thread(updateFileList);
            lst.Start();
        }

        private void serverFileTree_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void ClientWindow_Load(object sender, EventArgs e)
        {
            if (memoryStatic.privileges == 5)
                Administrator.Parent = null;
            if (memoryStatic.privileges == 0)
            {
                privileges.ForeColor = Color.FromArgb(255, 128, 50, 248);
                privileges.Text = "Главный администратор";
            }
            if (memoryStatic.privileges == 5)
            {
                privileges.ForeColor = Color.FromArgb(255, 74, 211, 25);
                privileges.Text = "Пользователь";
            }
            scktEngine = memoryStatic.sock;
            name.Text = memoryStatic.login;
            if (Directory.Exists(name.Text) == false)
                Directory.CreateDirectory(name.Text);
        }

        private void ClientWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (workFlag == true)
                {
                    e.Cancel = false;
                    MessageBox.Show("Программа выполняет обмен данными и не может быть закрыта");
                }
            }
        }

        private void searchText_Enter(object sender, EventArgs e)
        {
            searchText.Text = "";
        }

        private void searchText_Leave(object sender, EventArgs e)
        {
            if (searchText.Text == "")
               searchText.Text = "Text to search...";
        }

        private void searchText_TextChanged(object sender, EventArgs e)
        {
            if (searchText.Text != "Text to search...")
            {
                serverFileTree.Rows.Clear();
                int rowsCount = 0;
     
                for (int i = 0; i < filesC.listOfFiles.fileNames.Length; i++)
                {
                    if (filesC.listOfFiles.fileNames[i].Contains(searchText.Text) == true)
                    {
                        serverFileTree.Rows.Add(1);
                        serverFileTree.Rows[rowsCount].Cells[0].Value = filesC.listOfFiles.fileNames[i];
                        serverFileTree.Rows[rowsCount].Cells[1].Value = filesC.listOfFiles.downloadNumbers[i];
                        rowsCount++;
                    }
                }
                serverFileTree.Sort(serverFileTree.Columns[1], ListSortDirection.Descending);
            }
        }

        private void serverFileTree_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (serverFileTree.Rows.Count > 0)
            {
                if (serverFileTree.SelectedCells.Count > 0)
                {
                    if (serverFileTree.SelectedCells[0].ColumnIndex == 0)
                    {
                        saveFileControls.Enabled = true;
                        VdownloadFileName = serverFileTree.SelectedCells[0].Value.ToString();
                        if (saveDownloadFile.SelectedPath != "")
                            VdownloadFilePath = saveDownloadFile.SelectedPath + "\\" + name.Text + "\\" + VdownloadFileName;
                        else
                            VdownloadFilePath = name.Text + "\\" + VdownloadFileName;
                        savePath.Text = VdownloadFilePath;
                        uploadFileSave.Enabled = true;
                        string previewPath = name.Text + "\\TEMP\\" + VdownloadFileName + ".smll";
                        Bitmap preview;
                        if (File.Exists(previewPath))
                        {
                            FileStream fs = File.OpenRead(previewPath);
                            previewPicture.Image = Image.FromStream(fs);
                            fs.Close();
                        }
                    }
                }
            }
        }

        private void filesDel_Click(object sender, EventArgs e)
        {
            scktEngine.sendMessage("delete");
            string message = "";
            if (scktEngine.receiveMessage().Contains("ready to delete files"))
            {
                scktEngine.sendMessage(filesDelTxt.Text);
                while (message.Contains("failed") == false
                    && message.Contains("success") == false)
                {
                    message = scktEngine.receiveMessage();
                    switch (message)
                    {
                        case "ready to delete files":
                            scktEngine.sendMessage(filesDelTxt.Text);
                            break;
                        case "failed":
                            scktEngine.sendMessage("why");
                            MessageBox.Show(scktEngine.receiveMessage());
                            message = "failed";
                            break;
                        case "success":
                            MessageBox.Show("Successfully");
                            break;
                        default:
                            MessageBox.Show("Failed: " + message);
                            message = "failed";
                        break;
                    }
                }
            }
        }
    }
}
