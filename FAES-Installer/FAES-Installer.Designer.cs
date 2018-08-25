namespace FAESInstaller
{
    partial class FAESInstaller
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FAESInstaller));
            this.label1 = new System.Windows.Forms.Label();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.installDir = new System.Windows.Forms.TextBox();
            this.browseInstallDir = new System.Windows.Forms.Button();
            this.installButton = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.passAccept = new System.Windows.Forms.RadioButton();
            this.failAccept = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.branchComboBox = new System.Windows.Forms.ComboBox();
            this.versionInstalling = new System.Windows.Forms.Label();
            this.noteLabel = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            this.titleLabel = new System.Windows.Forms.Label();
            this.versionLabel = new System.Windows.Forms.Label();
            this.copyrightLabel = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Install Location:";
            // 
            // folderBrowserDialog
            // 
            this.folderBrowserDialog.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // installDir
            // 
            this.installDir.Location = new System.Drawing.Point(86, 22);
            this.installDir.Name = "installDir";
            this.installDir.Size = new System.Drawing.Size(318, 20);
            this.installDir.TabIndex = 1;
            // 
            // browseInstallDir
            // 
            this.browseInstallDir.Location = new System.Drawing.Point(410, 20);
            this.browseInstallDir.Name = "browseInstallDir";
            this.browseInstallDir.Size = new System.Drawing.Size(64, 23);
            this.browseInstallDir.TabIndex = 2;
            this.browseInstallDir.Text = "Browse...";
            this.browseInstallDir.UseVisualStyleBackColor = true;
            this.browseInstallDir.Click += new System.EventHandler(this.browseInstallDir_Click);
            // 
            // installButton
            // 
            this.installButton.Enabled = false;
            this.installButton.Location = new System.Drawing.Point(303, 99);
            this.installButton.Name = "installButton";
            this.installButton.Size = new System.Drawing.Size(75, 23);
            this.installButton.TabIndex = 3;
            this.installButton.Text = "Install";
            this.installButton.UseVisualStyleBackColor = true;
            this.installButton.Click += new System.EventHandler(this.installButton_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 97);
            this.textBox1.MaxLength = 0;
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(469, 141);
            this.textBox1.TabIndex = 5;
            this.textBox1.Text = resources.GetString("textBox1.Text");
            // 
            // passAccept
            // 
            this.passAccept.AutoSize = true;
            this.passAccept.Location = new System.Drawing.Point(13, 244);
            this.passAccept.Name = "passAccept";
            this.passAccept.Size = new System.Drawing.Size(110, 17);
            this.passAccept.TabIndex = 6;
            this.passAccept.Text = "I accept the terms";
            this.passAccept.UseVisualStyleBackColor = true;
            this.passAccept.CheckedChanged += new System.EventHandler(this.passAccept_CheckedChanged);
            // 
            // failAccept
            // 
            this.failAccept.AutoSize = true;
            this.failAccept.Checked = true;
            this.failAccept.Location = new System.Drawing.Point(13, 267);
            this.failAccept.Name = "failAccept";
            this.failAccept.Size = new System.Drawing.Size(143, 17);
            this.failAccept.TabIndex = 7;
            this.failAccept.TabStop = true;
            this.failAccept.Text = "I do not accept the terms";
            this.failAccept.UseVisualStyleBackColor = true;
            this.failAccept.CheckedChanged += new System.EventHandler(this.failAccept_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.branchComboBox);
            this.groupBox1.Controls.Add(this.versionInstalling);
            this.groupBox1.Controls.Add(this.noteLabel);
            this.groupBox1.Controls.Add(this.cancelButton);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.installDir);
            this.groupBox1.Controls.Add(this.browseInstallDir);
            this.groupBox1.Controls.Add(this.installButton);
            this.groupBox1.Location = new System.Drawing.Point(7, 290);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(478, 131);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Install Branch:";
            // 
            // branchComboBox
            // 
            this.branchComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.branchComboBox.FormattingEnabled = true;
            this.branchComboBox.Items.AddRange(new object[] {
            "Stable",
            "Development"});
            this.branchComboBox.Location = new System.Drawing.Point(86, 48);
            this.branchComboBox.Name = "branchComboBox";
            this.branchComboBox.Size = new System.Drawing.Size(131, 21);
            this.branchComboBox.TabIndex = 14;
            this.branchComboBox.SelectedIndexChanged += new System.EventHandler(this.branchComboBox_SelectedIndexChanged);
            // 
            // versionInstalling
            // 
            this.versionInstalling.Location = new System.Drawing.Point(303, 63);
            this.versionInstalling.Name = "versionInstalling";
            this.versionInstalling.Size = new System.Drawing.Size(169, 30);
            this.versionInstalling.TabIndex = 13;
            this.versionInstalling.Text = "Installing Version: v0.0.0.0";
            // 
            // noteLabel
            // 
            this.noteLabel.BackColor = System.Drawing.SystemColors.Control;
            this.noteLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.noteLabel.ForeColor = System.Drawing.Color.DarkRed;
            this.noteLabel.Location = new System.Drawing.Point(3, 75);
            this.noteLabel.Name = "noteLabel";
            this.noteLabel.Size = new System.Drawing.Size(283, 47);
            this.noteLabel.TabIndex = 5;
            this.noteLabel.Text = "Note: This program is in BETA, not all features are implemented yet.\r\nSupport for" +
    " full installs is still in early stages for FileAES.";
            this.noteLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(397, 99);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.Location = new System.Drawing.Point(97, 13);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(174, 26);
            this.titleLabel.TabIndex = 10;
            this.titleLabel.Text = "FileAES Installer";
            // 
            // versionLabel
            // 
            this.versionLabel.AutoSize = true;
            this.versionLabel.Location = new System.Drawing.Point(99, 39);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(46, 13);
            this.versionLabel.TabIndex = 11;
            this.versionLabel.Text = "v0.0.0.0";
            // 
            // copyrightLabel
            // 
            this.copyrightLabel.AutoSize = true;
            this.copyrightLabel.Location = new System.Drawing.Point(99, 66);
            this.copyrightLabel.Name = "copyrightLabel";
            this.copyrightLabel.Size = new System.Drawing.Size(141, 13);
            this.copyrightLabel.TabIndex = 12;
            this.copyrightLabel.Text = "Copyright © mullak99 - 2018";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::FAESInstaller.Properties.Resources.Icon;
            this.pictureBox1.Location = new System.Drawing.Point(7, 13);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(84, 78);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // FAESInstaller
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(493, 428);
            this.Controls.Add(this.copyrightLabel);
            this.Controls.Add(this.versionLabel);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.failAccept);
            this.Controls.Add(this.passAccept);
            this.Controls.Add(this.textBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FAESInstaller";
            this.Text = "FileAES: Installer";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.TextBox installDir;
        private System.Windows.Forms.Button browseInstallDir;
        private System.Windows.Forms.Button installButton;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.RadioButton passAccept;
        private System.Windows.Forms.RadioButton failAccept;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.Label copyrightLabel;
        private System.Windows.Forms.Label versionInstalling;
        private System.Windows.Forms.Label noteLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox branchComboBox;
    }
}

