namespace FileAES
{
    partial class WarnPlaceholder
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WarnPlaceholder));
            this.placeholderWarningLabel = new System.Windows.Forms.Label();
            this.closeButton = new System.Windows.Forms.Button();
            this.versionLabel = new System.Windows.Forms.Label();
            this.copyrightLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // placeholderWarningLabel
            // 
            this.placeholderWarningLabel.Location = new System.Drawing.Point(13, 13);
            this.placeholderWarningLabel.Name = "placeholderWarningLabel";
            this.placeholderWarningLabel.Size = new System.Drawing.Size(259, 127);
            this.placeholderWarningLabel.TabIndex = 0;
            this.placeholderWarningLabel.Text = resources.GetString("placeholderWarningLabel.Text");
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(16, 131);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(256, 23);
            this.closeButton.TabIndex = 1;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // versionLabel
            // 
            this.versionLabel.Location = new System.Drawing.Point(0, 163);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(158, 13);
            this.versionLabel.TabIndex = 20;
            this.versionLabel.Text = "v0.0.0.0";
            this.versionLabel.Click += new System.EventHandler(this.versionLabel_Click);
            // 
            // copyrightLabel
            // 
            this.copyrightLabel.AutoSize = true;
            this.copyrightLabel.Location = new System.Drawing.Point(197, 163);
            this.copyrightLabel.Name = "copyrightLabel";
            this.copyrightLabel.Size = new System.Drawing.Size(88, 13);
            this.copyrightLabel.TabIndex = 19;
            this.copyrightLabel.Text = "mullak99 © 2018";
            // 
            // WarnPlaceholder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 178);
            this.Controls.Add(this.versionLabel);
            this.Controls.Add(this.copyrightLabel);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.placeholderWarningLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "WarnPlaceholder";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FileAES: Dev";
            this.Load += new System.EventHandler(this.WarnPlaceholder_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label placeholderWarningLabel;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.Label copyrightLabel;
    }
}