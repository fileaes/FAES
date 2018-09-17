using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FAESInstaller
{
    public partial class FAESInstaller : Form
    {

        private bool _isInstallComplete = false;
        private bool _canInstall = false;
        private bool _hasAccepted = false;

        public FAESInstaller()
        {
            InitializeComponent();
            versionLabel.Text = getVersionInfo();
            tosTextbox.Text = Program.getUpdatedTOS();
            if (Program.getAccepted()) passAccept.Checked = true;

            if (Program.getBranch() == "dev")
                branchComboBox.SelectedIndex = 1;
            else
                branchComboBox.SelectedIndex = 0;

            this.ActiveControl = installDir;
            installDir.Text = ProgramFiles86() + @"\mullak99\FileAES";
            installDir.Select(installDir.Text.Length + 1, installDir.Text.Length + 1);
            updateInstaller();

            if (!IsRunAsAdmin())
            {
                if (MessageBox.Show("You are not running the installer as an admin, by doing this you will not be able to install to some directories.\n\nDo you want to launch as admin?", "Notice", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    runAsAdmin();
                else
                    installDir.Text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"mullak99\FileAES");
            }
                
        }

        private void updateInstaller()
        {
            if (getLatestVersion(branchConvert()) == "SERVER ERROR!")
            {
                _canInstall = false;
                versionInstalling.Text = "No Version Found!";
            }
            else
            {
                _canInstall = true;
                versionInstalling.Text = "Installing Version: v" + getLatestVersion(branchConvert());
            }
            
            if (_canInstall && _hasAccepted) installButton.Enabled = true;
            else installButton.Enabled = false;
        }

        private string branchConvert()
        {
            if (branchComboBox.Text == "Development") return "dev";
            else return "stable";
        }

        private string ProgramFiles86()
        {
            if (8 == IntPtr.Size || (!String.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432"))))
                return Environment.GetEnvironmentVariable("ProgramFiles(x86)");
            else
                return Environment.GetEnvironmentVariable("ProgramFiles");
        }

        private void doInstall()
        {
            string result = Installer.doInstall(_canInstall, _hasAccepted, installDir.Text, branchConvert());

            if (result.Contains("Installation Completed!"))
            {
                if (MessageBox.Show("Installation Complete!\n\nDo you want to close the installer?", "Done", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    Application.Exit();
            }
            else if (result.Contains("A connection could not be established with the download server."))
            {
                if (MessageBox.Show("A connection could not be established with the download server.\nPlease check your internet connection or try again later.", "Error", MessageBoxButtons.RetryCancel) == DialogResult.Retry)
                    doInstall();
            }
            else
            {
                MessageBox.Show(result, "Error");
            }
        }

        public string getVersionInfo(bool raw = false)
        {
            if (!raw)
            {
                if (isDebugBuild())
                    return "v" + Application.ProductVersion + " BETA";
                else
                    return "v" + Application.ProductVersion;
            }
            else
                return Application.ProductVersion;
        }

        private bool isDebugBuild()
        {
            return this.GetType().Assembly.GetCustomAttributes(false).OfType<DebuggableAttribute>().Select(da => da.IsJITTrackingEnabled).FirstOrDefault();
        }

        private string getLatestVersion(string sBranch)
        {
            try
            {
                WebClient client = new WebClient();

                string url = "https://builds.mullak99.co.uk/FileAES/checkupdate.php?branch=" + sBranch;

                byte[] html = client.DownloadData(url);
                UTF8Encoding utf = new UTF8Encoding();
                if (String.IsNullOrEmpty(utf.GetString(html)) || utf.GetString(html) == "null")
                    return "SERVER ERROR!";
                else
                    return utf.GetString(html);
            }
            catch (Exception)
            {
                return "SERVER ERROR!";
            }
        }

        private void browseInstallDir_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                installDir.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void passAccept_CheckedChanged(object sender, EventArgs e)
        {
            _hasAccepted = true;
            updateInstaller();
        }

        private void failAccept_CheckedChanged(object sender, EventArgs e)
        {
            _hasAccepted = false;
            updateInstaller();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void installButton_Click(object sender, EventArgs e)
        {
            doInstall();
        }

        private void branchComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateInstaller();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.WindowsShutDown) return;

            if (!_isInstallComplete)
            {
                switch (MessageBox.Show("Are you sure you wish to cancel the installation?", "Cancel Installation", MessageBoxButtons.YesNo))
                {
                    case DialogResult.No:
                        e.Cancel = true;
                        break;
                    default:
                        break;
                }
            }
        }

        internal bool IsRunAsAdmin()
        {
            WindowsIdentity id = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(id);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        private void runAsAdmin()
        {
            if (!IsRunAsAdmin())
            {
                ProcessStartInfo proc = new ProcessStartInfo();
                proc.UseShellExecute = true;
                proc.WorkingDirectory = Environment.CurrentDirectory;
                proc.FileName = Application.ExecutablePath;
                proc.Verb = "runas";

                try
                {
                    Process.Start(proc);
                }
                catch
                {
                    return;
                }

                Environment.Exit(0);
            }
        }
    }
}
