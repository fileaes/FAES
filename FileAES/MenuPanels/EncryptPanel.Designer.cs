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
            this.label1 = new System.Windows.Forms.Label();
            this.compressMode = new System.Windows.Forms.ComboBox();
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
            this.passTextbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.passLabel.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.passLabel.Location = new System.Drawing.Point(3, 97);
            this.passLabel.Name = "passLabel";
            this.passLabel.Size = new System.Drawing.Size(128, 23);
            this.passLabel.TabIndex = 3;
            this.passLabel.Text = "Password:";
            this.passLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // passConfLabel
            // 
            this.passConfLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.passConfLabel.ForeColor = System.Drawing.Color.White;
            this.passConfLabel.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.passConfLabel.Location = new System.Drawing.Point(3, 138);
            this.passConfLabel.Name = "passConfLabel";
            this.passConfLabel.Size = new System.Drawing.Size(128, 23);
            this.passConfLabel.TabIndex = 8;
            this.passConfLabel.Text = "Conf. Pass:";
            this.passConfLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // passConfTextbox
            // 
            this.passConfTextbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.passConfTextbox.Enabled = false;
            this.passConfTextbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.passConfTextbox.Location = new System.Drawing.Point(137, 136);
            this.passConfTextbox.Name = "passConfTextbox";
            this.passConfTextbox.PasswordChar = '*';
            this.passConfTextbox.Size = new System.Drawing.Size(266, 27);
            this.passConfTextbox.TabIndex = 3;
            this.passConfTextbox.TextChanged += new System.EventHandler(this.combinedPassword_TextChanged);
            this.passConfTextbox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.allTextbox_KeyDown);
            // 
            // passHintLabel
            // 
            this.passHintLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.passHintLabel.ForeColor = System.Drawing.Color.White;
            this.passHintLabel.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.passHintLabel.Location = new System.Drawing.Point(3, 176);
            this.passHintLabel.Name = "passHintLabel";
            this.passHintLabel.Size = new System.Drawing.Size(128, 50);
            this.passHintLabel.TabIndex = 10;
            this.passHintLabel.Text = "Password Hint:";
            this.passHintLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // passHintTextbox
            // 
            this.passHintTextbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.passHintTextbox.Enabled = false;
            this.passHintTextbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.passHintTextbox.Location = new System.Drawing.Point(137, 174);
            this.passHintTextbox.MaxLength = 64;
            this.passHintTextbox.Multiline = true;
            this.passHintTextbox.Name = "passHintTextbox";
            this.passHintTextbox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.passHintTextbox.Size = new System.Drawing.Size(266, 49);
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
            this.statusInformation.Location = new System.Drawing.Point(3, 274);
            this.statusInformation.Name = "statusInformation";
            this.statusInformation.Size = new System.Drawing.Size(408, 35);
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
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label1.Location = new System.Drawing.Point(7, 241);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 29);
            this.label1.TabIndex = 15;
            this.label1.Text = "Compression:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // compressMode
            // 
            this.compressMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.compressMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.compressMode.FormattingEnabled = true;
            this.compressMode.Location = new System.Drawing.Point(137, 238);
            this.compressMode.Name = "compressMode";
            this.compressMode.Size = new System.Drawing.Size(266, 28);
            this.compressMode.TabIndex = 16;
            // 
            // encryptPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.Controls.Add(this.compressMode);
            this.Controls.Add(this.label1);
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox compressMode;
    }
}
