using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using FAES;

namespace FileAES
{
    public partial class FileAES_Decrypt : Form
    {
        Core core = new Core();
        FileAES_Update update = new FileAES_Update();

        private bool _inProgress = false;
        private bool _decryptSuccessful;
        private string _fileToDecrypt, _autoPassword;

        public FileAES_Decrypt(string file, string password = null)
        {
            if (!String.IsNullOrEmpty(file)) _fileToDecrypt = file;
            else throw new ArgumentException("Parameter cannot be null", "file");
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
            hintTextbox.Text = FileAES_Utilities.GetPasswordHint(_fileToDecrypt);

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

                runtime_Tick(sender, e);
            }
        }

        private void setNoteLabel(string note, int severity)
        {
            if (note.Contains("ERROR:"))
            {
                note = note.Replace("ERROR:", "Error:");
                noteLabel.Invoke(new MethodInvoker(delegate { this.noteLabel.Text = note; }));
            }
            else
            {
                if (severity == 1) noteLabel.Invoke(new MethodInvoker(delegate { this.noteLabel.Text = "Warning: " + note; }));
                else if (severity == 2) noteLabel.Invoke(new MethodInvoker(delegate { this.noteLabel.Text = "Important: " + note; }));
                else if (severity == 3) noteLabel.Invoke(new MethodInvoker(delegate { this.noteLabel.Text = "Error: " + note; }));
                else noteLabel.Invoke(new MethodInvoker(delegate { this.noteLabel.Text = "Note: " + note; }));
            }
        }

        private void doDecrypt()
        {
            try
            {
                setNoteLabel("Decrypting... Please wait.", 0);

                _inProgress = true;
                _decryptSuccessful = false;

                while (!backgroundDecrypt.CancellationPending)
                {
                    FAES.FileAES_Decrypt encrypt = new FAES.FileAES_Decrypt(new FAES_File(_fileToDecrypt), passwordInput.Text);

                    _decryptSuccessful = encrypt.decryptFile();

                    backgroundDecrypt.CancelAsync();
                }
            }
            catch (Exception e)
            {
                setNoteLabel(FileAES_Utilities.FAES_ExceptionHandling(e), 3);
            }
        }

        private void backgroundDecrypt_DoWork(object sender, DoWorkEventArgs e)
        {
            doDecrypt();
        }

        private void backgroundDecrypt_Complete(object sender, RunWorkerCompletedEventArgs e)
        {
            _inProgress = false;
            if (_decryptSuccessful) Application.Exit();
            else setNoteLabel("Password Incorrect!", 3);
        }

        private void runtime_Tick(object sender, EventArgs e)
        {
            if (_inProgress)
            {
                decryptButton.Enabled = false;
                passwordInput.Enabled = false;
            }
            else if (Core.isDecryptFileValid(_fileToDecrypt) && passwordInput.Text.Length > 3 && !_inProgress)
            {
                decryptButton.Enabled = true;
                passwordInput.Enabled = true;

            }
            else
            {
                decryptButton.Enabled = false;
                passwordInput.Enabled = true;
            }
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
                        if (File.Exists(Path.Combine(Directory.GetParent(_fileToDecrypt).FullName, fileName.Text.Substring(0, fileName.Text.Length - Path.GetExtension(fileName.Text).Length) + FileAES_Utilities.ExtentionUFAES)))
                            File.Delete(Path.Combine(Directory.GetParent(_fileToDecrypt).FullName, fileName.Text.Substring(0, fileName.Text.Length - Path.GetExtension(fileName.Text).Length) + FileAES_Utilities.ExtentionUFAES));


                        FileAES_Utilities.PurgeInstancedTempFolders();
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

        private void noteLabel_MouseHover(object sender, EventArgs e)
        {
            slowToolTip.SetToolTip(noteLabel, noteLabel.Text);
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (Form.ModifierKeys == Keys.None && keyData == Keys.Escape)
                Application.Exit();

            return base.ProcessDialogKey(keyData);
        }
    }
}
