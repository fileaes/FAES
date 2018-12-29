namespace FileAES
{
    partial class FileAES_Decrypt
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileAES_Decrypt));
            this.fileName = new System.Windows.Forms.Label();
            this.decryptButton = new System.Windows.Forms.Button();
            this.passwordInput = new System.Windows.Forms.TextBox();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.pathLabel = new System.Windows.Forms.Label();
            this.runtime = new System.Windows.Forms.Timer(this.components);
            this.backgroundDecrypt = new System.ComponentModel.BackgroundWorker();
            this.noteLabel = new System.Windows.Forms.Label();
            this.copyrightLabel = new System.Windows.Forms.Label();
            this.versionLabel = new System.Windows.Forms.Label();
            this.hintLabel = new System.Windows.Forms.Label();
            this.hintTextbox = new System.Windows.Forms.TextBox();
            this.slowToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // fileName
            // 
            this.fileName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fileName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fileName.ForeColor = System.Drawing.SystemColors.ControlText;
            this.fileName.Location = new System.Drawing.Point(45, 8);
            this.fileName.Name = "fileName";
            this.fileName.Size = new System.Drawing.Size(196, 17);
            this.fileName.TabIndex = 16;
            this.fileName.Text = "PLACEHOLDER";
            this.fileName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // decryptButton
            // 
            this.decryptButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.decryptButton.Enabled = false;
            this.decryptButton.Location = new System.Drawing.Point(12, 126);
            this.decryptButton.Name = "decryptButton";
            this.decryptButton.Size = new System.Drawing.Size(228, 23);
            this.decryptButton.TabIndex = 14;
            this.decryptButton.Text = "Decrypt";
            this.decryptButton.UseVisualStyleBackColor = true;
            this.decryptButton.Click += new System.EventHandler(this.decryptButton_Click);
            // 
            // passwordInput
            // 
            this.passwordInput.Location = new System.Drawing.Point(74, 43);
            this.passwordInput.Name = "passwordInput";
            this.passwordInput.PasswordChar = '*';
            this.passwordInput.Size = new System.Drawing.Size(167, 20);
            this.passwordInput.TabIndex = 12;
            this.passwordInput.Enter += new System.EventHandler(this.passwordBox_Focus);
            // 
            // passwordLabel
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Location = new System.Drawing.Point(12, 46);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(56, 13);
            this.passwordLabel.TabIndex = 10;
            this.passwordLabel.Text = "Password:";
            // 
            // pathLabel
            // 
            this.pathLabel.AutoSize = true;
            this.pathLabel.Location = new System.Drawing.Point(12, 9);
            this.pathLabel.Name = "pathLabel";
            this.pathLabel.Size = new System.Drawing.Size(32, 13);
            this.pathLabel.TabIndex = 9;
            this.pathLabel.Text = "Path:";
            // 
            // runtime
            // 
            this.runtime.Enabled = true;
            this.runtime.Tick += new System.EventHandler(this.runtime_Tick);
            // 
            // backgroundDecrypt
            // 
            this.backgroundDecrypt.WorkerReportsProgress = true;
            this.backgroundDecrypt.WorkerSupportsCancellation = true;
            this.backgroundDecrypt.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundDecrypt_DoWork);
            this.backgroundDecrypt.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundDecrypt_Complete);
            // 
            // noteLabel
            // 
            this.noteLabel.AutoEllipsis = true;
            this.noteLabel.BackColor = System.Drawing.SystemColors.Control;
            this.noteLabel.ForeColor = System.Drawing.Color.Black;
            this.noteLabel.Location = new System.Drawing.Point(8, 100);
            this.noteLabel.Name = "noteLabel";
            this.noteLabel.Size = new System.Drawing.Size(236, 23);
            this.noteLabel.TabIndex = 15;
            this.noteLabel.Text = "Note: Press \'Decrypt\' to decrypt your chosen file.";
            this.noteLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.noteLabel.MouseHover += new System.EventHandler(this.noteLabel_MouseHover);
            // 
            // copyrightLabel
            // 
            this.copyrightLabel.AutoSize = true;
            this.copyrightLabel.Location = new System.Drawing.Point(164, 154);
            this.copyrightLabel.Name = "copyrightLabel";
            this.copyrightLabel.Size = new System.Drawing.Size(88, 13);
            this.copyrightLabel.TabIndex = 17;
            this.copyrightLabel.Text = "mullak99 © 2018";
            // 
            // versionLabel
            // 
            this.versionLabel.Location = new System.Drawing.Point(0, 154);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(158, 13);
            this.versionLabel.TabIndex = 18;
            this.versionLabel.Text = "v0.0.0.0";
            this.versionLabel.Click += new System.EventHandler(this.versionLabel_Click);
            // 
            // hintLabel
            // 
            this.hintLabel.AutoSize = true;
            this.hintLabel.Location = new System.Drawing.Point(12, 71);
            this.hintLabel.Name = "hintLabel";
            this.hintLabel.Size = new System.Drawing.Size(53, 26);
            this.hintLabel.TabIndex = 19;
            this.hintLabel.Text = "Password\r\nHint:";
            // 
            // hintTextbox
            // 
            this.hintTextbox.Location = new System.Drawing.Point(74, 71);
            this.hintTextbox.MaxLength = 64;
            this.hintTextbox.Multiline = true;
            this.hintTextbox.Name = "hintTextbox";
            this.hintTextbox.ReadOnly = true;
            this.hintTextbox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.hintTextbox.Size = new System.Drawing.Size(167, 26);
            this.hintTextbox.TabIndex = 22;
            // 
            // slowToolTip
            // 
            this.slowToolTip.AutoPopDelay = 5000;
            this.slowToolTip.InitialDelay = 1000;
            this.slowToolTip.ReshowDelay = 100;
            // 
            // FileAES_Decrypt
            // 
            this.AcceptButton = this.decryptButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(251, 168);
            this.Controls.Add(this.hintTextbox);
            this.Controls.Add(this.hintLabel);
            this.Controls.Add(this.versionLabel);
            this.Controls.Add(this.copyrightLabel);
            this.Controls.Add(this.fileName);
            this.Controls.Add(this.decryptButton);
            this.Controls.Add(this.passwordInput);
            this.Controls.Add(this.passwordLabel);
            this.Controls.Add(this.pathLabel);
            this.Controls.Add(this.noteLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FileAES_Decrypt";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FileAES: Decrypt";
            this.Load += new System.EventHandler(this.FileAES_Decrypt_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label fileName;
        private System.Windows.Forms.Button decryptButton;
        private System.Windows.Forms.TextBox passwordInput;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.Label pathLabel;
        private System.Windows.Forms.Timer runtime;
        private System.ComponentModel.BackgroundWorker backgroundDecrypt;
        private System.Windows.Forms.Label noteLabel;
        private System.Windows.Forms.Label copyrightLabel;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.Label hintLabel;
        private System.Windows.Forms.TextBox hintTextbox;
        private System.Windows.Forms.ToolTip slowToolTip;
    }
}