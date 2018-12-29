using FAES;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace FAES_GUI.MenuPanels
{
    public partial class decryptPanel : UserControl
    {
        private FAES_File _fileToDecrypt;

        private bool _inProgress = false;
        private bool _decryptSuccessful;
        private bool _closeAfterOp = false;

        public decryptPanel()
        {
            Initialise();
        }

        public decryptPanel(FAES_File faesFile)
        {
            Initialise();

            if (!setFileToDecrypt(faesFile))
                throw new Exception("Input file cannot be decrypted!");
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
        }

        public void ResetFile()
        {
            Locked(true);
            _fileToDecrypt = null;
            fileInfoLabel.Text = "No File Selected!";
            passTextbox.Text = "";
            passHintTextbox.Text = "";
            encryptedFileMetaData.Text = "";
        }

        public void LockFileSelect(bool lockFile)
        {
            selectDecryptButton.Enabled = !lockFile;
        }

        public bool setFileToDecrypt(FAES_File faesFile)
        {
            if (faesFile.isFileDecryptable())
            {
                _fileToDecrypt = faesFile;
                fileInfoLabel.Text = _fileToDecrypt.getFileName();
                setMetaData();
                Locked(false);
                decryptButton.Enabled = false;
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

        private void setMetaData()
        {
            int timestamp = FileAES_Utilities.GetEncryptionTimeStamp(_fileToDecrypt.getPath());

            if (timestamp >= 0)
            {
                DateTime dateTime = FileAES_Utilities.UnixTimeStampToDateTime((double)timestamp);
                encryptedFileMetaData.Text = String.Format("Encrypted on {0} at {1}", dateTime.ToString("dd/MM/yyyy"), dateTime.ToString("HH:mm:ss tt"));
            }
            else
            {
                encryptedFileMetaData.Text = String.Format("This file does not contain a encryption date. This is likely due to this file being encrypted using an older FAES version.");
            }

            passHintTextbox.Text = FileAES_Utilities.GetPasswordHint(_fileToDecrypt.getPath());
        }

        private void Locked(bool lockChanges)
        {
            passTextbox.Enabled = !lockChanges;
            passHintTextbox.Enabled = !lockChanges;
            decryptButton.Enabled = !lockChanges;
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
                    FileAES_Decrypt decrypt = new FileAES_Decrypt(_fileToDecrypt, passTextbox.Text);
                    _decryptSuccessful = decrypt.decryptFile();

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
            Locked(false);

            if (_decryptSuccessful)
            {
                setNoteLabel("Decryption Complete", 0);
                if (_closeAfterOp) Application.Exit();
                else ResetFile();
            }
            else
            {
                setNoteLabel("Password Incorrect!", 3);
                passTextbox.Focus();
            }
        }

        private void decryptButton_Click(object sender, EventArgs e)
        {
            if (_fileToDecrypt.isFileDecryptable() && !_inProgress && passTextbox.Text.Length > 3)
            {
                backgroundDecrypt.RunWorkerAsync();
                Locked(true);
            }
            else if (_inProgress) setNoteLabel("Decryption already in progress.", 1);
            else
            {
                setNoteLabel("Decryption Failed. Try again later.", 1);
                decryptButton.Focus();
            }
        }

        private void selectDecryptButton_DragDrop(object sender, DragEventArgs e)
        {
            string[] FileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            if (FileList.Length > 1) setNoteLabel("You may only decrypt a single file or folder at a time.", 2);
            else
            {
                FAES_File tFaesFile = new FAES_File(FileList[0]);
                if (!setFileToDecrypt(tFaesFile)) setNoteLabel("Chosen file cannot be decrypted!", 2);
            }
        }

        private void selectDecryptButton_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
            {
                String[] strGetFormats = e.Data.GetFormats();
                e.Effect = DragDropEffects.None;
            }
        }

        private void selectDecryptButton_Click(object sender, EventArgs e)
        {
            if (openFileToDecrypt.ShowDialog() == DialogResult.OK)
            {
                FAES_File tFaesFile = new FAES_File(openFileToDecrypt.FileName);
                setFileToDecrypt(tFaesFile);
            }
        }

        private void combinedPassword_TextChanged(object sender, EventArgs e)
        {
            if (passTextbox.Text.Length > 3) decryptButton.Enabled = true;
            else if (decryptButton.Enabled) decryptButton.Enabled = false;
        }

        private void allTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                decryptButton_Click(sender, e);
            }
        }
    }
}