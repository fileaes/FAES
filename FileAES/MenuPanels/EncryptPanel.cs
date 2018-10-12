using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FAES;

namespace FAES_GUI.MenuPanels
{
    public partial class encryptPanel : UserControl
    {
        private FAES_File _fileToEncrypt;

        private bool _inProgress = false;
        private bool _encryptSuccessful;
        private bool _closeAfterOp = false;

        public encryptPanel()
        {
            Initialise();
        }

        public encryptPanel(FAES_File faesFile)
        {
            Initialise();

            if (!setFileToEncrypt(faesFile))
                throw new Exception("Input file cannot be encrypted!");
        }

        public void setCloseAfterOperationSuccessful(bool close)
        {
            _closeAfterOp = close;
        }

        private void Initialise()
        {
            InitializeComponent();

            ResetFile();
            statusInformation.Text = "";

            this.Focus();
        }

        public void ResetFile()
        {
            Locked(true);
            _fileToEncrypt = null;
            passTextbox.Text = "";
            passConfTextbox.Text = "";
            passHintTextbox.Text = "";
            fileInfoLabel.Text = "No File Selected!";
        }

        public void LockFileSelect(bool lockFile)
        {
            selectEncryptButton.Enabled = !lockFile;
        }

        public bool setFileToEncrypt(FAES_File faesFile)
        {
            if (faesFile.isFileEncryptable())
            {
                _fileToEncrypt = faesFile;
                fileInfoLabel.Text = _fileToEncrypt.getFileName();
                Locked(false);
                encryptButton.Enabled = false;
                this.ActiveControl = passTextbox;
                return true;
            }
            return false;
        }

        private void setNoteLabel(string note, int severity)
        {
            if (severity == 1) statusInformation.Invoke(new MethodInvoker(delegate { this.statusInformation.Text = "Warning: " + note; }));
            else if (severity == 2) statusInformation.Invoke(new MethodInvoker(delegate { this.statusInformation.Text = "Important: " + note; }));
            else if (severity == 3) statusInformation.Invoke(new MethodInvoker(delegate { this.statusInformation.Text = "Error: " + note; }));
            else statusInformation.Invoke(new MethodInvoker(delegate { this.statusInformation.Text = "Note: " + note; }));
        }

        private void Locked(bool lockChanges)
        {
            passTextbox.Enabled = !lockChanges;
            passConfTextbox.Enabled = !lockChanges;
            passHintTextbox.Enabled = !lockChanges;
            encryptButton.Enabled = !lockChanges;
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
                    FileAES_Encrypt encrypt = new FileAES_Encrypt(_fileToEncrypt, passTextbox.Text, passHintTextbox.Text);
                    _encryptSuccessful = encrypt.encryptFile();

                    backgroundEncrypt.CancelAsync();
                }
            }
            catch (Exception e)
            {
                setNoteLabel(FileAES_Utilities.FAES_ExceptionHandling(e), 3);
            }
        }

        private void backgroundEncrypt_DoWork(object sender, DoWorkEventArgs e)
        {
            doEncrypt();
        }

        private void backgroundEncrypt_Complete(object sender, RunWorkerCompletedEventArgs e)
        {
            _inProgress = false;
            Locked(false);

            if (_encryptSuccessful)
            {
                setNoteLabel("Encryption Complete", 0);
                if (_closeAfterOp) Application.Exit();
                else ResetFile();
            }
            else setNoteLabel("Encryption Failed. Try again later.", 1);
        }

        private void encryptButton_Click(object sender, EventArgs e)
        {
            if (_fileToEncrypt.isFileEncryptable() && !_inProgress && passConfTextbox.Text == passTextbox.Text)
            {
                backgroundEncrypt.RunWorkerAsync();
                Locked(true);
            }
            else if (passConfTextbox.Text != passTextbox.Text)
            {
                setNoteLabel("Passwords do not match!", 2);
                passConfTextbox.Focus();
            }
            else if (_inProgress) setNoteLabel("Encryption already in progress.", 1);
            else
            {
                setNoteLabel("Encryption Failed. Try again later.", 1);
                encryptButton.Focus();
            }
        }

        private void selectEncryptButton_DragDrop(object sender, DragEventArgs e)
        {
            string[] FileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            if (FileList.Length > 1) setNoteLabel("You may only encrypt a single file or folder at a time.", 2);
            else
            {
                FAES_File tFaesFile = new FAES_File(FileList[0]);
                if (!setFileToEncrypt(tFaesFile)) setNoteLabel("Chosen file cannot be encrypted!", 2);
            }
        }

        private void selectEncryptButton_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
            {
                String[] strGetFormats = e.Data.GetFormats();
                e.Effect = DragDropEffects.None;
            }
        }

        private void selectEncryptButton_Click(object sender, EventArgs e)
        {
            if (openFileToEncrypt.ShowDialog() == DialogResult.OK)
            {
                FAES_File tFaesFile = new FAES_File(openFileToEncrypt.FileName);
                setFileToEncrypt(tFaesFile);
            }
        }

        private void combinedPassword_TextChanged(object sender, EventArgs e)
        {
            if (passTextbox.Text.Length > 3 && passConfTextbox.Text.Length > 3) encryptButton.Enabled = true;
            else if (encryptButton.Enabled) encryptButton.Enabled = false;
        }

        private void passHintTextbox_TextChanged(object sender, EventArgs e)
        {
            passHintTextbox.Text = passHintTextbox.Text.Replace(Environment.NewLine, "");
        }

        private void allTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                encryptButton_Click(sender, e);
            }
        }
    }
}
