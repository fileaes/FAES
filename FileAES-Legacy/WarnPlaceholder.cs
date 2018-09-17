using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileAES
{
    public partial class WarnPlaceholder : Form
    {
        Core core = new Core();
        FileAES_Update update = new FileAES_Update();

        public WarnPlaceholder()
        {
            InitializeComponent();
            versionLabel.Text = core.getVersionInfo();
        }

        private void WarnPlaceholder_Load(object sender, EventArgs e)
        {
            update.checkForUpdate();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            update.Close();
            this.Close();
        }

        private void versionLabel_Click(object sender, EventArgs e)
        {
            update.Show();
        }
    }
}
