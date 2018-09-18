namespace FAES_GUI.CustomControls
{
    partial class SubMenuButton
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
            this.selectedIndicator = new System.Windows.Forms.Panel();
            this.mainButton = new System.Windows.Forms.Panel();
            this.buttonLabel = new System.Windows.Forms.Label();
            this.mainButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // selectedIndicator
            // 
            this.selectedIndicator.BackColor = System.Drawing.Color.Transparent;
            this.selectedIndicator.Location = new System.Drawing.Point(0, 0);
            this.selectedIndicator.Name = "selectedIndicator";
            this.selectedIndicator.Size = new System.Drawing.Size(10, 66);
            this.selectedIndicator.TabIndex = 0;
            this.selectedIndicator.Click += new System.EventHandler(this.buttonLabel_Click);
            this.selectedIndicator.Leave += new System.EventHandler(this.buttonLabel_MouseLeave);
            this.selectedIndicator.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonLabel_MouseDown);
            this.selectedIndicator.MouseEnter += new System.EventHandler(this.buttonLabel_MouseEnter);
            this.selectedIndicator.MouseLeave += new System.EventHandler(this.buttonLabel_MouseLeave);
            this.selectedIndicator.MouseHover += new System.EventHandler(this.buttonLabel_MouseHover);
            this.selectedIndicator.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonLabel_MouseUp);
            // 
            // mainButton
            // 
            this.mainButton.BackColor = System.Drawing.Color.Transparent;
            this.mainButton.Controls.Add(this.buttonLabel);
            this.mainButton.Location = new System.Drawing.Point(0, 0);
            this.mainButton.Name = "mainButton";
            this.mainButton.Size = new System.Drawing.Size(144, 66);
            this.mainButton.TabIndex = 1;
            this.mainButton.Leave += new System.EventHandler(this.buttonLabel_MouseLeave);
            // 
            // buttonLabel
            // 
            this.buttonLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonLabel.ForeColor = System.Drawing.Color.White;
            this.buttonLabel.Location = new System.Drawing.Point(10, 0);
            this.buttonLabel.Name = "buttonLabel";
            this.buttonLabel.Size = new System.Drawing.Size(133, 66);
            this.buttonLabel.TabIndex = 0;
            this.buttonLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.buttonLabel.Click += new System.EventHandler(this.buttonLabel_Click);
            this.buttonLabel.Leave += new System.EventHandler(this.buttonLabel_MouseLeave);
            this.buttonLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonLabel_MouseDown);
            this.buttonLabel.MouseEnter += new System.EventHandler(this.buttonLabel_MouseEnter);
            this.buttonLabel.MouseLeave += new System.EventHandler(this.buttonLabel_MouseLeave);
            this.buttonLabel.MouseHover += new System.EventHandler(this.buttonLabel_MouseHover);
            this.buttonLabel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonLabel_MouseUp);
            // 
            // SubMenuButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.selectedIndicator);
            this.Controls.Add(this.mainButton);
            this.Name = "SubMenuButton";
            this.Size = new System.Drawing.Size(147, 66);
            this.mainButton.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel selectedIndicator;
        private System.Windows.Forms.Panel mainButton;
        private System.Windows.Forms.Label buttonLabel;
    }
}
