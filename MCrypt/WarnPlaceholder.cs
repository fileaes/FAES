using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MCrypt
{
    public partial class WarnPlaceholder : Form
    {
        Core core = new Core();

        public WarnPlaceholder()
        {
            InitializeComponent();
            versionLabel.Text = core.getVersionInfo();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
