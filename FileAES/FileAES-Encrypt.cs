using System;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Windows.Forms;

namespace FileAES
{
    public partial class FileAES_Encrypt : Form
    {
        Core core = new Core();
        SecureAES aes = new SecureAES();
        FileAES_Update update = new FileAES_Update();
        private bool _inProgress = false;
        private string _fileToEncrypt;
        private string _autoPassword;

        public FileAES_Encrypt(string file, string password = null)
        {
            if (!String.IsNullOrEmpty(file)) _fileToEncrypt = file;
            else throw new System.ArgumentException("Parameter cannot be null", "file");
            InitializeComponent();
            versionLabel.Text = core.getVersionInfo();
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
            noteLabel.Invoke(new MethodInvoker(delegate { this.noteLabel.Text = "Note: " + note; }));
        }

        private void encryptButton_Click(object sender, EventArgs e)
        {
            if (encryptButton.Enabled)
            {
                if (Core.isEncryptFileValid(_fileToEncrypt) && !_inProgress && passwordInputConf.Text == passwordInput.Text) backgroundEncrypt.RunWorkerAsync();
                else if (passwordInputConf.Text != passwordInput.Text) setNoteLabel("Passwords do not match!", 2);
                else if (_inProgress) setNoteLabel("Encryption already in progress.", 1);
                else setNoteLabel("Encryption Failed. Try again later.", 1);
            }
        }

        private void doEncrypt()
        {
            string rawName = fileName.Text;
            string fileToDelete = Path.Combine(Program.tempPathInstance, rawName + ".faeszip");
            try
            {
                while (!backgroundEncrypt.CancellationPending)
                {
                    _inProgress = true;
                    string tempFolderName = "";
                    setNoteLabel("Encrypting... Please wait.", 0);
                    if (Program.doEncryptFile)
                    {
                        if (!Directory.Exists(Path.Combine(Program.tempPathInstance))) Directory.CreateDirectory(Path.Combine(Program.tempPathInstance));

                        using (ZipArchive zip = ZipFile.Open(Path.Combine(Program.tempPathInstance, rawName) + ".faeszip", ZipArchiveMode.Create))
                        {
                            zip.CreateEntryFromFile(_fileToEncrypt, rawName);
                            zip.Dispose();
                        }
                    }
                    else
                    {
                        tempFolderName = core.tempFolderNameGen(rawName.Substring(0, rawName.Length - Path.GetExtension(rawName).Length));
                        if (Directory.Exists(Path.Combine(Program.tempPathInstance, tempFolderName))) Directory.Delete(Path.Combine(Program.tempPathInstance, tempFolderName), true);
                        core.DirectoryCopy(_fileToEncrypt, Path.Combine(Program.tempPathInstance, tempFolderName, rawName), true);
                        ZipFile.CreateFromDirectory(Path.Combine(Program.tempPathInstance, tempFolderName), Path.Combine(Program.tempPathInstance, rawName) + ".faeszip");
                    }

                    aes.AES_Encrypt(Path.Combine(Program.tempPathInstance, rawName) + ".faeszip", passwordInput.Text, Path.Combine(Program.tempPathInstance, rawName.Substring(0, rawName.Length - Path.GetExtension(rawName).Length) + ".faes"));
                    setNoteLabel("Encrypting...", 0);
                    if (File.Exists(fileToDelete))
                        File.Delete(fileToDelete);
                    if (Program.doEncryptFile) File.Move(Path.Combine(Program.tempPathInstance, rawName.Substring(0, rawName.Length - Path.GetExtension(rawName).Length) + ".faes"), Path.Combine(Directory.GetParent(_fileToEncrypt).FullName, rawName.Substring(0, rawName.Length - Path.GetExtension(rawName).Length) + ".faes"));
                    if (Directory.Exists(Path.Combine(Program.tempPathInstance, tempFolderName))) Directory.Delete(Path.Combine(Program.tempPathInstance, tempFolderName), true);
                    if (Program.doEncryptFolder && Directory.Exists(Path.Combine(Program.tempPathInstance, tempFolderName))) Directory.Delete(Path.Combine(Program.tempPathInstance, tempFolderName), true);
                    if (Program.doEncryptFile) File.Delete(_fileToEncrypt);
                    else Directory.Delete(_fileToEncrypt, true);
                    if (Program.doEncryptFolder) File.Move(Path.Combine(Program.tempPathInstance, rawName.Substring(0, rawName.Length - Path.GetExtension(rawName).Length) + ".faes"), Path.Combine(Directory.GetParent(_fileToEncrypt).FullName, rawName.Substring(0, rawName.Length - Path.GetExtension(rawName).Length) + ".faes"));
                    File.SetAttributes(rawName.Substring(0, rawName.Length - Path.GetExtension(rawName).Length) + ".faes", FileAttributes.Encrypted);
                }
                backgroundEncrypt.CancelAsync();
            }
            finally
            {
                if (File.Exists(fileToDelete))
                    File.Delete(fileToDelete);
            }
        }

        private void backgroundEncrypt_DoWork(object sender, DoWorkEventArgs e)
        {
            doEncrypt();
        }

        private void backgroundEncrypt_Complete(object sender, RunWorkerCompletedEventArgs e)
        {
            setNoteLabel("Done!", 0);
            _inProgress = false;
            Application.Exit();
        }

        private void runtime_Tick(object sender, EventArgs e)
        {
            if (Core.isEncryptFileValid(_fileToEncrypt) && passwordInput.Text.Length > 3 && passwordInputConf.Text.Length > 3 && !_inProgress) encryptButton.Enabled = true;
            else encryptButton.Enabled = false;
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
                            if (File.Exists(Path.Combine(Path.GetDirectoryName(_fileToEncrypt), fileName.Text) + ".faeszip")) File.Delete(Path.Combine(Path.GetDirectoryName(_fileToEncrypt), fileName.Text) + ".faeszip");
                            if (File.Exists(Path.Combine(Path.GetDirectoryName(_fileToEncrypt), fileName.Text) + ".faeszip")) File.Delete(Path.Combine(Path.GetDirectoryName(_fileToEncrypt), fileName.Text) + ".faeszip");

                        }
                        else if (Program.doEncryptFolder)
                        {
                            if (File.Exists(Path.Combine(Directory.GetParent(_fileToEncrypt).FullName, fileName.Text) + ".faeszip")) File.Delete(Path.Combine(Directory.GetParent(_fileToEncrypt).FullName, fileName.Text) + ".faeszip");
                            if (File.Exists(Path.Combine(Directory.GetParent(_fileToEncrypt).FullName, fileName.Text) + ".faeszip")) File.Delete(Path.Combine(Directory.GetParent(_fileToEncrypt).FullName, fileName.Text) + ".faeszip");
                            if (!core.IsDirectoryEmpty(Path.Combine(Program.tempPathInstance))) Directory.Delete(Path.Combine(Program.tempPathInstance), true);
                        }
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
            {
                Directory.Delete(Program.tempPathInstance, true);
            }
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
