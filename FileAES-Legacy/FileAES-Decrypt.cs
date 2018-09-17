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
using System.IO.Compression;
using System.Threading;

namespace FileAES
{
    public partial class FileAES_Decrypt : Form
    {
        Core core = new Core();
        SecureAES aes = new SecureAES();
        FileAES_Update update = new FileAES_Update();
        private bool _inProgress = false;
        private string _fileToDecrypt;
        private string _autoPassword;

        public FileAES_Decrypt(string file, string password = null)
        {
            if (!String.IsNullOrEmpty(file)) _fileToDecrypt = file;
            else throw new System.ArgumentException("Parameter cannot be null", "file");
            InitializeComponent();
            versionLabel.Text = core.getVersionInfo();
            if (Program.doDecrypt) fileName.Text = Path.GetFileName(_fileToDecrypt);
            this.Focus();
            this.ActiveControl = passwordInput;
            _autoPassword = password;
        }

        private void FileAES_Decrypt_Load(object sender, EventArgs e)
        {
            update.checkForUpdate();

            if (_autoPassword != null && _autoPassword.Length > 3)
            {
                passwordInput.Text = _autoPassword;
                runtime_Tick(null, null);
                decryptButton_Click(null, null);
            }
        }

        private void decryptButton_Click(object sender, EventArgs e)
        {
            if (decryptButton.Enabled)
            {
                if (Core.isDecryptFileValid(_fileToDecrypt) && !_inProgress) backgroundDecrypt.RunWorkerAsync();
                else if (_inProgress) setNoteLabel("Decryption already in progress.", 1);
                else setNoteLabel("Decryption Failed. Try again later.", 1);
            }
        }

        private void setNoteLabel(string note, int severity)
        {
            noteLabel.Invoke(new MethodInvoker(delegate { this.noteLabel.Text = "Note: " + note; }));
        }

        private void doDecrypt()
        {
            while (!backgroundDecrypt.CancellationPending)
            {
                _inProgress = true;
                setNoteLabel("Decrypting... Please wait.", 0);

                if (!core.isFileLegacy(_fileToDecrypt))
                    aes.AES_Decrypt(_fileToDecrypt, passwordInput.Text, _fileToDecrypt.Replace(".faes", ".faeszip"));
                else
                    aes.AES_Decrypt(_fileToDecrypt, passwordInput.Text, _fileToDecrypt.Replace(".mcrypt", ".faeszip"));

                Thread.Sleep(1000);
                File.SetAttributes(Path.Combine(Directory.GetParent(_fileToDecrypt).FullName, fileName.Text.Substring(0, fileName.Text.Length - Path.GetExtension(fileName.Text).Length) + ".faeszip"), FileAttributes.Hidden);                
                if (aes.getLastError() != "decryptIncorrectPassword")
                {
                    ZipFile.ExtractToDirectory(Path.Combine(Directory.GetParent(_fileToDecrypt).FullName, fileName.Text.Substring(0, fileName.Text.Length - Path.GetExtension(fileName.Text).Length) + ".faeszip"), Directory.GetParent(_fileToDecrypt).FullName);
                    File.SetAttributes(Directory.GetParent(_fileToDecrypt).FullName, FileAttributes.Hidden);
                    File.Delete(Path.Combine(Directory.GetParent(_fileToDecrypt).FullName, fileName.Text.Substring(0, fileName.Text.Length - Path.GetExtension(fileName.Text).Length) + ".faeszip"));
                    File.Delete(_fileToDecrypt);
                    File.SetAttributes(Directory.GetParent(_fileToDecrypt).FullName, FileAttributes.Normal);
                }
            }
            backgroundDecrypt.CancelAsync();
        }

        private void backgroundDecrypt_DoWork(object sender, DoWorkEventArgs e)
        {
            doDecrypt();
        }

        private void backgroundDecrypt_Complete(object sender, RunWorkerCompletedEventArgs e)
        {
            _inProgress = false;
            if (aes.getLastError() == "decryptSuccess") setNoteLabel("Done!", 0);
            if (aes.getLastError() == "decryptIncorrectPassword") setNoteLabel("Password Incorrect!", 3);
            else Application.Exit();
        }

        private void runtime_Tick(object sender, EventArgs e)
        {
            if (Core.isDecryptFileValid(_fileToDecrypt) && passwordInput.Text.Length > 3 && !_inProgress) decryptButton.Enabled = true;
            else decryptButton.Enabled = false;
        }

        private void passwordBox_Focus(object sender, EventArgs e)
        {
            this.AcceptButton = decryptButton;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.WindowsShutDown) return;
            if (e.CloseReason == CloseReason.ApplicationExitCall) return;

            if (_inProgress)
            {
                if (MessageBox.Show(this, "Are you sure you want to stop decrypting?", "Closing", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    backgroundDecrypt.CancelAsync();
                    try
                    {
                        if (File.Exists(Path.Combine(Directory.GetParent(_fileToDecrypt).FullName, fileName.Text.Substring(0, fileName.Text.Length - Path.GetExtension(fileName.Text).Length) + ".faeszip")))
                            File.Delete(Path.Combine(Directory.GetParent(_fileToDecrypt).FullName, fileName.Text.Substring(0, fileName.Text.Length - Path.GetExtension(fileName.Text).Length) + ".faeszip"));
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("This action is currently unsupported!", "Error");
                        e.Cancel = true;
                    }
                }
                else e.Cancel = true;
            }
            update.Dispose();
        }

        private void versionLabel_Click(object sender, EventArgs e)
        {
            update.Show();
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (Form.ModifierKeys == Keys.None && keyData == Keys.Escape)
            {
                Application.Exit();
                return true;
            }
            return base.ProcessDialogKey(keyData);
        }
    }
}
