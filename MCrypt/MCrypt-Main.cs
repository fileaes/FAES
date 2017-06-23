using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MCrypt
{
    public partial class MCrypt_Main : Form
    {
        Core core = new Core();
        MCrypt_Update update = new MCrypt_Update();

        public MCrypt_Main()
        {
            InitializeComponent();
            versionLabel.Text = core.getVersionInfo();
        }

        private void MCrypt_Main_Load(object sender, EventArgs e)
        {
            update.checkForUpdate();
        }

        private void encryptFileDrop_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
            {
                String[] strGetFormats = e.Data.GetFormats();
                e.Effect = DragDropEffects.None;
            }
        }

        private void decryptFileDrop_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
            {
                String[] strGetFormats = e.Data.GetFormats();
                e.Effect = DragDropEffects.None;
            }
        }

        private void encryptFileDrop_DragDrop(object sender, DragEventArgs e)
        {
            string[] FileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            if (FileList.Length > 1) MessageBox.Show("You may only encrypt a single file or folder at a time.", "Multiple Files Unsupported");
            else
            {
                if (Directory.Exists(FileList[0])) Program.doEncryptFolder = true;
                else if (File.Exists(FileList[0])) Program.doEncryptFile = true;
                using (MCrypt_Encrypt encrypt = new MCrypt_Encrypt(FileList[0].Replace(@"\\", @"\")))
                {
                    encrypt.StartPosition = FormStartPosition.CenterParent;
                    encrypt.ShowDialog();
                    encrypt.Focus();
                }
            }
        }

        private void encryptFileDrop_Click(object sender, EventArgs e)
        {
            var fileToEncrypt = "";
            if (openFileToEncrypt.ShowDialog() == DialogResult.OK)
            {
                fileToEncrypt = openFileToEncrypt.FileName;
                if (Directory.Exists(fileToEncrypt)) Program.doEncryptFolder = true;
                else if (File.Exists(fileToEncrypt)) Program.doEncryptFile = true;
                using (MCrypt_Encrypt encrypt = new MCrypt_Encrypt(fileToEncrypt.Replace(@"\\", @"\")))
                {
                    encrypt.StartPosition = FormStartPosition.CenterParent;
                    encrypt.ShowDialog();
                    encrypt.Focus();
                }
            }
        }

        private void decryptFileDrop_DragDrop(object sender, DragEventArgs e)
        {
            string[] FileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            if (FileList.Length > 1) MessageBox.Show("You may only decrypt a single file at a time.", "Multiple Files Unsupported");
            else if (!Program.isValidFiletype(FileList[0])) MessageBox.Show("Please select a valid filetype.", "Invalid Filetype");
            else
            {
                Program.doDecrypt = true;
                using (MCrypt_Decrypt decrypt = new MCrypt_Decrypt(FileList[0].Replace(@"\\", @"\")))
                {
                    decrypt.StartPosition = FormStartPosition.CenterParent;
                    decrypt.ShowDialog();
                    decrypt.Focus();
                }
            }
        }

        private void decryptFileDrop_Click(object sender, EventArgs e)
        {
            var fileToDecrypt = "";
            if (openFileToDecrypt.ShowDialog() == DialogResult.OK)
            {
                if (!Program.isValidFiletype(openFileToDecrypt.FileName)) MessageBox.Show("Please select a valid filetype.", "Invalid Filetype");
                else
                {
                    fileToDecrypt = openFileToDecrypt.FileName;
                    Program.doDecrypt = true;
                    using (MCrypt_Decrypt decrypt = new MCrypt_Decrypt(fileToDecrypt.Replace(@"\\", @"\")))
                    {
                        decrypt.StartPosition = FormStartPosition.CenterParent;
                        decrypt.ShowDialog();
                        decrypt.Focus();
                    }
                }
            }
        }

        private void versionLabel_Click(object sender, EventArgs e)
        {
            update.Show();
        }
    }
}
