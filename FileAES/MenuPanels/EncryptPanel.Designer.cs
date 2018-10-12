namespace FAES_GUI.MenuPanels
{
    partial class encryptPanel
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
            this.selectEncryptButton = new System.Windows.Forms.Button();
            this.passTextbox = new System.Windows.Forms.TextBox();
            this.passLabel = new System.Windows.Forms.Label();
            this.passConfLabel = new System.Windows.Forms.Label();
            this.passConfTextbox = new System.Windows.Forms.TextBox();
            this.passHintLabel = new System.Windows.Forms.Label();
            this.passHintTextbox = new System.Windows.Forms.TextBox();
            this.encryptButton = new System.Windows.Forms.Button();
            this.statusInformation = new System.Windows.Forms.Label();
            this.backgroundEncrypt = new System.ComponentModel.BackgroundWorker();
            this.openFileToEncrypt = new System.Windows.Forms.OpenFileDialog();
            this.fileInfoPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // fileInfoPanel
            // 
            this.fileInfoPanel.BackColor = System.Drawing.Color.Gray;
            this.fileInfoPanel.Controls.Add(this.fileInfoLabel);
            this.fileInfoPanel.Location = new System.Drawing.Point(0, 45);
            this.fileInfoPanel.Name = "fileInfoPanel";
            this.fileInfoPanel.Size = new System.Drawing.Size(414, 54);
            this.fileInfoPanel.TabIndex = 0;
            // 
            // fileInfoLabel
            // 
            this.fileInfoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fileInfoLabel.ForeColor = System.Drawing.Color.White;
            this.fileInfoLabel.Location = new System.Drawing.Point(3, 3);
            this.fileInfoLabel.Name = "fileInfoLabel";
            this.fileInfoLabel.Size = new System.Drawing.Size(408, 50);
            this.fileInfoLabel.TabIndex = 2;
            this.fileInfoLabel.Text = "PLACEHOLDER FILE NAME";
            this.fileInfoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // selectEncryptButton
            // 
            this.selectEncryptButton.AllowDrop = true;
            this.selectEncryptButton.BackColor = System.Drawing.Color.DarkBlue;
            this.selectEncryptButton.FlatAppearance.BorderSize = 0;
            this.selectEncryptButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.selectEncryptButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selectEncryptButton.ForeColor = System.Drawing.Color.White;
            this.selectEncryptButton.Location = new System.Drawing.Point(0, 0);
            this.selectEncryptButton.Name = "selectEncryptButton";
            this.selectEncryptButton.Size = new System.Drawing.Size(414, 46);
            this.selectEncryptButton.TabIndex = 1;
            this.selectEncryptButton.Text = "Select File/Folder";
            this.selectEncryptButton.UseVisualStyleBackColor = false;
            this.selectEncryptButton.Click += new System.EventHandler(this.selectEncryptButton_Click);
            this.selectEncryptButton.DragDrop += new System.Windows.Forms.DragEventHandler(this.selectEncryptButton_DragDrop);
            this.selectEncryptButton.DragEnter += new System.Windows.Forms.DragEventHandler(this.selectEncryptButton_DragEnter);
            // 
            // passTextbox
            // 
            this.passTextbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.passTextbox.Enabled = false;
            this.passTextbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.passTextbox.Location = new System.Drawing.Point(106, 111);
            this.passTextbox.Name = "passTextbox";
            this.passTextbox.PasswordChar = '*';
            this.passTextbox.Size = new System.Drawing.Size(305, 29);
            this.passTextbox.TabIndex = 2;
            this.passTextbox.TextChanged += new System.EventHandler(this.combinedPassword_TextChanged);
            this.passTextbox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.allTextbox_KeyDown);
            // 
            // passLabel
            // 
            this.passLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.passLabel.ForeColor = System.Drawing.Color.White;
            this.passLabel.Location = new System.Drawing.Point(3, 113);
            this.passLabel.Name = "passLabel";
            this.passLabel.Size = new System.Drawing.Size(100, 23);
            this.passLabel.TabIndex = 3;
            this.passLabel.Text = "Password:";
            this.passLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // passConfLabel
            // 
            this.passConfLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.passConfLabel.ForeColor = System.Drawing.Color.White;
            this.passConfLabel.Location = new System.Drawing.Point(3, 163);
            this.passConfLabel.Name = "passConfLabel";
            this.passConfLabel.Size = new System.Drawing.Size(100, 23);
            this.passConfLabel.TabIndex = 8;
            this.passConfLabel.Text = "Confirm:";
            this.passConfLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // passConfTextbox
            // 
            this.passConfTextbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.passConfTextbox.Enabled = false;
            this.passConfTextbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.passConfTextbox.Location = new System.Drawing.Point(106, 161);
            this.passConfTextbox.Name = "passConfTextbox";
            this.passConfTextbox.PasswordChar = '*';
            this.passConfTextbox.Size = new System.Drawing.Size(305, 29);
            this.passConfTextbox.TabIndex = 3;
            this.passConfTextbox.TextChanged += new System.EventHandler(this.combinedPassword_TextChanged);
            this.passConfTextbox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.allTextbox_KeyDown);
            // 
            // passHintLabel
            // 
            this.passHintLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.passHintLabel.ForeColor = System.Drawing.Color.White;
            this.passHintLabel.Location = new System.Drawing.Point(3, 213);
            this.passHintLabel.Name = "passHintLabel";
            this.passHintLabel.Size = new System.Drawing.Size(100, 50);
            this.passHintLabel.TabIndex = 10;
            this.passHintLabel.Text = "Password Hint:";
            this.passHintLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // passHintTextbox
            // 
            this.passHintTextbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.passHintTextbox.Enabled = false;
            this.passHintTextbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.passHintTextbox.Location = new System.Drawing.Point(106, 214);
            this.passHintTextbox.MaxLength = 64;
            this.passHintTextbox.Multiline = true;
            this.passHintTextbox.Name = "passHintTextbox";
            this.passHintTextbox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.passHintTextbox.Size = new System.Drawing.Size(305, 49);
            this.passHintTextbox.TabIndex = 4;
            this.passHintTextbox.TextChanged += new System.EventHandler(this.passHintTextbox_TextChanged);
            this.passHintTextbox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.allTextbox_KeyDown);
            // 
            // encryptButton
            // 
            this.encryptButton.BackColor = System.Drawing.Color.ForestGreen;
            this.encryptButton.Enabled = false;
            this.encryptButton.FlatAppearance.BorderSize = 0;
            this.encryptButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.encryptButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.encryptButton.ForeColor = System.Drawing.Color.White;
            this.encryptButton.Location = new System.Drawing.Point(3, 312);
            this.encryptButton.Name = "encryptButton";
            this.encryptButton.Size = new System.Drawing.Size(408, 42);
            this.encryptButton.TabIndex = 5;
            this.encryptButton.Text = "Encrypt";
            this.encryptButton.UseVisualStyleBackColor = false;
            this.encryptButton.Click += new System.EventHandler(this.encryptButton_Click);
            // 
            // statusInformation
            // 
            this.statusInformation.ForeColor = System.Drawing.Color.White;
            this.statusInformation.Location = new System.Drawing.Point(3, 270);
            this.statusInformation.Name = "statusInformation";
            this.statusInformation.Size = new System.Drawing.Size(408, 39);
            this.statusInformation.TabIndex = 14;
            this.statusInformation.Text = "Error: PLACEHOLDER ERROR";
            this.statusInformation.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // backgroundEncrypt
            // 
            this.backgroundEncrypt.WorkerReportsProgress = true;
            this.backgroundEncrypt.WorkerSupportsCancellation = true;
            this.backgroundEncrypt.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundEncrypt_DoWork);
            this.backgroundEncrypt.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundEncrypt_Complete);
            // 
            // openFileToEncrypt
            // 
            this.openFileToEncrypt.Title = "Select a file to encrypt";
            // 
            // encryptPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.Controls.Add(this.statusInformation);
            this.Controls.Add(this.encryptButton);
            this.Controls.Add(this.passHintTextbox);
            this.Controls.Add(this.passHintLabel);
            this.Controls.Add(this.passConfTextbox);
            this.Controls.Add(this.passConfLabel);
            this.Controls.Add(this.passLabel);
            this.Controls.Add(this.passTextbox);
            this.Controls.Add(this.selectEncryptButton);
            this.Controls.Add(this.fileInfoPanel);
            this.Name = "encryptPanel";
            this.Size = new System.Drawing.Size(414, 357);
            this.fileInfoPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel fileInfoPanel;
        private System.Windows.Forms.Label fileInfoLabel;
        private System.Windows.Forms.Button selectEncryptButton;
        private System.Windows.Forms.TextBox passTextbox;
        private System.Windows.Forms.Label passLabel;
        private System.Windows.Forms.Label passConfLabel;
        private System.Windows.Forms.TextBox passConfTextbox;
        private System.Windows.Forms.Label passHintLabel;
        private System.Windows.Forms.TextBox passHintTextbox;
        private System.Windows.Forms.Button encryptButton;
        private System.Windows.Forms.Label statusInformation;
        private System.ComponentModel.BackgroundWorker backgroundEncrypt;
        private System.Windows.Forms.OpenFileDialog openFileToEncrypt;
    }
}
