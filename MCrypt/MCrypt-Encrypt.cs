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

namespace MCrypt
{
    public partial class MCrypt_Encrypt : Form
    {
        Core core = new Core();
        SecureAES aes = new SecureAES();
        MCrypt_Update update = new MCrypt_Update();
        private bool inProgress = false;

        public MCrypt_Encrypt()
        {
            InitializeComponent();
            versionLabel.Text = core.getVersionInfo();
            if (Program.doEncryptFile) fileName.Text = Path.GetFileName(Program.fileName);
            else if (Program.doEncryptFolder) fileName.Text = Path.GetFileName(Program.fileName.TrimEnd(Path.DirectorySeparatorChar));
        }

        private void MCrypt_Encrypt_Load(object sender, EventArgs e)
        {
            update.checkForUpdate();
        }

        private void setNoteLabel(string note, int severity)
        {
            if (severity == 1) noteLabel.Text = "Note: " + note;
            else if (severity == 3) noteLabel.Text = "Note: " + note;
            else noteLabel.Text = "Note: " + note;
        }

        private void encryptButton_Click(object sender, EventArgs e)
        {
            if (encryptButton.Enabled)
            {
                if (core.isEncryptFileValid(Program.fileName) && !inProgress && passwordInputConf.Text == passwordInput.Text) backgroundEncrypt.RunWorkerAsync();
                else if (passwordInputConf.Text != passwordInput.Text) setNoteLabel("Passwords do not match!", 2);
                else if (inProgress) setNoteLabel("Encryption already in progress.", 1);
                else setNoteLabel("Encryption Failed. Try again later.", 1);
            }
        }

        private void doEncrypt()
        {
            while (!backgroundEncrypt.CancellationPending)
            {
                inProgress = true;
                string tempFolderName = "";
                setNoteLabel("Encrypting... Please wait.", 0);
                if (Program.doEncryptFile)
                {
                    if (!Directory.Exists(Path.Combine(Path.GetTempPath(), "MCrypt"))) Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), "MCrypt"));

                    using (ZipArchive zip = ZipFile.Open(Path.Combine(Path.GetTempPath(), "MCrypt", fileName.Text) + ".zip", ZipArchiveMode.Create)) zip.CreateEntryFromFile(Program.fileName, fileName.Text);
                }
                else
                {
                    tempFolderName = core.tempFolderNameGen(fileName.Text.Substring(0, fileName.Text.Length - Path.GetExtension(fileName.Text).Length));
                    if (Directory.Exists(Path.Combine(Path.GetTempPath(), "MCrypt", tempFolderName))) Directory.Delete(Path.Combine(Path.GetTempPath(), "MCrypt", tempFolderName), true);
                    core.DirectoryCopy(Program.fileName, Path.Combine(Path.GetTempPath(), "MCrypt", tempFolderName, fileName.Text), true);
                    ZipFile.CreateFromDirectory(Path.Combine(Path.GetTempPath(), "MCrypt", tempFolderName), Path.Combine(Path.GetTempPath(), "MCrypt", fileName.Text) + ".zip");
                }

                aes.AES_Encrypt(Path.Combine(Path.GetTempPath() + "MCrypt", fileName.Text) + ".zip", passwordInput.Text, Path.Combine(Path.GetTempPath(), fileName.Text.Substring(0, fileName.Text.Length - Path.GetExtension(fileName.Text).Length) + ".mcrypt"));
                setNoteLabel("Encrypting...", 0);
                File.Delete(Path.Combine(Path.GetTempPath(), "MCrypt", fileName.Text) + ".zip");
                if (Directory.Exists(Path.Combine(Path.GetTempPath(), "MCrypt", tempFolderName))) Directory.Delete(Path.Combine(Path.GetTempPath(), "MCrypt", tempFolderName), true);
                if (Program.doEncryptFolder && Directory.Exists(Path.Combine(Path.GetTempPath(), "MCrypt", tempFolderName))) Directory.Delete(Path.Combine(Path.GetTempPath(), "MCrypt", tempFolderName), true);
                if (Program.doEncryptFile) File.Delete(Program.fileName);
                else Directory.Delete(Program.fileName, true);
                File.Move(Path.Combine(Path.GetTempPath(), fileName.Text.Substring(0, fileName.Text.Length - Path.GetExtension(fileName.Text).Length) + ".mcrypt"), Path.Combine(Directory.GetParent(Program.fileName).FullName, fileName.Text.Substring(0, fileName.Text.Length - Path.GetExtension(fileName.Text).Length) + ".mcrypt"));
                File.SetAttributes(fileName.Text.Substring(0, fileName.Text.Length - Path.GetExtension(fileName.Text).Length) + ".mcrypt", FileAttributes.Encrypted);
            }
            backgroundEncrypt.CancelAsync();
        }

        private void backgroundEncrypt_DoWork(object sender, DoWorkEventArgs e)
        {
            doEncrypt();
        }

        private void backgroundEncrypt_Complete(object sender, RunWorkerCompletedEventArgs e)
        {
            setNoteLabel("Done!", 0);
            inProgress = false;
            Application.Exit();
        }

        private void runtime_Tick(object sender, EventArgs e)
        {
            if (core.isEncryptFileValid(Program.fileName) && passwordInput.Text.Length > 3 && passwordInputConf.Text.Length > 3 && !inProgress) encryptButton.Enabled = true;
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

            if (inProgress)
            {
                if (MessageBox.Show(this, "Are you sure you want to stop encrypting?", "Closing", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    backgroundEncrypt.CancelAsync();
                    try
                    {
                        if (Program.doEncryptFile)
                        {
                            if (File.Exists(Path.Combine(Path.GetDirectoryName(Program.fileName), fileName.Text) + ".zip")) File.Delete(Path.Combine(Path.GetDirectoryName(Program.fileName), fileName.Text) + ".zip");
                            if (File.Exists(Path.Combine(Path.GetDirectoryName(Program.fileName), fileName.Text) + ".zip")) File.Delete(Path.Combine(Path.GetDirectoryName(Program.fileName), fileName.Text) + ".zip");

                        }
                        else if (Program.doEncryptFolder)
                        {
                            if (File.Exists(Path.Combine(Directory.GetParent(Program.fileName).FullName, fileName.Text) + ".zip")) File.Delete(Path.Combine(Directory.GetParent(Program.fileName).FullName, fileName.Text) + ".zip");
                            if (File.Exists(Path.Combine(Directory.GetParent(Program.fileName).FullName, fileName.Text) + ".zip")) File.Delete(Path.Combine(Directory.GetParent(Program.fileName).FullName, fileName.Text) + ".zip");
                            if (core.IsDirectoryEmpty(Path.Combine(Path.GetTempPath(), "MCrypt"))) Directory.Delete(Path.Combine(Path.GetTempPath(), "MCrypt"), true);
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
        }

        private void versionLabel_Click(object sender, EventArgs e)
        {
            update.Show();
        }
    }
}
