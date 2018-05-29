using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileAESUpdater
{
    class Program
    {
        private static bool flagIsDevBuild = true;

        private static bool cleanInstall = false;
        private static bool runAfter = true;
        private static string installDir = Directory.GetCurrentDirectory() + "..";
        private static bool verbose = false;
        private static string packDir = "";
        private static bool customPack = false;
        private static bool cancelUpdateOperation = false;
        private static bool fullInstall = false;
        private static bool developer = false;
        //private static bool urlInstall = false;
        private static string branch = "stable";
        private static string url = "http://builds.mullak99.co.uk/FileAES/latest?branch=stable";
        private static bool createLog = false;
        private static string logLoc = "";


        private static string help = "'--branch' or '-b' can be used to specify the branch that FileAES will be downloaded from (Default: Stable).\n'--clean' or '-c' can be used to perform a clean install.\n'--run' or '-r' will install the program and run it.\n'--directory' or '-dir' or '-d' followed by a directory, will set the install directory.\n'--package' or '-pack' or '-p' followed by the location of a pack file, will install a local package file.\n'--signedchecksums' or '-sc' will show a list of all valid files that can be installed.\n'--verify' or '-v' proceded by a package selection will check if that chosen file is valid and can be installed.";


        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        static void Main(string[] args)
        {
            var handle = GetConsoleWindow();

            if (File.Exists("updaterParams.temp"))
            {
                args = readUpdaterParams();
                try
                {
                    File.Delete("updaterParams.temp");
                }
                catch { }
            }
                

            for (int i = 0; i < args.Length; i++)
            {
                args[i].ToLower();
                if (args[i].Equals("-verbose") || args[i].Equals("--verbose")) verbose = true;
                if (args[i].Equals("-clean") || args[i].Equals("--clean") || args[i].Equals("-c") || args[i].Equals("--c")) cleanInstall = true;
                if (args[i].Equals("-fullinstall") || args[i].Equals("--fullinstall") || args[i].Equals("-f") || args[i].Equals("--f")) fullInstall = true;
                if (args[i].Equals("-run") || args[i].Equals("--run") || args[i].Equals("-r") || args[i].Equals("--r")) runAfter = false;
                if (args[i].Equals("-directory") || args[i].Equals("--directory") || args[i].Equals("-dir") || args[i].Equals("--dir") || args[i].Equals("-d") || args[i].Equals("--d") && !String.IsNullOrEmpty(args[i + 1])) installDir = args[i + 1];
                if (args[i].Equals("-developer") || args[i].Equals("--developer") || args[i].Equals("-dev") || args[i].Equals("--dev") && !String.IsNullOrEmpty(args[i + 1])) developer = true;
                if (args[i].Equals("-package") || args[i].Equals("--package") || args[i].Equals("-pack") || args[i].Equals("--pack") || args[i].Equals("-p") || args[i].Equals("--p") && !String.IsNullOrEmpty(args[i + 1]))
                {
                    if (args[i + 1].Contains("http://") || args[i + 1].Contains("https://"))
                    {
                        //urlInstall = true;
                        url = args[i + 1];
                    }
                    else
                    {
                        customPack = true;
                        packDir = args[i + 1];
                    }
                }
                if (args[i].Equals("-branch") || args[i].Equals("--branch") || args[i].Equals("-b") || args[i].Equals("--b") && !String.IsNullOrEmpty(args[i + 1]))
                {
                    if (args[i + 1] == "dev" || args[i + 1] == "developer") branch = "dev";
                    else branch = "stable";

                    url = "http://builds.mullak99.co.uk/FileAES/latest?branch=" + branch;
                }
                if (args[i].Equals("-help") || args[i].Equals("--help") || args[i].Equals("-h") || args[i].Equals("--h") || args[i].Equals("-?") || args[i].Equals("--?"))
                {
                    Console.WriteLine(help);
                    cancelUpdateOperation = true;
                }
                if (args[i].Equals("-signedchecksums") || args[i].Equals("--signedchecksums") || args[i].Equals("-sc") || args[i].Equals("--sc"))
                {
                    if (checkServerConnection()) logWrite("Signed Checksums (SHA256):\n" + String.Join("\n", getChecksums()), "", false, true);
                    else logWrite("Could not connect to the update server! Please try again later.", "FAIL: Update Server ('http://builds.mullak99.co.uk/FileAES') is not availible!");
                    cancelUpdateOperation = true;
                }
                if (args[i].Equals("-verify") || args[i].Equals("--verify") || args[i].Equals("-v") || args[i].Equals("--v"))
                {
                    try
                    {
                        if (!String.IsNullOrEmpty(packDir))
                        {
                            if (doChecksumValidation(packDir)) logWrite(Path.GetFileName(packDir) + "\nChecksum: '" + getSHA256(packDir) + "' is VALID!", "", false, true);
                            else logWrite(Path.GetFileName(packDir) + "\nChecksum: '" + getSHA256(packDir) + "' is INVALID!", "", false, true);
                        }
                        else
                        {
                            logWrite("Please specify a pack file with '--package' or '-p'.", "FAIL: A pack file was not specified with the '--package' or '-p' argument!");
                        }

                        cancelUpdateOperation = true;
                    }
                    catch
                    {
                        logWrite("Invalid filetype selected! Please select a '.PACK' or '.ZIP' file.", "FAIL: '" + packDir + "' is not a valid filetype!");
                        cancelUpdateOperation = true;
                    }
                }
                if (args[i].Equals("-log") || args[i].Equals("--log") || args[i].Equals("-l") || args[i].Equals("--l"))
                {
                    createLog = true;

                    DateTime dt = DateTime.Now;
                    string time = dt.Day.ToString("00") + "-" + dt.Month.ToString("00") + "-" + dt.Year.ToString("00") + "--" + dt.Hour.ToString("00") + "." + dt.Minute.ToString("00") + "." + dt.Second.ToString("00");
                    logLoc = Path.Combine(Directory.GetCurrentDirectory(), "logs", ("FileAES_Log[" + time + "].log"));
                    if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "logs"))) Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "logs"));
                    File.AppendAllText(logLoc, "FileAESUpdater " + getVersionInfo() + Environment.NewLine);
                    File.AppendAllText(logLoc, "IsAdmin: " + isAdmin() + Environment.NewLine);
                    File.AppendAllText(logLoc, "Log Created on: " + dt.Day.ToString("00") + "/" + dt.Month.ToString("00") + "/" + dt.Year.ToString("00") + " | " + dt.Hour.ToString("00") + ":" + dt.Minute.ToString("00") + ":" + dt.Second.ToString("00") + Environment.NewLine);
                }
                if (args[i].Equals("-silent") || args[i].Equals("--silent") || args[i].Equals("-s") || args[i].Equals("--s")) ShowWindow(handle, SW_HIDE);
                else ShowWindow(handle, SW_SHOW);
            }

            try
            {
                if (!installDir.Contains(":") && !String.IsNullOrEmpty(installDir)) installDir = Path.Combine(Directory.GetCurrentDirectory(), installDir);
                if (installDir[0] == '\\') installDir.TrimStart('\\');
                if (installDir[0] == '/') installDir.TrimStart('/');
            }
            catch
            { }

            try
            {
                if (!packDir.Contains(":") && !String.IsNullOrEmpty(packDir)) packDir = Path.Combine(Directory.GetCurrentDirectory(), packDir);
                if (packDir[0] == '\\') packDir.TrimStart('\\');
                if (packDir[0] == '/') packDir.TrimStart('/');
            }
            catch
            { }

            if (!IsDirectoryWritable(installDir))
            {
                try
                {
                    File.WriteAllText("updaterParams.temp", String.Join(" ", args));
                }
                catch
                { }
                runAsAdmin();
            }

            if (!cancelUpdateOperation) updateFileAES();

        }

        private static void updateFileAES()
        {
            foreach (var process in Process.GetProcessesByName("FileAES"))
            {
                process.Kill();
                logWrite("", "Killed '" + process.ToString() + "'.", true);
            }
            purgeInstallFiles();
            Thread.Sleep(100);

            try
            {
                if (!Directory.Exists(installDir))
                {
                    Directory.CreateDirectory(installDir);
                    logWrite("Install directory has been created!", "PASS: '" + installDir + "' has been created!");
                }
                
            }
            catch
            {
                logWrite("Directory is not writable! Please choose another or launch as admin.", "FAIL: '" + installDir + "' is not writable!");
                return;
            }

            if (customPack)
            {
                if (File.Exists(packDir) && (Path.GetExtension(packDir) == ".pack" || Path.GetExtension(packDir) == ".zip"))
                {
                    ZipFile.ExtractToDirectory(packDir, installDir);
                    logWrite("Extraction Complete!", "Extracted '" + packDir + "' to '" + installDir + "'.");
                }   
                else
                {
                    purgeInstallFiles();
                    logWrite("Invalid filetype selected! Please select a '.PACK' or '.ZIP' file.", "FAIL: '" + packDir + "' is not a valid filetype!");
                    return;
                }
            }
            else if (checkServerConnection())
            {

                Directory.CreateDirectory("data");
                purgeFileAES();
                purgeExtraFiles();

                try
                {
                    string fDir;
                    WebClient webClient = new WebClient();
                    if (branch == "dev") fDir = "data/FileAES-Dev.pack";
                    else fDir = "data/FileAES-Latest.pack";
                    webClient.DownloadFile(new Uri(url), fDir);
                    if (url.Contains("http://builds.mullak99.co.uk/FileAES/latest")) logWrite("Download Complete!", "Downloaded 'v" + getLatestVersion() + "' from 'http://builds.mullak99.co.uk/FileAES/latest' (BRANCH: " + branch.ToUpper() + ")");
                    else logWrite("Download Complete!", "Downloaded file from '" + url + "'.");
                    if (!doChecksumValidation(fDir))
                    {
                        purgeInstallFiles();
                        return;
                    }
                    else
                    {
                        ZipFile.ExtractToDirectory(fDir, installDir);
                        logWrite("Extraction Complete!", "Extracted PACK to '" + installDir + "'.");
                        string path = null;
                        if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"mullak99\FileAES\config\launchParams.cfg"))) path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"mullak99\FileAES\config\launchParams.cfg");
                        else if (File.Exists(@"Config\launchParams.cfg")) path = @"Config\launchParams.cfg";
                        if (path != null)
                        {
                            string[] param = File.ReadAllLines(path);

                            for (int i = 0; i < param.Length; i++)
                            {
                                if (param[i] == "--skipupdate")
                                {
                                    string text = File.ReadAllText(path);
                                    text = text.Replace("--skipupdate", "");
                                    File.WriteAllText(path, text);
                                }
                            }
                        }
                    } 
                }
                catch (WebException)
                {
                    logWrite("Download failed! No files exist in the '" + branch.ToUpper() + "' branch.", "FAIL: No files found in the '" + branch.ToUpper() + "' branch.");
                    purgeInstallFiles();
                    return;
                }
                catch (Exception e)
                {
                    logWrite("An unexpected error occured!", e.ToString());
                    return;
                }
            }
            else
            {
                logWrite("Could not connect to the update server! Please try again later.", "FAIL: Update Server ('http://builds.mullak99.co.uk/FileAES') is not availible!");
                purgeInstallFiles();
                return;
            }

            if (File.Exists("FileAES.exe") || File.Exists(installDir + "/FileAES.exe"))
            {
                purgeExtraFiles();
                if (fullInstall)
                {
                    if (!Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"mullak99\FileAES\config"))) Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"mullak99\FileAES\config"));
                    StreamWriter sw = File.CreateText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"mullak99\FileAES\config\launchParams.cfg"));
                    if (cleanInstall) sw.WriteLine("-c");
                    if (fullInstall) sw.WriteLine("-f");
                    if (developer) sw.WriteLine("-d");
                    if (branch == "dev") sw.WriteLine("--dev");
                    else sw.WriteLine("--stable");
                    sw.Close();
                    logWrite("", @"Created Config files in '" + Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"mullak99\FileAES") + "'.", true);
                }
                else if (!fullInstall)
                {
                    if (!Directory.Exists(Path.Combine(installDir, "config"))) Directory.CreateDirectory(Path.Combine(installDir, "config"));
                    StreamWriter sw = File.CreateText(Path.Combine(installDir, "config/launchParams.cfg"));
                    if (cleanInstall) sw.Write("-c ");
                    if (developer) sw.Write("-d ");
                    sw.Close();
                    logWrite("", @"Created Config files in '" + installDir + "'.", true);
                }
                purgeInstallFiles();

                if (!runAfter)
                {
                    if (File.Exists("FileAES.exe")) Process.Start("FileAES.exe");
                    else if (File.Exists(installDir + "/FileAES.exe")) Process.Start(installDir + "/FileAES.exe");
                    else logWrite("Could not find 'FileAES.exe'.", "FAIL: Could not find 'FileAES.exe' in '" + installDir + "'.");
                }
                logWrite("Done!", "", false, true);
            }
            else
            {
                purgeInstallFiles();
                purgeExtraFiles();
                logWrite("Extraction Failed: FileAES could not be found!", "FAIL: 'FileAES.exe' could not be found in '" + installDir + "'. Extraction assumed to have failed!");
            }
        }

        private static void purgeExtraFiles()
        {
            try
            {
                if (File.Exists("updaterParams.temp")) File.Delete("updaterParams.temp");

                if (installDir == Directory.GetCurrentDirectory() + "..")
                {
                    if (File.Exists("ChangeLog.txt")) File.Delete("ChangeLog.txt");
                    if (File.Exists("LICENSE")) File.Delete("LICENSE");
                    if (File.Exists("README.md")) File.Delete("README.md");
                }
                else
                {
                    if (File.Exists(installDir + "/ChangeLog.txt")) File.Delete(installDir + "/ChangeLog.txt");
                    if (File.Exists(installDir + "/LICENSE")) File.Delete(installDir + "/LICENSE");
                    if (File.Exists(installDir + "/README.md")) File.Delete(installDir + "/README.md");
                }
            }
            catch (Exception e)
            {
                logWrite("An error occured while purging extra files.", e.ToString());
            }
        }

        private static void purgeInstallFiles()
        {
            try
            {
                if (Directory.Exists("data"))
                {
                    if (File.Exists("data/FileAES-Dev.pack")) File.Delete("data/FileAES-Dev.pack");
                    if (File.Exists("data/FileAES-Latest.pack")) File.Delete("data/FileAES-Latest.pack");
                    Directory.Delete("data");
                }
            }
            catch (Exception e)
            {
                logWrite("An error occured while purging updater files.", e.ToString());
            }
        }

        private static void purgeFileAES()
        {
            try
            {
                if (installDir == Directory.GetCurrentDirectory() + ".." && File.Exists("FileAES.exe")) File.Delete("FileAES.exe");
                else if (File.Exists(installDir + "/FileAES.exe")) File.Delete(installDir + "/FileAES.exe");
            }
            catch
            {
                logWrite("An error occured while removing existing install of FileAES.", "FAIL: Could not remove 'FileAES.exe' from '" + installDir + "'.");
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

        private static string getSHA256(string path)
        {
            using (FileStream stream = File.OpenRead(path))
            {
                using (SHA256Managed sha = new SHA256Managed())
                {
                    byte[] checksum = sha.ComputeHash(stream);
                    return BitConverter.ToString(checksum).Replace("-", string.Empty);
                }
            }
        }

        private static string[] getChecksums()
        {
            string temp;
            try
            {
                WebClient client = new WebClient();

                string url = "http://builds.mullak99.co.uk/FileAES/checksums.php";

                byte[] html = client.DownloadData(url);
                UTF8Encoding utf = new UTF8Encoding();
                if (String.IsNullOrEmpty(utf.GetString(html))) temp = "SERVER ERROR!";
                else temp = utf.GetString(html);
            }
            catch
            {
                temp = "SERVER ERROR!";
            }
            return temp.Split(new string[] { "<br/>" }, StringSplitOptions.None);
        }

        private static bool doChecksumValidation(string path)
        {
            string[] validHashes = getChecksums();
            for (int i = 0; i < validHashes.Length; i++)
            {
                if (getSHA256(path).ToLower() == validHashes[i].ToLower())
                {
                    logWrite("", "PASS: SHA256 ('" + getSHA256(path).ToLower() + "') was found on the server!", true);
                    return true;
                }
            }
            logWrite("Checksum failed! An invalid file was downloaded or the file was corrupted. Please try again.", "FAIL: SHA256 ('" + getSHA256(path).ToLower() + "') was not found on the server!");
            return false;
        }

        private static bool IsDirectoryWritable(string dirPath)
        {
            try
            {
                using (FileStream fs = File.Create(Path.Combine(dirPath, Path.GetRandomFileName()), 1, FileOptions.DeleteOnClose))
                return true;
            }
            catch
            {
                return false;
            }
        }

        internal static bool IsRunAsAdmin()
        {
            WindowsPrincipal principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        private static string isAdmin()
        {
            if (IsRunAsAdmin()) return "true";
            else return "false";
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

        public static string getLatestVersion()
        {
            try
            {
                WebClient client = new WebClient();
                string url = "http://builds.mullak99.co.uk/FileAES/checkupdate.php?branch=" + branch;
                byte[] html = client.DownloadData(url);
                UTF8Encoding utf = new UTF8Encoding();
                if (String.IsNullOrEmpty(utf.GetString(html))) return "SERVER ERROR!";
                else if (utf.GetString(html) == "null") return "NO VERSIONS FOUND!";
                else return utf.GetString(html);
            }
            catch
            {
                return "SERVER ERROR!";
            }
        }

        public static string[] readUpdaterParams()
        {
            TextReader tr = new StreamReader("updaterParams.temp");
            string[] temp = tr.ReadLine().Split(' ');
            tr.Close();
            return temp;
        }

        private static void logWrite(string normalText, string verboseText, bool verboseExclusive = false, bool normalEqualsVerbose = false)
        {
            if (verbose && !normalEqualsVerbose) Console.WriteLine(verboseText);
            else if (!verboseExclusive) Console.WriteLine(normalText);

            if (createLog)
            {
                if (normalEqualsVerbose) File.AppendAllText(logLoc, Environment.NewLine + ("[" + DateTime.Now.Hour.ToString("00") + ":" + DateTime.Now.Minute.ToString("00") + ":" + DateTime.Now.Second.ToString("00") + "] ") + normalText);
                else File.AppendAllText(logLoc, Environment.NewLine + ("[" + DateTime.Now.Hour.ToString("00") + ":" + DateTime.Now.Minute.ToString("00") + ":" + DateTime.Now.Second.ToString("00") + "] ") + verboseText);
            }
        }


        static DateTime buildDate = new FileInfo(Assembly.GetExecutingAssembly().Location).LastWriteTime;

        private static string buildHash()
        {
            return (buildDate.Year % 100).ToString() + (buildDate.Month).ToString("00") + (buildDate.Day).ToString("00")  + (buildDate.Hour).ToString("00") + (buildDate.Minute).ToString("00") + (buildDate.Second).ToString("00");
        }

        public static string getVersionInfo(bool raw = false)
        {
            if (!raw)
            {
                if (flagIsDevBuild) return "v" + Application.ProductVersion + " DEV-" + buildHash();
                else return "v" + Application.ProductVersion;
            }
            else return Application.ProductVersion;
        }
    }
}
