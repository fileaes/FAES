namespace FileAES
{
    partial class FileAES_Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileAES_Main));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.encryptFileDrop = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.decryptFileDrop = new System.Windows.Forms.Label();
            this.openFileToEncrypt = new System.Windows.Forms.OpenFileDialog();
            this.openFileToDecrypt = new System.Windows.Forms.OpenFileDialog();
            this.versionLabel = new System.Windows.Forms.Label();
            this.copyrightLabel = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.encryptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.decryptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.utilitiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearTempToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.encryptFileDrop);
            this.groupBox1.Location = new System.Drawing.Point(12, 22);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(368, 102);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // encryptFileDrop
            // 
            this.encryptFileDrop.AllowDrop = true;
            this.encryptFileDrop.BackColor = System.Drawing.Color.Transparent;
            this.encryptFileDrop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.encryptFileDrop.Location = new System.Drawing.Point(0, 9);
            this.encryptFileDrop.Name = "encryptFileDrop";
            this.encryptFileDrop.Size = new System.Drawing.Size(368, 90);
            this.encryptFileDrop.TabIndex = 0;
            this.encryptFileDrop.Text = "Encrypt";
            this.encryptFileDrop.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.encryptFileDrop.Click += new System.EventHandler(this.encryptFileDrop_Click);
            this.encryptFileDrop.DragDrop += new System.Windows.Forms.DragEventHandler(this.encryptFileDrop_DragDrop);
            this.encryptFileDrop.DragEnter += new System.Windows.Forms.DragEventHandler(this.encryptFileDrop_DragEnter);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.decryptFileDrop);
            this.groupBox2.Location = new System.Drawing.Point(12, 130);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(368, 102);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            // 
            // decryptFileDrop
            // 
            this.decryptFileDrop.AllowDrop = true;
            this.decryptFileDrop.BackColor = System.Drawing.Color.Transparent;
            this.decryptFileDrop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.decryptFileDrop.Location = new System.Drawing.Point(0, 9);
            this.decryptFileDrop.Name = "decryptFileDrop";
            this.decryptFileDrop.Size = new System.Drawing.Size(368, 90);
            this.decryptFileDrop.TabIndex = 0;
            this.decryptFileDrop.Text = "Decrypt";
            this.decryptFileDrop.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.decryptFileDrop.Click += new System.EventHandler(this.decryptFileDrop_Click);
            this.decryptFileDrop.DragDrop += new System.Windows.Forms.DragEventHandler(this.decryptFileDrop_DragDrop);
            this.decryptFileDrop.DragEnter += new System.Windows.Forms.DragEventHandler(this.decryptFileDrop_DragEnter);
            // 
            // openFileToEncrypt
            // 
            this.openFileToEncrypt.Title = "Select a file to encrypt";
            // 
            // openFileToDecrypt
            // 
            this.openFileToDecrypt.Filter = "FileAES Files|*.faes;*.mcrypt";
            this.openFileToDecrypt.Title = "Select a file to decrypt";
            // 
            // versionLabel
            // 
            this.versionLabel.Location = new System.Drawing.Point(0, 235);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(158, 13);
            this.versionLabel.TabIndex = 20;
            this.versionLabel.Text = "v0.0.0.0";
            this.versionLabel.Click += new System.EventHandler(this.versionLabel_Click);
            // 
            // copyrightLabel
            // 
            this.copyrightLabel.AutoSize = true;
            this.copyrightLabel.Location = new System.Drawing.Point(305, 235);
            this.copyrightLabel.Name = "copyrightLabel";
            this.copyrightLabel.Size = new System.Drawing.Size(88, 13);
            this.copyrightLabel.TabIndex = 19;
            this.copyrightLabel.Text = "mullak99 © 2018";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.utilitiesToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(392, 24);
            this.menuStrip1.TabIndex = 21;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.encryptToolStripMenuItem,
            this.decryptToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // encryptToolStripMenuItem
            // 
            this.encryptToolStripMenuItem.Name = "encryptToolStripMenuItem";
            this.encryptToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.encryptToolStripMenuItem.Text = "Encrypt";
            this.encryptToolStripMenuItem.Click += new System.EventHandler(this.encryptFileDrop_Click);
            // 
            // decryptToolStripMenuItem
            // 
            this.decryptToolStripMenuItem.Name = "decryptToolStripMenuItem";
            this.decryptToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.decryptToolStripMenuItem.Text = "Decrypt";
            this.decryptToolStripMenuItem.Click += new System.EventHandler(this.decryptFileDrop_Click);
            // 
            // utilitiesToolStripMenuItem
            // 
            this.utilitiesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearTempToolStripMenuItem});
            this.utilitiesToolStripMenuItem.Name = "utilitiesToolStripMenuItem";
            this.utilitiesToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
            this.utilitiesToolStripMenuItem.Text = "Utilities";
            // 
            // clearTempToolStripMenuItem
            // 
            this.clearTempToolStripMenuItem.Name = "clearTempToolStripMenuItem";
            this.clearTempToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.clearTempToolStripMenuItem.Text = "Clear Temp";
            this.clearTempToolStripMenuItem.Click += new System.EventHandler(this.clearTempToolStripMenuItem_Click);
            // 
            // FileAES_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 249);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.versionLabel);
            this.Controls.Add(this.copyrightLabel);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "FileAES_Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FileAES";
            this.Load += new System.EventHandler(this.FileAES_Main_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label encryptFileDrop;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label decryptFileDrop;
        private System.Windows.Forms.OpenFileDialog openFileToEncrypt;
        private System.Windows.Forms.OpenFileDialog openFileToDecrypt;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.Label copyrightLabel;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem encryptToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem decryptToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem utilitiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearTempToolStripMenuItem;
    }
}