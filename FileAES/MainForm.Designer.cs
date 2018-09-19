namespace FAES_GUI
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.titleBar = new System.Windows.Forms.Panel();
            this.titleBarLogo = new System.Windows.Forms.PictureBox();
            this.titleLabel = new System.Windows.Forms.Label();
            this.quitButton = new System.Windows.Forms.Button();
            this.sidePanel = new System.Windows.Forms.Panel();
            this.copyrightLabel = new System.Windows.Forms.Label();
            this.slowToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.settingsMenuButton = new FAES_GUI.CustomControls.SubMenuButton();
            this.decryptMenuButton = new FAES_GUI.CustomControls.SubMenuButton();
            this.encryptMenuButton = new FAES_GUI.CustomControls.SubMenuButton();
            this.autoSelectMenuButton = new FAES_GUI.CustomControls.SubMenuButton();
            this.titleBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.titleBarLogo)).BeginInit();
            this.sidePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // titleBar
            // 
            this.titleBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.titleBar.Controls.Add(this.titleBarLogo);
            this.titleBar.Controls.Add(this.titleLabel);
            this.titleBar.Controls.Add(this.quitButton);
            this.titleBar.Location = new System.Drawing.Point(0, 0);
            this.titleBar.Name = "titleBar";
            this.titleBar.Size = new System.Drawing.Size(645, 25);
            this.titleBar.TabIndex = 0;
            this.titleBar.Paint += new System.Windows.Forms.PaintEventHandler(this.titleBar_Paint);
            this.titleBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.titleBar_MouseDown);
            // 
            // titleBarLogo
            // 
            this.titleBarLogo.Image = global::FAES_GUI.Properties.Resources.Icon;
            this.titleBarLogo.InitialImage = global::FAES_GUI.Properties.Resources.Icon;
            this.titleBarLogo.Location = new System.Drawing.Point(4, 3);
            this.titleBarLogo.Name = "titleBarLogo";
            this.titleBarLogo.Size = new System.Drawing.Size(20, 20);
            this.titleBarLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.titleBarLogo.TabIndex = 2;
            this.titleBarLogo.TabStop = false;
            this.titleBarLogo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.titleBar_MouseDown);
            // 
            // titleLabel
            // 
            this.titleLabel.BackColor = System.Drawing.Color.Transparent;
            this.titleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.ForeColor = System.Drawing.SystemColors.Control;
            this.titleLabel.Location = new System.Drawing.Point(26, 1);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(216, 25);
            this.titleLabel.TabIndex = 1;
            this.titleLabel.Text = "FileAES 2.0.0";
            this.titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.titleLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.titleBar_MouseDown);
            // 
            // quitButton
            // 
            this.quitButton.BackColor = System.Drawing.Color.Transparent;
            this.quitButton.FlatAppearance.BorderSize = 0;
            this.quitButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.quitButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.quitButton.ForeColor = System.Drawing.Color.LightGray;
            this.quitButton.Location = new System.Drawing.Point(621, 1);
            this.quitButton.Name = "quitButton";
            this.quitButton.Size = new System.Drawing.Size(23, 23);
            this.quitButton.TabIndex = 1;
            this.quitButton.Text = "✖";
            this.quitButton.UseVisualStyleBackColor = false;
            this.quitButton.Click += new System.EventHandler(this.quitButton_Click);
            this.quitButton.MouseEnter += new System.EventHandler(this.quitButton_MouseEnter);
            this.quitButton.MouseLeave += new System.EventHandler(this.quitButton_MouseLeave);
            this.quitButton.MouseHover += new System.EventHandler(this.quitButton_MouseHover);
            // 
            // sidePanel
            // 
            this.sidePanel.BackColor = System.Drawing.Color.Gray;
            this.sidePanel.Controls.Add(this.settingsMenuButton);
            this.sidePanel.Controls.Add(this.decryptMenuButton);
            this.sidePanel.Controls.Add(this.encryptMenuButton);
            this.sidePanel.Controls.Add(this.autoSelectMenuButton);
            this.sidePanel.Controls.Add(this.copyrightLabel);
            this.sidePanel.Location = new System.Drawing.Point(0, 24);
            this.sidePanel.Name = "sidePanel";
            this.sidePanel.Size = new System.Drawing.Size(149, 382);
            this.sidePanel.TabIndex = 1;
            this.sidePanel.Paint += new System.Windows.Forms.PaintEventHandler(this.sidePanel_Paint);
            // 
            // copyrightLabel
            // 
            this.copyrightLabel.BackColor = System.Drawing.Color.Transparent;
            this.copyrightLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.copyrightLabel.ForeColor = System.Drawing.SystemColors.Control;
            this.copyrightLabel.Location = new System.Drawing.Point(3, 341);
            this.copyrightLabel.Name = "copyrightLabel";
            this.copyrightLabel.Size = new System.Drawing.Size(143, 37);
            this.copyrightLabel.TabIndex = 2;
            this.copyrightLabel.Text = "© - 2018 | mullak99";
            this.copyrightLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // slowToolTip
            // 
            this.slowToolTip.AutoPopDelay = 5000;
            this.slowToolTip.InitialDelay = 1000;
            this.slowToolTip.ReshowDelay = 100;
            // 
            // settingsMenuButton
            // 
            this.settingsMenuButton.BackColor = System.Drawing.Color.Transparent;
            this.settingsMenuButton.Location = new System.Drawing.Point(1, 231);
            this.settingsMenuButton.Name = "settingsMenuButton";
            this.settingsMenuButton.Selected = false;
            this.settingsMenuButton.Size = new System.Drawing.Size(147, 66);
            this.settingsMenuButton.TabIndex = 6;
            this.settingsMenuButton.Text = "Settings";
            // 
            // decryptMenuButton
            // 
            this.decryptMenuButton.BackColor = System.Drawing.Color.Transparent;
            this.decryptMenuButton.Location = new System.Drawing.Point(1, 165);
            this.decryptMenuButton.Name = "decryptMenuButton";
            this.decryptMenuButton.Selected = false;
            this.decryptMenuButton.Size = new System.Drawing.Size(147, 66);
            this.decryptMenuButton.TabIndex = 5;
            this.decryptMenuButton.Text = "Decrypt";
            // 
            // encryptMenuButton
            // 
            this.encryptMenuButton.BackColor = System.Drawing.Color.Transparent;
            this.encryptMenuButton.Location = new System.Drawing.Point(1, 99);
            this.encryptMenuButton.Name = "encryptMenuButton";
            this.encryptMenuButton.Selected = false;
            this.encryptMenuButton.Size = new System.Drawing.Size(147, 66);
            this.encryptMenuButton.TabIndex = 4;
            this.encryptMenuButton.Text = "Encrypt";
            // 
            // autoSelectMenuButton
            // 
            this.autoSelectMenuButton.BackColor = System.Drawing.Color.Transparent;
            this.autoSelectMenuButton.Location = new System.Drawing.Point(1, 33);
            this.autoSelectMenuButton.Name = "autoSelectMenuButton";
            this.autoSelectMenuButton.Selected = true;
            this.autoSelectMenuButton.Size = new System.Drawing.Size(147, 66);
            this.autoSelectMenuButton.TabIndex = 3;
            this.autoSelectMenuButton.Text = "Auto-Select Drag/Drop";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(645, 406);
            this.Controls.Add(this.titleBar);
            this.Controls.Add(this.sidePanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "FileAES";
            this.titleBar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.titleBarLogo)).EndInit();
            this.sidePanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel titleBar;
        private System.Windows.Forms.Button quitButton;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Panel sidePanel;
        private System.Windows.Forms.ToolTip slowToolTip;
        private System.Windows.Forms.Label copyrightLabel;
        private CustomControls.SubMenuButton autoSelectMenuButton;
        private CustomControls.SubMenuButton encryptMenuButton;
        private CustomControls.SubMenuButton settingsMenuButton;
        private CustomControls.SubMenuButton decryptMenuButton;
        private System.Windows.Forms.PictureBox titleBarLogo;
    }
}

