namespace FileAES
{
    partial class FileAES_Update
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileAES_Update));
            this.descLabel = new System.Windows.Forms.Label();
            this.currentVerLabel = new System.Windows.Forms.Label();
            this.latestVerLabel = new System.Windows.Forms.Label();
            this.updateButton = new System.Windows.Forms.Button();
            this.neverRemindButton = new System.Windows.Forms.Button();
            this.copyrightLabel = new System.Windows.Forms.Label();
            this.checkForUpdateButton = new System.Windows.Forms.Button();
            this.FaesVersion = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // descLabel
            // 
            this.descLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.descLabel.Location = new System.Drawing.Point(12, 9);
            this.descLabel.Name = "descLabel";
            this.descLabel.Size = new System.Drawing.Size(246, 49);
            this.descLabel.TabIndex = 0;
            this.descLabel.Text = "An update is available!";
            this.descLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // currentVerLabel
            // 
            this.currentVerLabel.Location = new System.Drawing.Point(13, 46);
            this.currentVerLabel.Name = "currentVerLabel";
            this.currentVerLabel.Size = new System.Drawing.Size(246, 15);
            this.currentVerLabel.TabIndex = 1;
            this.currentVerLabel.Text = "Current Version: v0.0.0.0";
            // 
            // latestVerLabel
            // 
            this.latestVerLabel.Location = new System.Drawing.Point(18, 66);
            this.latestVerLabel.Name = "latestVerLabel";
            this.latestVerLabel.Size = new System.Drawing.Size(240, 13);
            this.latestVerLabel.TabIndex = 2;
            this.latestVerLabel.Text = "Latest Version: v0.0.0.0";
            // 
            // updateButton
            // 
            this.updateButton.BackColor = System.Drawing.Color.Lime;
            this.updateButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.updateButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.updateButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.updateButton.Location = new System.Drawing.Point(12, 107);
            this.updateButton.Name = "updateButton";
            this.updateButton.Size = new System.Drawing.Size(119, 40);
            this.updateButton.TabIndex = 3;
            this.updateButton.Text = "Update Now";
            this.updateButton.UseVisualStyleBackColor = false;
            this.updateButton.Visible = false;
            this.updateButton.Click += new System.EventHandler(this.updateButton_Click);
            // 
            // neverRemindButton
            // 
            this.neverRemindButton.BackColor = System.Drawing.Color.Red;
            this.neverRemindButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.neverRemindButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.neverRemindButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.neverRemindButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.neverRemindButton.Location = new System.Drawing.Point(140, 107);
            this.neverRemindButton.Name = "neverRemindButton";
            this.neverRemindButton.Size = new System.Drawing.Size(119, 40);
            this.neverRemindButton.TabIndex = 5;
            this.neverRemindButton.Text = "Never Remind Me";
            this.neverRemindButton.UseVisualStyleBackColor = false;
            this.neverRemindButton.Visible = false;
            this.neverRemindButton.Click += new System.EventHandler(this.neverRemindButton_Click);
            // 
            // copyrightLabel
            // 
            this.copyrightLabel.AutoSize = true;
            this.copyrightLabel.Location = new System.Drawing.Point(184, 196);
            this.copyrightLabel.Name = "copyrightLabel";
            this.copyrightLabel.Size = new System.Drawing.Size(88, 13);
            this.copyrightLabel.TabIndex = 20;
            this.copyrightLabel.Text = "mullak99 © 2018";
            // 
            // checkForUpdateButton
            // 
            this.checkForUpdateButton.BackColor = System.Drawing.Color.Cyan;
            this.checkForUpdateButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.checkForUpdateButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkForUpdateButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkForUpdateButton.Location = new System.Drawing.Point(12, 153);
            this.checkForUpdateButton.Name = "checkForUpdateButton";
            this.checkForUpdateButton.Size = new System.Drawing.Size(247, 40);
            this.checkForUpdateButton.TabIndex = 21;
            this.checkForUpdateButton.Text = "Check for Update";
            this.checkForUpdateButton.UseVisualStyleBackColor = false;
            this.checkForUpdateButton.Click += new System.EventHandler(this.checkForUpdateButton_Click);
            // 
            // FaesVersion
            // 
            this.FaesVersion.Location = new System.Drawing.Point(20, 86);
            this.FaesVersion.Name = "FaesVersion";
            this.FaesVersion.Size = new System.Drawing.Size(238, 13);
            this.FaesVersion.TabIndex = 22;
            this.FaesVersion.Text = "FAES Version: v0.0.0.0";
            // 
            // FileAES_Update
            // 
            this.AcceptButton = this.updateButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.neverRemindButton;
            this.ClientSize = new System.Drawing.Size(271, 210);
            this.Controls.Add(this.FaesVersion);
            this.Controls.Add(this.checkForUpdateButton);
            this.Controls.Add(this.copyrightLabel);
            this.Controls.Add(this.neverRemindButton);
            this.Controls.Add(this.updateButton);
            this.Controls.Add(this.latestVerLabel);
            this.Controls.Add(this.currentVerLabel);
            this.Controls.Add(this.descLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FileAES_Update";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FileAES: Update";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label descLabel;
        private System.Windows.Forms.Label currentVerLabel;
        private System.Windows.Forms.Label latestVerLabel;
        private System.Windows.Forms.Button updateButton;
        private System.Windows.Forms.Button neverRemindButton;
        private System.Windows.Forms.Label copyrightLabel;
        private System.Windows.Forms.Button checkForUpdateButton;
        private System.Windows.Forms.Label FaesVersion;
    }
}