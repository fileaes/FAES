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
    public partial class MCrypt_Decrypt : Form
    {
        Core core = new Core();
        SecureAES aes = new SecureAES();
        private bool inProgress = false;

        public MCrypt_Decrypt()
        {
            InitializeComponent();
            versionLabel.Text = core.getVersionInfo();
            if (Program.doDecrypt) fileName.Text = Path.GetFileName(Program.fileName);
        }

        private void decryptButton_Click(object sender, EventArgs e)
        {
            if (decryptButton.Enabled)
            {
                if (core.isDecryptFileValid(Program.fileName) && !inProgress) backgroundDecrypt.RunWorkerAsync();
                else if (inProgress) setNoteLabel("Decryption already in progress.", 1);
                else setNoteLabel("Decryption Failed. Try again later.", 1);
            }
        }

        private void setNoteLabel(string note, int severity)
        {
            if (severity == 1) noteLabel.Text = "Note: " + note;
            else if (severity == 3) noteLabel.Text = "Note: " + note;
            else noteLabel.Text = "Note: " + note;
        }

        private void doDecrypt()
        {
            while (!backgroundDecrypt.CancellationPending)
            {
                inProgress = true;
                setNoteLabel("Decrypting... Please wait.", 0);
                aes.AES_Decrypt(Program.fileName, passwordInput.Text, fileName.Text.Substring(0, fileName.Text.Length - Path.GetExtension(fileName.Text).Length) + ".zip");
                File.SetAttributes(fileName.Text.Substring(0, fileName.Text.Length - Path.GetExtension(fileName.Text).Length) + ".zip", FileAttributes.Hidden);                
                if (aes.getLastError() != "decryptIncorrectPassword")
                {
                    ZipFile.ExtractToDirectory(Path.Combine(Directory.GetParent(Program.fileName).FullName, fileName.Text.Substring(0, fileName.Text.Length - Path.GetExtension(fileName.Text).Length) + ".zip"), Directory.GetParent(Program.fileName).FullName);
                    File.SetAttributes(Directory.GetParent(Program.fileName).FullName, FileAttributes.Hidden);
                    File.Delete(Path.Combine(Directory.GetParent(Program.fileName).FullName, fileName.Text.Substring(0, fileName.Text.Length - Path.GetExtension(fileName.Text).Length) + ".zip"));
                    File.Delete(Program.fileName);
                    File.SetAttributes(Directory.GetParent(Program.fileName).FullName, FileAttributes.Normal);
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
            inProgress = false;
            if (aes.getLastError() == "decryptSuccess") setNoteLabel("Done!", 0);
            if (aes.getLastError() == "decryptIncorrectPassword") setNoteLabel("Password Incorrect!", 3);
            else Application.Exit();
        }

        private void runtime_Tick(object sender, EventArgs e)
        {
            if (core.isDecryptFileValid(Program.fileName) && passwordInput.Text.Length > 3 && !inProgress) decryptButton.Enabled = true;
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

            if (inProgress)
            {
                if (MessageBox.Show(this, "Are you sure you want to stop decrypting?", "Closing", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    backgroundDecrypt.CancelAsync();
                    try
                    {
                        if (File.Exists(Path.Combine(Directory.GetParent(Program.fileName).FullName, fileName.Text.Substring(0, fileName.Text.Length - Path.GetExtension(fileName.Text).Length) + ".zip")))
                            File.Delete(Path.Combine(Directory.GetParent(Program.fileName).FullName, fileName.Text.Substring(0, fileName.Text.Length - Path.GetExtension(fileName.Text).Length) + ".zip"));
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("This action is currently unsupported!", "Error");
                        e.Cancel = true;
                    }
                }
                else e.Cancel = true;
            }
        }
    }
}
