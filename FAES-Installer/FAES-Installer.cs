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

        private bool isInstallComplete = false;
        private bool canInstall = false;
        private bool hasAccepted = false;

        public FAESInstaller()
        {
            InitializeComponent();
            versionLabel.Text = getVersionInfo();
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
                canInstall = false;
                versionInstalling.Text = "No Version Found!";
            }
            else
            {
                canInstall = true;
                versionInstalling.Text = "Installing Version: v" + getLatestVersion(branchConvert());
            }
            
            if (canInstall && hasAccepted) installButton.Enabled = true;
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

        public bool checkServerConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (var stream = client.OpenRead("http://mullak99.co.uk/"))
                    return true;
            }
            catch
            {
                return false;
            }
        }

        private void doInstall()
        {
            if (checkServerConnection() && canInstall && hasAccepted)
            {
                cleanInstallFiles();
                try
                {
                    if (!Directory.Exists(installDir.Text))
                        Directory.CreateDirectory(installDir.Text);
                }
                catch (UnauthorizedAccessException)
                {
                    MessageBox.Show("You do not have permission to write to this location!\nPlease choose another or start with admin privilages.", "Error");
                    cleanInstallFiles();
                    return;
                }
                
                try
                {
                    WebClient webClient = new WebClient();
                    webClient.DownloadFile(new Uri("http://builds.mullak99.co.uk/FileAES/updater/latest"), Path.Combine(installDir.Text, "updater.pack"));
                }
                catch (Exception)
                {
                    MessageBox.Show("You do not have permission to write to this location!\nPlease choose another or start with admin privilages.", "Error");
                    cleanInstallFiles();
                    return;
                }
                try
                {
                    if (File.Exists(Path.Combine(installDir.Text, "FAES-Updater.exe")))
                        File.Delete(Path.Combine(installDir.Text, "FAES-Updater.exe"));

                    ZipFile.ExtractToDirectory(Path.Combine(installDir.Text, "updater.pack"), installDir.Text);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString(), "Error");
                    return;
                }

                try
                {
                    Process p = new Process();
                    p.StartInfo.FileName = Path.Combine(installDir.Text, @"FAES-Updater.exe");
                    p.StartInfo.Arguments = "-c -f -b " + branchConvert() + " -d \"" + installDir.Text + "\"";
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.CreateNoWindow = true;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.Verb = "runas";
                    p.Start();
                    p.WaitForExit();

                    cleanInstallFiles();
                    isInstallComplete = true;
                    if (MessageBox.Show("Installation Complete!\n\nDo you want to close the installer?", "Done", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        Application.Exit();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString(), "Error");
                    return;
                }
            }
            else
            {
                if (MessageBox.Show("A connection could not be established with the download server.\nPlease check your internet connection or try again later.", "Error", MessageBoxButtons.RetryCancel) == DialogResult.Retry)
                    doInstall();
            }
        }

        private void cleanInstallFiles()
        {
            if (File.Exists(Path.Combine(installDir.Text, "updater.pack")))
                File.Delete(Path.Combine(installDir.Text, "updater.pack"));
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

                string url = "http://builds.mullak99.co.uk/FileAES/checkupdate.php?branch=" + sBranch;

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
            hasAccepted = true;
            updateInstaller();
        }

        private void failAccept_CheckedChanged(object sender, EventArgs e)
        {
            hasAccepted = false;
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

            if (!isInstallComplete)
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
