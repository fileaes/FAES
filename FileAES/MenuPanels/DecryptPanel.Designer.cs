namespace FAES_GUI.MenuPanels
{
    partial class decryptPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.fileInfoPanel = new System.Windows.Forms.Panel();
            this.fileInfoLabel = new System.Windows.Forms.Label();
            this.selectDecryptButton = new System.Windows.Forms.Button();
            this.passTextbox = new System.Windows.Forms.TextBox();
            this.passLabel = new System.Windows.Forms.Label();
            this.passHintLabel = new System.Windows.Forms.Label();
            this.passHintTextbox = new System.Windows.Forms.TextBox();
            this.decryptButton = new System.Windows.Forms.Button();
            this.encryptedFileMetaData = new System.Windows.Forms.Label();
            this.statusInformation = new System.Windows.Forms.Label();
            this.backgroundDecrypt = new System.ComponentModel.BackgroundWorker();
            this.openFileToDecrypt = new System.Windows.Forms.OpenFileDialog();
            this.fileInfoPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // fileInfoPanel
            // 
            this.fileInfoPanel.BackColor = System.Drawing.Color.Gray;
            this.fileInfoPanel.Controls.Add(this.fileInfoLabel);
            this.fileInfoPanel.Location = new System.Drawing.Point(0, 45);
            this.fileInfoPanel.Name = "fileInfoPanel";
            this.fileInfoPanel.Size = new System.Drawing.Size(414, 34);
            this.fileInfoPanel.TabIndex = 0;
            // 
            // fileInfoLabel
            // 
            this.fileInfoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fileInfoLabel.ForeColor = System.Drawing.Color.White;
            this.fileInfoLabel.Location = new System.Drawing.Point(3, 3);
            this.fileInfoLabel.Name = "fileInfoLabel";
            this.fileInfoLabel.Size = new System.Drawing.Size(408, 30);
            this.fileInfoLabel.TabIndex = 2;
            this.fileInfoLabel.Text = "PLACEHOLDER FILE NAME";
            this.fileInfoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // selectDecryptButton
            // 
            this.selectDecryptButton.AllowDrop = true;
            this.selectDecryptButton.BackColor = System.Drawing.Color.DarkBlue;
            this.selectDecryptButton.FlatAppearance.BorderSize = 0;
            this.selectDecryptButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.selectDecryptButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selectDecryptButton.ForeColor = System.Drawing.Color.White;
            this.selectDecryptButton.Location = new System.Drawing.Point(0, 0);
            this.selectDecryptButton.Name = "selectDecryptButton";
            this.selectDecryptButton.Size = new System.Drawing.Size(414, 46);
            this.selectDecryptButton.TabIndex = 1;
            this.selectDecryptButton.Text = "Select File/Folder";
            this.selectDecryptButton.UseVisualStyleBackColor = false;
            this.selectDecryptButton.Click += new System.EventHandler(this.selectDecryptButton_Click);
            this.selectDecryptButton.DragDrop += new System.Windows.Forms.DragEventHandler(this.selectDecryptButton_DragDrop);
            this.selectDecryptButton.DragEnter += new System.Windows.Forms.DragEventHandler(this.selectDecryptButton_DragEnter);
            // 
            // passTextbox
            // 
            this.passTextbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.passTextbox.Enabled = false;
            this.passTextbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.passTextbox.Location = new System.Drawing.Point(137, 95);
            this.passTextbox.Name = "passTextbox";
            this.passTextbox.PasswordChar = '*';
            this.passTextbox.Size = new System.Drawing.Size(266, 27);
            this.passTextbox.TabIndex = 2;
            this.passTextbox.TextChanged += new System.EventHandler(this.combinedPassword_TextChanged);
            this.passTextbox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.allTextbox_KeyDown);
            // 
            // passLabel
            // 
            this.passLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.passLabel.ForeColor = System.Drawing.Color.White;
            this.passLabel.Location = new System.Drawing.Point(3, 97);
            this.passLabel.Name = "passLabel";
            this.passLabel.Size = new System.Drawing.Size(128, 23);
            this.passLabel.TabIndex = 3;
            this.passLabel.Text = "Password:";
            this.passLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // passHintLabel
            // 
            this.passHintLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.passHintLabel.ForeColor = System.Drawing.Color.White;
            this.passHintLabel.Location = new System.Drawing.Point(7, 135);
            this.passHintLabel.Name = "passHintLabel";
            this.passHintLabel.Size = new System.Drawing.Size(124, 50);
            this.passHintLabel.TabIndex = 10;
            this.passHintLabel.Text = "Password Hint:";
            this.passHintLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // passHintTextbox
            // 
            this.passHintTextbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.passHintTextbox.Enabled = false;
            this.passHintTextbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.passHintTextbox.Location = new System.Drawing.Point(137, 136);
            this.passHintTextbox.MaxLength = 64;
            this.passHintTextbox.Multiline = true;
            this.passHintTextbox.Name = "passHintTextbox";
            this.passHintTextbox.ReadOnly = true;
            this.passHintTextbox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.passHintTextbox.Size = new System.Drawing.Size(266, 49);
            this.passHintTextbox.TabIndex = 3;
            // 
            // decryptButton
            // 
            this.decryptButton.BackColor = System.Drawing.Color.DarkRed;
            this.decryptButton.Enabled = false;
            this.decryptButton.FlatAppearance.BorderSize = 0;
            this.decryptButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.decryptButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.decryptButton.ForeColor = System.Drawing.Color.White;
            this.decryptButton.Location = new System.Drawing.Point(3, 312);
            this.decryptButton.Name = "decryptButton";
            this.decryptButton.Size = new System.Drawing.Size(408, 42);
            this.decryptButton.TabIndex = 4;
            this.decryptButton.Text = "Decrypt";
            this.decryptButton.UseVisualStyleBackColor = false;
            this.decryptButton.Click += new System.EventHandler(this.decryptButton_Click);
            // 
            // encryptedFileMetaData
            // 
            this.encryptedFileMetaData.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.encryptedFileMetaData.ForeColor = System.Drawing.Color.White;
            this.encryptedFileMetaData.Location = new System.Drawing.Point(3, 197);
            this.encryptedFileMetaData.Name = "encryptedFileMetaData";
            this.encryptedFileMetaData.Size = new System.Drawing.Size(408, 73);
            this.encryptedFileMetaData.TabIndex = 12;
            this.encryptedFileMetaData.Text = "PLACEHOLDER METADATA";
            this.encryptedFileMetaData.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // statusInformation
            // 
            this.statusInformation.ForeColor = System.Drawing.Color.White;
            this.statusInformation.Location = new System.Drawing.Point(3, 274);
            this.statusInformation.Name = "statusInformation";
            this.statusInformation.Size = new System.Drawing.Size(408, 35);
            this.statusInformation.TabIndex = 13;
            this.statusInformation.Text = "Error: PLACEHOLDER ERROR";
            this.statusInformation.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // backgroundDecrypt
            // 
            this.backgroundDecrypt.WorkerReportsProgress = true;
            this.backgroundDecrypt.WorkerSupportsCancellation = true;
            this.backgroundDecrypt.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundDecrypt_DoWork);
            this.backgroundDecrypt.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundDecrypt_Complete);
            // 
            // openFileToDecrypt
            // 
            this.openFileToDecrypt.Filter = "FileAES Files|*.faes;*.mcrypt";
            this.openFileToDecrypt.Title = "Select a file to decrypt";
            // 
            // decryptPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.Controls.Add(this.statusInformation);
            this.Controls.Add(this.encryptedFileMetaData);
            this.Controls.Add(this.decryptButton);
            this.Controls.Add(this.passHintTextbox);
            this.Controls.Add(this.passHintLabel);
            this.Controls.Add(this.passLabel);
            this.Controls.Add(this.passTextbox);
            this.Controls.Add(this.selectDecryptButton);
            this.Controls.Add(this.fileInfoPanel);
            this.Name = "decryptPanel";
            this.Size = new System.Drawing.Size(414, 357);
            this.fileInfoPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel fileInfoPanel;
        private System.Windows.Forms.Label fileInfoLabel;
        private System.Windows.Forms.Button selectDecryptButton;
        private System.Windows.Forms.TextBox passTextbox;
        private System.Windows.Forms.Label passLabel;
        private System.Windows.Forms.Label passHintLabel;
        private System.Windows.Forms.TextBox passHintTextbox;
        private System.Windows.Forms.Button decryptButton;
        private System.Windows.Forms.Label encryptedFileMetaData;
        private System.Windows.Forms.Label statusInformation;
        private System.ComponentModel.BackgroundWorker backgroundDecrypt;
        private System.Windows.Forms.OpenFileDialog openFileToDecrypt;
    }
}
