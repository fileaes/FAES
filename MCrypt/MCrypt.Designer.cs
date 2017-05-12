namespace MCrypt
{
    partial class MCrypt
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.openFileToEncrypt = new System.Windows.Forms.OpenFileDialog();
            this.Runtime = new System.Windows.Forms.Timer(this.components);
            this.openFileToDecrypt = new System.Windows.Forms.OpenFileDialog();
            this.copyrightLabel = new System.Windows.Forms.Label();
            this.encryptMasterGroup = new System.Windows.Forms.GroupBox();
            this.encryptPasswordGroup = new System.Windows.Forms.GroupBox();
            this.passwordInputEncryptConf = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.warningEncrypt = new System.Windows.Forms.Label();
            this.deleteOnEncrypt = new System.Windows.Forms.CheckBox();
            this.encryptFile = new System.Windows.Forms.Button();
            this.showPassCheckEncrypt = new System.Windows.Forms.CheckBox();
            this.passwordInputEncrypt = new System.Windows.Forms.TextBox();
            this.randomPassEncrypt = new System.Windows.Forms.Button();
            this.encryptPasswordLabel = new System.Windows.Forms.Label();
            this.openEncryptDiag = new System.Windows.Forms.Button();
            this.fileToEncrypt = new System.Windows.Forms.TextBox();
            this.encryptFilenameLabel = new System.Windows.Forms.Label();
            this.decryptMasterGroup = new System.Windows.Forms.GroupBox();
            this.decryptPasswordGroup = new System.Windows.Forms.GroupBox();
            this.warningDecrypt = new System.Windows.Forms.Label();
            this.deleteOnDecrypt = new System.Windows.Forms.CheckBox();
            this.decryptFile = new System.Windows.Forms.Button();
            this.showPassCheckDecrypt = new System.Windows.Forms.CheckBox();
            this.passwordInputDecrypt = new System.Windows.Forms.TextBox();
            this.decryptPasswordLabel = new System.Windows.Forms.Label();
            this.openDecryptDiag = new System.Windows.Forms.Button();
            this.fileToDecrypt = new System.Windows.Forms.TextBox();
            this.decryptFilenameLabel = new System.Windows.Forms.Label();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.encryptMasterGroup.SuspendLayout();
            this.encryptPasswordGroup.SuspendLayout();
            this.decryptMasterGroup.SuspendLayout();
            this.decryptPasswordGroup.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileToEncrypt
            // 
            this.openFileToEncrypt.Filter = "All Files|*.*";
            // 
            // Runtime
            // 
            this.Runtime.Enabled = true;
            this.Runtime.Tick += new System.EventHandler(this.Runtime_Tick);
            // 
            // openFileToDecrypt
            // 
            this.openFileToDecrypt.Filter = "SecureAES Files|*.encrypted;*.aes;*.secureaes;*.mcrypt";
            // 
            // copyrightLabel
            // 
            this.copyrightLabel.AutoSize = true;
            this.copyrightLabel.Location = new System.Drawing.Point(328, 510);
            this.copyrightLabel.Name = "copyrightLabel";
            this.copyrightLabel.Size = new System.Drawing.Size(88, 13);
            this.copyrightLabel.TabIndex = 15;
            this.copyrightLabel.Text = "mullak99 © 2017";
            // 
            // encryptMasterGroup
            // 
            this.encryptMasterGroup.Controls.Add(this.encryptPasswordGroup);
            this.encryptMasterGroup.Controls.Add(this.openEncryptDiag);
            this.encryptMasterGroup.Controls.Add(this.fileToEncrypt);
            this.encryptMasterGroup.Controls.Add(this.encryptFilenameLabel);
            this.encryptMasterGroup.Location = new System.Drawing.Point(12, 27);
            this.encryptMasterGroup.Name = "encryptMasterGroup";
            this.encryptMasterGroup.Size = new System.Drawing.Size(390, 237);
            this.encryptMasterGroup.TabIndex = 13;
            this.encryptMasterGroup.TabStop = false;
            this.encryptMasterGroup.Text = "Encrypt";
            // 
            // encryptPasswordGroup
            // 
            this.encryptPasswordGroup.Controls.Add(this.passwordInputEncryptConf);
            this.encryptPasswordGroup.Controls.Add(this.label1);
            this.encryptPasswordGroup.Controls.Add(this.warningEncrypt);
            this.encryptPasswordGroup.Controls.Add(this.deleteOnEncrypt);
            this.encryptPasswordGroup.Controls.Add(this.encryptFile);
            this.encryptPasswordGroup.Controls.Add(this.showPassCheckEncrypt);
            this.encryptPasswordGroup.Controls.Add(this.passwordInputEncrypt);
            this.encryptPasswordGroup.Controls.Add(this.randomPassEncrypt);
            this.encryptPasswordGroup.Controls.Add(this.encryptPasswordLabel);
            this.encryptPasswordGroup.Location = new System.Drawing.Point(9, 51);
            this.encryptPasswordGroup.Name = "encryptPasswordGroup";
            this.encryptPasswordGroup.Size = new System.Drawing.Size(375, 180);
            this.encryptPasswordGroup.TabIndex = 4;
            this.encryptPasswordGroup.TabStop = false;
            // 
            // passwordInputEncryptConf
            // 
            this.passwordInputEncryptConf.Location = new System.Drawing.Point(59, 61);
            this.passwordInputEncryptConf.Name = "passwordInputEncryptConf";
            this.passwordInputEncryptConf.PasswordChar = '*';
            this.passwordInputEncryptConf.Size = new System.Drawing.Size(250, 20);
            this.passwordInputEncryptConf.TabIndex = 4;
            this.passwordInputEncryptConf.TextChanged += new System.EventHandler(this.passwordInputEncrypt_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 26);
            this.label1.TabIndex = 17;
            this.label1.Text = "Conf.\r\nPassword:";
            // 
            // warningEncrypt
            // 
            this.warningEncrypt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.warningEncrypt.Location = new System.Drawing.Point(220, 91);
            this.warningEncrypt.Name = "warningEncrypt";
            this.warningEncrypt.Size = new System.Drawing.Size(151, 46);
            this.warningEncrypt.TabIndex = 16;
            this.warningEncrypt.Text = "PLACEHOLDER TEXT";
            this.warningEncrypt.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // deleteOnEncrypt
            // 
            this.deleteOnEncrypt.AutoSize = true;
            this.deleteOnEncrypt.Location = new System.Drawing.Point(10, 150);
            this.deleteOnEncrypt.Name = "deleteOnEncrypt";
            this.deleteOnEncrypt.Size = new System.Drawing.Size(163, 17);
            this.deleteOnEncrypt.TabIndex = 7;
            this.deleteOnEncrypt.Text = "Delete Original on Encryption";
            this.deleteOnEncrypt.UseVisualStyleBackColor = true;
            // 
            // encryptFile
            // 
            this.encryptFile.Enabled = false;
            this.encryptFile.Location = new System.Drawing.Point(220, 140);
            this.encryptFile.Name = "encryptFile";
            this.encryptFile.Size = new System.Drawing.Size(151, 34);
            this.encryptFile.TabIndex = 8;
            this.encryptFile.Text = "Encrypt/Save";
            this.encryptFile.UseVisualStyleBackColor = true;
            this.encryptFile.Click += new System.EventHandler(this.encryptFile_Click);
            // 
            // showPassCheckEncrypt
            // 
            this.showPassCheckEncrypt.AutoSize = true;
            this.showPassCheckEncrypt.Location = new System.Drawing.Point(60, 87);
            this.showPassCheckEncrypt.Name = "showPassCheckEncrypt";
            this.showPassCheckEncrypt.Size = new System.Drawing.Size(102, 17);
            this.showPassCheckEncrypt.TabIndex = 6;
            this.showPassCheckEncrypt.Text = "Show Password";
            this.showPassCheckEncrypt.UseVisualStyleBackColor = true;
            this.showPassCheckEncrypt.CheckedChanged += new System.EventHandler(this.showPassCheckEncrypt_CheckedChanged);
            // 
            // passwordInputEncrypt
            // 
            this.passwordInputEncrypt.Location = new System.Drawing.Point(59, 26);
            this.passwordInputEncrypt.Name = "passwordInputEncrypt";
            this.passwordInputEncrypt.PasswordChar = '*';
            this.passwordInputEncrypt.Size = new System.Drawing.Size(250, 20);
            this.passwordInputEncrypt.TabIndex = 3;
            this.passwordInputEncrypt.TextChanged += new System.EventHandler(this.passwordInputEncrypt_TextChanged);
            // 
            // randomPassEncrypt
            // 
            this.randomPassEncrypt.Location = new System.Drawing.Point(314, 24);
            this.randomPassEncrypt.Name = "randomPassEncrypt";
            this.randomPassEncrypt.Size = new System.Drawing.Size(55, 23);
            this.randomPassEncrypt.TabIndex = 5;
            this.randomPassEncrypt.Text = "Random";
            this.randomPassEncrypt.UseVisualStyleBackColor = true;
            this.randomPassEncrypt.Click += new System.EventHandler(this.randomPassEncrypt_Click);
            // 
            // encryptPasswordLabel
            // 
            this.encryptPasswordLabel.AutoSize = true;
            this.encryptPasswordLabel.Location = new System.Drawing.Point(6, 29);
            this.encryptPasswordLabel.Name = "encryptPasswordLabel";
            this.encryptPasswordLabel.Size = new System.Drawing.Size(56, 13);
            this.encryptPasswordLabel.TabIndex = 3;
            this.encryptPasswordLabel.Text = "Password:";
            // 
            // openEncryptDiag
            // 
            this.openEncryptDiag.Location = new System.Drawing.Point(324, 23);
            this.openEncryptDiag.Name = "openEncryptDiag";
            this.openEncryptDiag.Size = new System.Drawing.Size(55, 23);
            this.openEncryptDiag.TabIndex = 2;
            this.openEncryptDiag.Text = "Open";
            this.openEncryptDiag.UseVisualStyleBackColor = true;
            this.openEncryptDiag.Click += new System.EventHandler(this.openEncryptDiag_Click);
            // 
            // fileToEncrypt
            // 
            this.fileToEncrypt.Location = new System.Drawing.Point(30, 25);
            this.fileToEncrypt.Name = "fileToEncrypt";
            this.fileToEncrypt.Size = new System.Drawing.Size(288, 20);
            this.fileToEncrypt.TabIndex = 1;
            // 
            // encryptFilenameLabel
            // 
            this.encryptFilenameLabel.AutoSize = true;
            this.encryptFilenameLabel.Location = new System.Drawing.Point(6, 28);
            this.encryptFilenameLabel.Name = "encryptFilenameLabel";
            this.encryptFilenameLabel.Size = new System.Drawing.Size(26, 13);
            this.encryptFilenameLabel.TabIndex = 0;
            this.encryptFilenameLabel.Text = "File:";
            // 
            // decryptMasterGroup
            // 
            this.decryptMasterGroup.Controls.Add(this.decryptPasswordGroup);
            this.decryptMasterGroup.Controls.Add(this.openDecryptDiag);
            this.decryptMasterGroup.Controls.Add(this.fileToDecrypt);
            this.decryptMasterGroup.Controls.Add(this.decryptFilenameLabel);
            this.decryptMasterGroup.Location = new System.Drawing.Point(13, 270);
            this.decryptMasterGroup.Name = "decryptMasterGroup";
            this.decryptMasterGroup.Size = new System.Drawing.Size(390, 237);
            this.decryptMasterGroup.TabIndex = 14;
            this.decryptMasterGroup.TabStop = false;
            this.decryptMasterGroup.Text = "Decrypt";
            // 
            // decryptPasswordGroup
            // 
            this.decryptPasswordGroup.Controls.Add(this.warningDecrypt);
            this.decryptPasswordGroup.Controls.Add(this.deleteOnDecrypt);
            this.decryptPasswordGroup.Controls.Add(this.decryptFile);
            this.decryptPasswordGroup.Controls.Add(this.showPassCheckDecrypt);
            this.decryptPasswordGroup.Controls.Add(this.passwordInputDecrypt);
            this.decryptPasswordGroup.Controls.Add(this.decryptPasswordLabel);
            this.decryptPasswordGroup.Location = new System.Drawing.Point(9, 51);
            this.decryptPasswordGroup.Name = "decryptPasswordGroup";
            this.decryptPasswordGroup.Size = new System.Drawing.Size(375, 180);
            this.decryptPasswordGroup.TabIndex = 4;
            this.decryptPasswordGroup.TabStop = false;
            // 
            // warningDecrypt
            // 
            this.warningDecrypt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.warningDecrypt.Location = new System.Drawing.Point(219, 91);
            this.warningDecrypt.Name = "warningDecrypt";
            this.warningDecrypt.Size = new System.Drawing.Size(151, 46);
            this.warningDecrypt.TabIndex = 15;
            this.warningDecrypt.Text = "PLACEHOLDER TEXT";
            this.warningDecrypt.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // deleteOnDecrypt
            // 
            this.deleteOnDecrypt.AutoSize = true;
            this.deleteOnDecrypt.Location = new System.Drawing.Point(9, 150);
            this.deleteOnDecrypt.Name = "deleteOnDecrypt";
            this.deleteOnDecrypt.Size = new System.Drawing.Size(177, 17);
            this.deleteOnDecrypt.TabIndex = 13;
            this.deleteOnDecrypt.Text = "Delete Encrypted on Decryption";
            this.deleteOnDecrypt.UseVisualStyleBackColor = true;
            // 
            // decryptFile
            // 
            this.decryptFile.Enabled = false;
            this.decryptFile.Location = new System.Drawing.Point(219, 140);
            this.decryptFile.Name = "decryptFile";
            this.decryptFile.Size = new System.Drawing.Size(151, 34);
            this.decryptFile.TabIndex = 14;
            this.decryptFile.Text = "Decrypt/Save";
            this.decryptFile.UseVisualStyleBackColor = true;
            this.decryptFile.Click += new System.EventHandler(this.decryptFile_Click);
            // 
            // showPassCheckDecrypt
            // 
            this.showPassCheckDecrypt.AutoSize = true;
            this.showPassCheckDecrypt.Location = new System.Drawing.Point(59, 52);
            this.showPassCheckDecrypt.Name = "showPassCheckDecrypt";
            this.showPassCheckDecrypt.Size = new System.Drawing.Size(102, 17);
            this.showPassCheckDecrypt.TabIndex = 12;
            this.showPassCheckDecrypt.Text = "Show Password";
            this.showPassCheckDecrypt.UseVisualStyleBackColor = true;
            // 
            // passwordInputDecrypt
            // 
            this.passwordInputDecrypt.Location = new System.Drawing.Point(59, 26);
            this.passwordInputDecrypt.Name = "passwordInputDecrypt";
            this.passwordInputDecrypt.PasswordChar = '*';
            this.passwordInputDecrypt.Size = new System.Drawing.Size(250, 20);
            this.passwordInputDecrypt.TabIndex = 11;
            this.passwordInputDecrypt.TextChanged += new System.EventHandler(this.passwordInputDecrypt_TextChanged);
            // 
            // decryptPasswordLabel
            // 
            this.decryptPasswordLabel.AutoSize = true;
            this.decryptPasswordLabel.Location = new System.Drawing.Point(6, 29);
            this.decryptPasswordLabel.Name = "decryptPasswordLabel";
            this.decryptPasswordLabel.Size = new System.Drawing.Size(56, 13);
            this.decryptPasswordLabel.TabIndex = 3;
            this.decryptPasswordLabel.Text = "Password:";
            // 
            // openDecryptDiag
            // 
            this.openDecryptDiag.Location = new System.Drawing.Point(324, 23);
            this.openDecryptDiag.Name = "openDecryptDiag";
            this.openDecryptDiag.Size = new System.Drawing.Size(55, 23);
            this.openDecryptDiag.TabIndex = 10;
            this.openDecryptDiag.Text = "Open";
            this.openDecryptDiag.UseVisualStyleBackColor = true;
            this.openDecryptDiag.Click += new System.EventHandler(this.openDecryptDiag_Click);
            // 
            // fileToDecrypt
            // 
            this.fileToDecrypt.Location = new System.Drawing.Point(30, 25);
            this.fileToDecrypt.Name = "fileToDecrypt";
            this.fileToDecrypt.Size = new System.Drawing.Size(288, 20);
            this.fileToDecrypt.TabIndex = 9;
            // 
            // decryptFilenameLabel
            // 
            this.decryptFilenameLabel.AutoSize = true;
            this.decryptFilenameLabel.Location = new System.Drawing.Point(6, 28);
            this.decryptFilenameLabel.Name = "decryptFilenameLabel";
            this.decryptFilenameLabel.Size = new System.Drawing.Size(26, 13);
            this.decryptFilenameLabel.TabIndex = 0;
            this.decryptFilenameLabel.Text = "File:";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(414, 24);
            this.menuStrip.TabIndex = 16;
            this.menuStrip.Text = "menuStrip1";
            // 
            // MCrypt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 525);
            this.Controls.Add(this.menuStrip);
            this.Controls.Add(this.copyrightLabel);
            this.Controls.Add(this.encryptMasterGroup);
            this.Controls.Add(this.decryptMasterGroup);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MCrypt";
            this.Text = "MCrypt";
            this.encryptMasterGroup.ResumeLayout(false);
            this.encryptMasterGroup.PerformLayout();
            this.encryptPasswordGroup.ResumeLayout(false);
            this.encryptPasswordGroup.PerformLayout();
            this.decryptMasterGroup.ResumeLayout(false);
            this.decryptMasterGroup.PerformLayout();
            this.decryptPasswordGroup.ResumeLayout(false);
            this.decryptPasswordGroup.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog openFileToEncrypt;
        private System.Windows.Forms.Timer Runtime;
        private System.Windows.Forms.OpenFileDialog openFileToDecrypt;
        private System.Windows.Forms.Label copyrightLabel;
        private System.Windows.Forms.GroupBox encryptMasterGroup;
        private System.Windows.Forms.GroupBox encryptPasswordGroup;
        private System.Windows.Forms.TextBox passwordInputEncryptConf;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label warningEncrypt;
        private System.Windows.Forms.CheckBox deleteOnEncrypt;
        private System.Windows.Forms.Button encryptFile;
        private System.Windows.Forms.CheckBox showPassCheckEncrypt;
        private System.Windows.Forms.TextBox passwordInputEncrypt;
        private System.Windows.Forms.Button randomPassEncrypt;
        private System.Windows.Forms.Label encryptPasswordLabel;
        private System.Windows.Forms.Button openEncryptDiag;
        private System.Windows.Forms.TextBox fileToEncrypt;
        private System.Windows.Forms.Label encryptFilenameLabel;
        private System.Windows.Forms.GroupBox decryptMasterGroup;
        private System.Windows.Forms.GroupBox decryptPasswordGroup;
        private System.Windows.Forms.Label warningDecrypt;
        private System.Windows.Forms.CheckBox deleteOnDecrypt;
        private System.Windows.Forms.Button decryptFile;
        private System.Windows.Forms.CheckBox showPassCheckDecrypt;
        private System.Windows.Forms.TextBox passwordInputDecrypt;
        private System.Windows.Forms.Label decryptPasswordLabel;
        private System.Windows.Forms.Button openDecryptDiag;
        private System.Windows.Forms.TextBox fileToDecrypt;
        private System.Windows.Forms.Label decryptFilenameLabel;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip;
    }
}

