using FAES;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace FileAES
{
    public partial class FileAES_Encrypt : Form
    {
        Core core = new Core();
        FileAES_Update update = new FileAES_Update();

        private bool _inProgress = false;
        private bool _encryptSuccessful;
        private string _fileToEncrypt, _autoPassword;

        public FileAES_Encrypt(string file, string password = null)
        {
            if (!String.IsNullOrEmpty(file)) _fileToEncrypt = file;
            else throw new ArgumentException("Parameter cannot be null", "file");
            InitializeComponent();
            versionLabel.Text = core.getVersionInfo();
            populateCompressionModes();
            if (Program.doEncryptFile) fileName.Text = Path.GetFileName(_fileToEncrypt);
            else if (Program.doEncryptFolder) fileName.Text = Path.GetFileName(_fileToEncrypt.TrimEnd(Path.DirectorySeparatorChar));
            this.Focus();
            this.ActiveControl = passwordInput;
            _autoPassword = password;
        }

        private void FileAES_Encrypt_Load(object sender, EventArgs e)
        {
            update.checkForUpdate();

            if (_autoPassword != null && _autoPassword.Length > 3)
            {
                passwordInput.Text = _autoPassword;
                passwordInputConf.Text = _autoPassword;
                runtime_Tick(null, null);
                encryptButton_Click(null, null);
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

        private void encryptButton_Click(object sender, EventArgs e)
        {
            if (encryptButton.Enabled)
            {
                if (Core.isEncryptFileValid(_fileToEncrypt) && !_inProgress && passwordInputConf.Text == passwordInput.Text) backgroundEncrypt.RunWorkerAsync();
                else if (passwordInputConf.Text != passwordInput.Text) setNoteLabel("Passwords do not match!", 2);
                else if (_inProgress) setNoteLabel("Encryption already in progress.", 1);
                else setNoteLabel("Encryption Failed. Try again later.", 1);

                runtime_Tick(sender, e);
            }
        }

        private void doEncrypt()
        {
            try
            {
                setNoteLabel("Encrypting... Please wait.", 0);

                _inProgress = true;
                _encryptSuccessful = false;

                while (!backgroundEncrypt.CancellationPending)
                {
                    FAES.FileAES_Encrypt encrypt = new FAES.FileAES_Encrypt(new FAES_File(_fileToEncrypt), passwordInput.Text, hintInput.Text);

                    encrypt.SetCompressionMode(FAES.Packaging.CompressionUtils.GetAllOptimiseModes()[compressMode.SelectedIndex]);

                    _encryptSuccessful = encrypt.encryptFile();

                    backgroundEncrypt.CancelAsync();
                }
            }
            catch (Exception e)
            {
                setNoteLabel(FileAES_Utilities.FAES_ExceptionHandling(e), 3);
            }
        }

        private void populateCompressionModes()
        {
            List<string> optimiseModes = FAES.Packaging.CompressionUtils.GetAllOptimiseModesAsStrings();

            compressMode.Items.Clear();
            
            foreach (string mode in optimiseModes)
            {
                compressMode.Items.Add(mode.Replace("_", " "));
            }

            compressMode.SelectedIndex = 0;
        }

        private void backgroundEncrypt_DoWork(object sender, DoWorkEventArgs e)
        {
            doEncrypt();
        }

        private void backgroundEncrypt_Complete(object sender, RunWorkerCompletedEventArgs e)
        {
            _inProgress = false;

            if (_encryptSuccessful)
            {
                setNoteLabel("Done!", 0);
                Application.Exit();
            }
        }

        private void runtime_Tick(object sender, EventArgs e)
        {
            if (_inProgress)
            {
                encryptButton.Enabled = false;
                passwordInput.Enabled = false;
                passwordInputConf.Enabled = false;
                hintInput.Enabled = false;
                compressMode.Enabled = false;
            }
            else if (Core.isEncryptFileValid(_fileToEncrypt) && passwordInput.Text.Length > 3 && passwordInputConf.Text.Length > 3 && !_inProgress)
            {
                encryptButton.Enabled = true;
                passwordInput.Enabled = true;
                passwordInputConf.Enabled = true;
                hintInput.Enabled = true;
                compressMode.Enabled = true;

            }
            else
            {
                encryptButton.Enabled = false;
                passwordInput.Enabled = true;
                passwordInputConf.Enabled = true;
                hintInput.Enabled = true;
                compressMode.Enabled = true;
            }
        }

        private void passwordBox_Focus(object sender, EventArgs e)
        {
            this.AcceptButton = encryptButton;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.WindowsShutDown) return;
            if (e.CloseReason == CloseReason.ApplicationExitCall) return;

            if (_inProgress)
            {
                if (MessageBox.Show(this, "Are you sure you want to stop encrypting?", "Closing", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    backgroundEncrypt.CancelAsync();
                    try
                    {
                        if (Program.doEncryptFile)
                        {
                            if (File.Exists(Path.Combine(Path.GetDirectoryName(_fileToEncrypt), fileName.Text) + ".faeszip")) File.Delete(Path.Combine(Path.GetDirectoryName(_fileToEncrypt), fileName.Text) + FileAES_Utilities.ExtentionUFAES);
                            if (File.Exists(Path.Combine(Path.GetDirectoryName(_fileToEncrypt), fileName.Text) + ".faeszip")) File.Delete(Path.Combine(Path.GetDirectoryName(_fileToEncrypt), fileName.Text) + FileAES_Utilities.ExtentionUFAES);
                        }
                        else if (Program.doEncryptFolder)
                        {
                            if (File.Exists(Path.Combine(Directory.GetParent(_fileToEncrypt).FullName, fileName.Text) + ".faeszip")) File.Delete(Path.Combine(Directory.GetParent(_fileToEncrypt).FullName, fileName.Text) + FileAES_Utilities.ExtentionUFAES);
                            if (File.Exists(Path.Combine(Directory.GetParent(_fileToEncrypt).FullName, fileName.Text) + ".faeszip")) File.Delete(Path.Combine(Directory.GetParent(_fileToEncrypt).FullName, fileName.Text) + FileAES_Utilities.ExtentionUFAES);

                            if (!core.IsDirectoryEmpty(Path.Combine(Program.tempPathInstance))) Directory.Delete(Path.Combine(Program.tempPathInstance), true);
                        }

                        FileAES_Utilities.PurgeInstancedTempFolders();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("This action is currently unsupported!", "Error");
                        e.Cancel = true;
                    }
                }
                else e.Cancel = true;
                update.Dispose();
            }
            else
                Directory.Delete(Program.tempPathInstance, true);
        }

        private void versionLabel_Click(object sender, EventArgs e)
        {
            update.Show();
        }

        private void hintInput_TextChanged(object sender, EventArgs e)
        {
            hintInput.Text = hintInput.Text.Replace(Environment.NewLine, "");
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
