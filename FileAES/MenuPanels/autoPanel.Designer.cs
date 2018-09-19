namespace FAES_GUI.MenuPanels
{
    partial class autoPanel
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
            this.placeholderLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // placeholderLabel
            // 
            this.placeholderLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 28F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.placeholderLabel.ForeColor = System.Drawing.Color.White;
            this.placeholderLabel.Location = new System.Drawing.Point(0, 86);
            this.placeholderLabel.Name = "placeholderLabel";
            this.placeholderLabel.Size = new System.Drawing.Size(494, 264);
            this.placeholderLabel.TabIndex = 0;
            this.placeholderLabel.Text = "PLACEHOLDER\r\nDRAG/DROP\r\n";
            this.placeholderLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // autoPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.Controls.Add(this.placeholderLabel);
            this.Name = "autoPanel";
            this.Size = new System.Drawing.Size(494, 380);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label placeholderLabel;
    }
}
