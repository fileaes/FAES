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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileAES
{
    public partial class FileAES_Update : Form
    {
        Core core = new Core();

        public FileAES_Update()
        {
            InitializeComponent();
            checkForUpdate();
            FaesVersion.Text = "FAES Version: " + FAES.FileAES_Utilities.GetVersion();
        }

        private void updateCurrentVersion()
        {
            currentVerLabel.Text = "Current Version: " + core.getVersionInfo(false, true);
        }

        private string getLatestVersion()
        {
            try
            {
                WebClient client = new WebClient();
                byte[] html = client.DownloadData("http://builds.mullak99.co.uk/FileAES/checkupdate.php?branch=" + Program.getBranch());
                UTF8Encoding utf = new UTF8Encoding();
                if (String.IsNullOrEmpty(utf.GetString(html)) || utf.GetString(html) == "null")
                    return "0.0.0.0";
                else
                    return utf.GetString(html);
            }
            catch (Exception)
            {
                return "0.0.0.0";
            }
        }
        public bool isUpdateAvailable()
        {
            try
            {
                bool isLatestDevBuild = false;
                bool isCurrentDevBuild = false;
                Int64 latestSubVersion, currentSubVersion;
                if (getLatestVersion().Contains("DEV")) isLatestDevBuild = true;
                string latestVerStripped = getLatestVersion();
                if (core.getVersionInfo(false, true).Contains("DEV"))
                {
                    isCurrentDevBuild = true;
                    latestVerStripped = latestVerStripped.Substring(0, latestVerStripped.LastIndexOf("-"));
                    latestVerStripped = latestVerStripped.Replace("v", "");
                }
                if (isLatestDevBuild) latestSubVersion = Convert.ToInt64(getLatestVersion().Substring(getLatestVersion().LastIndexOf("V") + 1));
                else latestSubVersion = 0;
                if (isCurrentDevBuild) currentSubVersion = Convert.ToInt64(core.getVersionInfo(false, true).Substring(core.getVersionInfo(false, true).LastIndexOf("V") + 1));
                else currentSubVersion = 0;

                int[] latestServerVer = ToIntArray(latestVerStripped, '.');
                int[] currentAppVer = ToIntArray(core.getVersionInfo(true), '.');

                if (getLatestVersion() == "0.0.0.0")
                    return false;
                else if (latestServerVer[0] > currentAppVer[0])
                    return true;
                else if (latestServerVer[1] > currentAppVer[1] && latestServerVer[0] == currentAppVer[0])
                    return true;
                else if (latestServerVer[2] > currentAppVer[2] && latestServerVer[1] == currentAppVer[1] && latestServerVer[0] == currentAppVer[0])
                    return true;
                else if (latestServerVer[3] > currentAppVer[3] && latestServerVer[2] == currentAppVer[2] && latestServerVer[1] == currentAppVer[1] && latestServerVer[0] == currentAppVer[0])
                    return true;
                else if ((isLatestDevBuild && isCurrentDevBuild) && latestSubVersion > currentSubVersion && latestServerVer[3] == currentAppVer[3] && latestServerVer[2] == currentAppVer[2] && latestServerVer[1] == currentAppVer[1] && latestServerVer[0] == currentAppVer[0])
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public bool isAppNewer()
        {
            try
            {
                bool isLatestDevBuild = false;
                bool isCurrentDevBuild = false;
                Int64 latestSubVersion, currentSubVersion;
                if (getLatestVersion().Contains("DEV")) isLatestDevBuild = true;
                string latestVerStripped = getLatestVersion();
                if (core.getVersionInfo(false, true).Contains("DEV"))
                {
                    isCurrentDevBuild = true;
                    latestVerStripped = latestVerStripped.Substring(0, latestVerStripped.LastIndexOf("-"));
                    latestVerStripped = latestVerStripped.Replace("v", "");
                }
                if (isLatestDevBuild) latestSubVersion = Convert.ToInt64(getLatestVersion().Substring(getLatestVersion().LastIndexOf("V") + 1));
                else latestSubVersion = 0;
                if (isCurrentDevBuild) currentSubVersion = Convert.ToInt64(core.getVersionInfo(false, true).Substring(core.getVersionInfo(false, true).LastIndexOf("V") + 1));
                else currentSubVersion = 0;

                int[] latestServerVer = ToIntArray(latestVerStripped, '.');
                int[] currentAppVer = ToIntArray(core.getVersionInfo(true), '.');

                if (latestServerVer[0] < currentAppVer[0])
                    return true;
                else if (latestServerVer[1] < currentAppVer[1] && latestServerVer[0] == currentAppVer[0])
                    return true;
                else if (latestServerVer[2] < currentAppVer[2] && latestServerVer[1] == currentAppVer[1] && latestServerVer[0] == currentAppVer[0])
                    return true;
                else if (latestServerVer[3] < currentAppVer[3] && latestServerVer[2] == currentAppVer[2] && latestServerVer[1] == currentAppVer[1] && latestServerVer[0] == currentAppVer[0])
                    return true;
                else if ((isLatestDevBuild && isCurrentDevBuild) && latestSubVersion < currentSubVersion && latestServerVer[3] == currentAppVer[3] && latestServerVer[2] == currentAppVer[2] && latestServerVer[1] == currentAppVer[1] && latestServerVer[0] == currentAppVer[0])
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        public string detailedUpdateInfo()
        {
            if (isUpdateAvailable())
                return getLatestVersion();
            else if (getLatestVersion() == "0.0.0.0")
                return "SERVERERROR";
            else if (isAppNewer())
                return "APPNEWER";
            else
                return "LATESTVERSION";
        }

        public void checkForUpdate()
        {
            updateCurrentVersion();

            if (detailedUpdateInfo() == "LATESTVERSION")
            {
                descLabel.Text = "You are on the latest version!";
                updateButton.Visible = false;
                updateButton.Enabled = false;
                neverRemindButton.Visible = false;
                neverRemindButton.Enabled = false;
                latestVerLabel.Text = "Latest Version: v" + getLatestVersion();
            }
            else if (detailedUpdateInfo() == "APPNEWER")
            {
                descLabel.Text = "You are on a private build.";
                updateButton.Visible = false;
                updateButton.Enabled = false;
                neverRemindButton.Visible = false;
                neverRemindButton.Enabled = false;
                latestVerLabel.Text = "Latest Version: v" + getLatestVersion();
            }
            else if (detailedUpdateInfo() == "SERVERERROR")
            {
                descLabel.Text = "Unable to connect to the update server.";
                updateButton.Visible = false;
                updateButton.Enabled = false;
                neverRemindButton.Visible = false;
                neverRemindButton.Enabled = false;
                latestVerLabel.Text = "Latest Version: SERVER ERROR!";
            }
            else
            {
                descLabel.Text = "An update is available!";
                updateButton.Visible = true;
                updateButton.Enabled = true;
                neverRemindButton.Visible = true;
                neverRemindButton.Enabled = true;
                if (!Program.getSkipUpdate()) this.Show();
                latestVerLabel.Text = "Latest Version: v" + getLatestVersion();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.WindowsShutDown) return;

            e.Cancel = true;
            Hide();
        }

        private int[] ToIntArray(string value, char separator)
        {
            return Array.ConvertAll(value.Split(separator), s => int.Parse(s));
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            selfUpdate(Program.getCleanUpdates());
        }

        private void neverRemindButton_Click(object sender, EventArgs e)
        {
            core.setIgnoreUpdate(true);
        }

        private void checkForUpdateButton_Click(object sender, EventArgs e)
        {
            checkForUpdateButton.Enabled = false;
            checkForUpdate();
            checkForUpdateButton.Enabled = true;
        }

        public static void selfUpdate(bool doCleanUpdate = false)
        {
            string installDir = Directory.GetCurrentDirectory();

            if (checkServerConnection())
                try
                {
                    File.Delete(Path.Combine(installDir, "FAES-Updater.exe"));
                    File.Delete(Path.Combine(installDir, "updater.pack"));
                    WebClient webClient = new WebClient();
                    webClient.DownloadFile(new Uri("http://builds.mullak99.co.uk/FileAES/updater/latest"), Path.Combine(installDir, "updater.pack"));
                    ZipFile.ExtractToDirectory(Path.Combine(installDir, "updater.pack"), Directory.GetCurrentDirectory() + "..");
                    File.Delete(Path.Combine(installDir, "updater.pack"));
                    Thread.Sleep(100);

                    string args = "";
                    if (doCleanUpdate) args += "-c ";
                    if (Program.getFullInstall()) args += "-f ";
                    args += "-b " + Program.getBranch() + " ";
                    args += "--silent ";
                    Process.Start(Path.Combine(installDir, "FAES-Updater.exe"), args);

                    Environment.Exit(0);
                }
                catch (UnauthorizedAccessException)
                {
                    if (MessageBox.Show("You are not running FileAES as an admin, by doing this you cannot update the application in admin protected directories.\n\nDo you want to launch as admin?", "Notice", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        runAsAdmin();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString(), "Error");
                    if (File.Exists(Path.Combine(installDir, "FAES-Updater.exe")))
                        File.Delete(Path.Combine(installDir, "FAES-Updater.exe"));
                    if (File.Exists(Path.Combine(installDir, "updater.pack")))
                        File.Delete(Path.Combine(installDir, "updater.pack"));
                }
            else
            {
                File.Delete(Path.Combine(installDir, "updater.pack"));
            }
        }

        public static bool checkServerConnection()
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

        internal static bool IsRunAsAdmin()
        {
            WindowsIdentity id = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(id);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        private static void runAsAdmin()
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
