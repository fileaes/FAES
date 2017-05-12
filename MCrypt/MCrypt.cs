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
    public partial class MCrypt : Form
    {
        public MCrypt()
        {
            InitializeComponent();
            //versionLabel.Text = "v" + Application.ProductVersion;
            warningEncrypt.Text = "";
            warningDecrypt.Text = "";
        }

        SecureAES aes = new SecureAES();
        Core core = new Core();
        //Form about = new About();

        private void openEncryptDiag_Click(object sender, EventArgs e)
        {
            if (openFileToEncrypt.ShowDialog() == DialogResult.OK)
                fileToEncrypt.Text = openFileToEncrypt.FileName;
        }

        private void randomPassEncrypt_Click(object sender, EventArgs e)
        {
            passwordInputEncrypt.Text = aes.CreateRandomPassword(32);
            passwordInputEncrypt_TextChanged(sender, e);
        }

        private void passwordInputEncrypt_TextChanged(object sender, EventArgs e)
        {
            if (showPassCheckEncrypt.Checked)
            {
                passwordInputEncrypt.PasswordChar = '\0';
                passwordInputEncryptConf.PasswordChar = '\0';
            }
            else
            {
                passwordInputEncrypt.PasswordChar = '*';
                passwordInputEncryptConf.PasswordChar = '*';
            }
        }

        private void showPassCheckEncrypt_CheckedChanged(object sender, EventArgs e)
        {
            passwordInputEncrypt_TextChanged(sender, e);
        }

        private void encryptFile_Click(object sender, EventArgs e)
        {
            if (core.isEncryptFileValid(fileToEncrypt.Text) && passwordInputEncryptConf.Text == passwordInputEncrypt.Text)
            {
                aes.AES_Encrypt(fileToEncrypt.Text, passwordInputEncrypt.Text);
                if (deleteOnEncrypt.Checked) File.Delete(fileToEncrypt.Text);
            }
            else if (passwordInputEncryptConf.Text != passwordInputEncrypt.Text)
            {
                warningEncrypt.ForeColor = Color.Red;
                warningEncrypt.Text = "Passwords do not match!";
            }
            else if (aes.getLastError() == "encryptSuccess")
            {
                warningEncrypt.ForeColor = Color.Green;
                warningEncrypt.Text = "Successfully Encrypted!";
            }
            else
            {
                warningEncrypt.ForeColor = Color.Red;
                warningEncrypt.Text = "An unknown error occured!";
            }
        }

        private void Runtime_Tick(object sender, EventArgs e)
        {
            encryptFile.Enabled = core.isEncryptFileValid(fileToEncrypt.Text);
            decryptFile.Enabled = core.isDecryptFileValid(fileToDecrypt.Text);
        }

        private void passwordInputDecrypt_TextChanged(object sender, EventArgs e)
        {
            if (showPassCheckDecrypt.Checked)
                passwordInputDecrypt.PasswordChar = '\0';
            else
                passwordInputDecrypt.PasswordChar = '*';
        }

        private void showPassCheckDecrypt_CheckedChanged(object sender, EventArgs e)
        {
            passwordInputDecrypt_TextChanged(sender, e);
        }

        private void openDecryptDiag_Click(object sender, EventArgs e)
        {
            if (openFileToDecrypt.ShowDialog() == DialogResult.OK)
                fileToDecrypt.Text = openFileToDecrypt.FileName;
        }

        private void decryptFile_Click(object sender, EventArgs e)
        {
            if (core.isDecryptFileValid(fileToDecrypt.Text))
                aes.AES_Decrypt(fileToDecrypt.Text, passwordInputDecrypt.Text);

            if (aes.getLastError() == "decryptIncorrectPassword")
            {
                warningDecrypt.ForeColor = Color.Red;
                warningDecrypt.Text = "Password Incorrect!";
            }
            else if (aes.getLastError() == "decryptSuccess")
            {
                warningDecrypt.ForeColor = Color.Green;
                warningDecrypt.Text = "Successfully Decrypted!";
                if (deleteOnDecrypt.Checked) File.Delete(fileToDecrypt.Text);
            }
            else
            {
                warningDecrypt.ForeColor = Color.Red;
                warningDecrypt.Text = "An unknown error occured!";
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*if (about.IsDisposed)
                about = new About();

            about.Show();*/
        }
    }
}
