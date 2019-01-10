namespace FileAES
{
    partial class FileAES_Encrypt
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileAES_Encrypt));
            this.pathLabel = new System.Windows.Forms.Label();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.passwordConfLabel = new System.Windows.Forms.Label();
            this.passwordInput = new System.Windows.Forms.TextBox();
            this.passwordInputConf = new System.Windows.Forms.TextBox();
            this.encryptButton = new System.Windows.Forms.Button();
            this.runtime = new System.Windows.Forms.Timer(this.components);
            this.backgroundEncrypt = new System.ComponentModel.BackgroundWorker();
            this.noteLabel = new System.Windows.Forms.Label();
            this.fileName = new System.Windows.Forms.Label();
            this.copyrightLabel = new System.Windows.Forms.Label();
            this.versionLabel = new System.Windows.Forms.Label();
            this.passwordHintLabel = new System.Windows.Forms.Label();
            this.hintInput = new System.Windows.Forms.TextBox();
            this.slowToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.compressModeLabel = new System.Windows.Forms.Label();
            this.compressMode = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // pathLabel
            // 
            this.pathLabel.AutoSize = true;
            this.pathLabel.Location = new System.Drawing.Point(12, 9);
            this.pathLabel.Name = "pathLabel";
            this.pathLabel.Size = new System.Drawing.Size(32, 13);
            this.pathLabel.TabIndex = 0;
            this.pathLabel.Text = "Path:";
            // 
            // passwordLabel
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Location = new System.Drawing.Point(12, 46);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(56, 13);
            this.passwordLabel.TabIndex = 2;
            this.passwordLabel.Text = "Password:";
            // 
            // passwordConfLabel
            // 
            this.passwordConfLabel.AutoSize = true;
            this.passwordConfLabel.Location = new System.Drawing.Point(12, 71);
            this.passwordConfLabel.Name = "passwordConfLabel";
            this.passwordConfLabel.Size = new System.Drawing.Size(56, 26);
            this.passwordConfLabel.TabIndex = 3;
            this.passwordConfLabel.Text = "Conf.\r\nPassword:";
            // 
            // passwordInput
            // 
            this.passwordInput.Location = new System.Drawing.Point(74, 43);
            this.passwordInput.Name = "passwordInput";
            this.passwordInput.PasswordChar = '*';
            this.passwordInput.Size = new System.Drawing.Size(167, 20);
            this.passwordInput.TabIndex = 4;
            this.passwordInput.Enter += new System.EventHandler(this.passwordBox_Focus);
            // 
            // passwordInputConf
            // 
            this.passwordInputConf.Location = new System.Drawing.Point(74, 77);
            this.passwordInputConf.Name = "passwordInputConf";
            this.passwordInputConf.PasswordChar = '*';
            this.passwordInputConf.Size = new System.Drawing.Size(167, 20);
            this.passwordInputConf.TabIndex = 5;
            this.passwordInputConf.Enter += new System.EventHandler(this.passwordBox_Focus);
            // 
            // encryptButton
            // 
            this.encryptButton.Enabled = false;
            this.encryptButton.Location = new System.Drawing.Point(12, 212);
            this.encryptButton.Name = "encryptButton";
            this.encryptButton.Size = new System.Drawing.Size(228, 23);
            this.encryptButton.TabIndex = 8;
            this.encryptButton.Text = "Encrypt";
            this.encryptButton.UseVisualStyleBackColor = true;
            this.encryptButton.Click += new System.EventHandler(this.encryptButton_Click);
            // 
            // runtime
            // 
            this.runtime.Enabled = true;
            this.runtime.Tick += new System.EventHandler(this.runtime_Tick);
            // 
            // backgroundEncrypt
            // 
            this.backgroundEncrypt.WorkerReportsProgress = true;
            this.backgroundEncrypt.WorkerSupportsCancellation = true;
            this.backgroundEncrypt.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundEncrypt_DoWork);
            this.backgroundEncrypt.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundEncrypt_Complete);
            // 
            // noteLabel
            // 
            this.noteLabel.AutoEllipsis = true;
            this.noteLabel.BackColor = System.Drawing.SystemColors.Control;
            this.noteLabel.ForeColor = System.Drawing.Color.Black;
            this.noteLabel.Location = new System.Drawing.Point(9, 179);
            this.noteLabel.Name = "noteLabel";
            this.noteLabel.Size = new System.Drawing.Size(235, 30);
            this.noteLabel.TabIndex = 7;
            this.noteLabel.Text = "Note: Press \'Encrypt\' to encrypt your chosen file.";
            this.noteLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.noteLabel.MouseHover += new System.EventHandler(this.noteLabel_MouseHover);
            // 
            // fileName
            // 
            this.fileName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fileName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fileName.ForeColor = System.Drawing.SystemColors.ControlText;
            this.fileName.Location = new System.Drawing.Point(45, 8);
            this.fileName.Name = "fileName";
            this.fileName.Size = new System.Drawing.Size(196, 17);
            this.fileName.TabIndex = 8;
            this.fileName.Text = "PLACEHOLDER";
            this.fileName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // copyrightLabel
            // 
            this.copyrightLabel.AutoSize = true;
            this.copyrightLabel.Location = new System.Drawing.Point(165, 238);
            this.copyrightLabel.Name = "copyrightLabel";
            this.copyrightLabel.Size = new System.Drawing.Size(88, 13);
            this.copyrightLabel.TabIndex = 18;
            this.copyrightLabel.Text = "mullak99 © 2018";
            // 
            // versionLabel
            // 
            this.versionLabel.Location = new System.Drawing.Point(-1, 238);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(160, 13);
            this.versionLabel.TabIndex = 19;
            this.versionLabel.Text = "v0.0.0.0";
            this.versionLabel.Click += new System.EventHandler(this.versionLabel_Click);
            // 
            // passwordHintLabel
            // 
            this.passwordHintLabel.AutoSize = true;
            this.passwordHintLabel.Location = new System.Drawing.Point(12, 108);
            this.passwordHintLabel.Name = "passwordHintLabel";
            this.passwordHintLabel.Size = new System.Drawing.Size(53, 26);
            this.passwordHintLabel.TabIndex = 20;
            this.passwordHintLabel.Text = "Password\r\nHint:";
            // 
            // hintInput
            // 
            this.hintInput.Location = new System.Drawing.Point(74, 108);
            this.hintInput.MaxLength = 64;
            this.hintInput.Multiline = true;
            this.hintInput.Name = "hintInput";
            this.hintInput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.hintInput.Size = new System.Drawing.Size(167, 26);
            this.hintInput.TabIndex = 6;
            this.hintInput.TextChanged += new System.EventHandler(this.hintInput_TextChanged);
            // 
            // slowToolTip
            // 
            this.slowToolTip.AutoPopDelay = 5000;
            this.slowToolTip.InitialDelay = 1000;
            this.slowToolTip.ReshowDelay = 100;
            // 
            // compressModeLabel
            // 
            this.compressModeLabel.AutoSize = true;
            this.compressModeLabel.Location = new System.Drawing.Point(12, 151);
            this.compressModeLabel.Name = "compressModeLabel";
            this.compressModeLabel.Size = new System.Drawing.Size(37, 13);
            this.compressModeLabel.TabIndex = 21;
            this.compressModeLabel.Text = "Mode:";
            // 
            // compressMode
            // 
            this.compressMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.compressMode.FormattingEnabled = true;
            this.compressMode.Location = new System.Drawing.Point(74, 148);
            this.compressMode.Name = "compressMode";
            this.compressMode.Size = new System.Drawing.Size(167, 21);
            this.compressMode.TabIndex = 7;
            // 
            // FileAES_Encrypt
            // 
            this.AcceptButton = this.encryptButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(252, 253);
            this.Controls.Add(this.compressMode);
            this.Controls.Add(this.compressModeLabel);
            this.Controls.Add(this.hintInput);
            this.Controls.Add(this.passwordHintLabel);
            this.Controls.Add(this.versionLabel);
            this.Controls.Add(this.copyrightLabel);
            this.Controls.Add(this.fileName);
            this.Controls.Add(this.noteLabel);
            this.Controls.Add(this.encryptButton);
            this.Controls.Add(this.passwordInputConf);
            this.Controls.Add(this.passwordInput);
            this.Controls.Add(this.passwordConfLabel);
            this.Controls.Add(this.passwordLabel);
            this.Controls.Add(this.pathLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FileAES_Encrypt";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FileAES: Encrypt";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FileAES_Encrypt_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label pathLabel;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.Label passwordConfLabel;
        private System.Windows.Forms.TextBox passwordInput;
        private System.Windows.Forms.TextBox passwordInputConf;
        private System.Windows.Forms.Button encryptButton;
        private System.Windows.Forms.Timer runtime;
        private System.ComponentModel.BackgroundWorker backgroundEncrypt;
        private System.Windows.Forms.Label noteLabel;
        private System.Windows.Forms.Label fileName;
        private System.Windows.Forms.Label copyrightLabel;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.Label passwordHintLabel;
        private System.Windows.Forms.TextBox hintInput;
        private System.Windows.Forms.ToolTip slowToolTip;
        private System.Windows.Forms.Label compressModeLabel;
        private System.Windows.Forms.ComboBox compressMode;
    }
}