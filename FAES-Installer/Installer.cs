using FAESInstaller;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

public class Installer
{
    public Installer()
    {

    }

    public static void cleanInstallFiles(string installDir)
    {
        if (File.Exists(Path.Combine(installDir, "updater.pack")))
            File.Delete(Path.Combine(installDir, "updater.pack"));
    }

    public static bool checkServerConnection()
    {
        try
        {
            using (var client = new WebClient())
            using (var stream = client.OpenRead("https://mullak99.co.uk/"))
                return true;
        }
        catch
        {
            return false;
        }
    }

    public static string doInstall(bool canInstall, bool hasAccepted, string installDir, string branch)
    {
        if (checkServerConnection() && canInstall && hasAccepted)
        {
            cleanInstallFiles(installDir);
            try
            {
                if (!Directory.Exists(installDir))
                    Directory.CreateDirectory(installDir);
            }
            catch (UnauthorizedAccessException)
            {
                cleanInstallFiles(installDir);
                return "You do not have permission to write to this location!\nPlease choose another or start with admin privilages.";
            }

            try
            {
                WebClient webClient = new WebClient();
                webClient.DownloadFile(new Uri("https://builds.mullak99.co.uk/FileAES/updater/latest"), Path.Combine(installDir, "updater.pack"));
            }
            catch (Exception)
            {
                cleanInstallFiles(installDir);
                return "You do not have permission to write to this location!\nPlease choose another or start with admin privilages.";
            }
            try
            {
                if (File.Exists(Path.Combine(installDir, "FAES-Updater.exe")))
                    File.Delete(Path.Combine(installDir, "FAES-Updater.exe"));

                ZipFile.ExtractToDirectory(Path.Combine(installDir, "updater.pack"), installDir);
            }
            catch (Exception e)
            {
                return e.ToString();
            }

            try
            {
                Process p = new Process();
                p.StartInfo.FileName = Path.Combine(installDir, @"FAES-Updater.exe");
                p.StartInfo.Arguments = "-c -f -b " + branch + " -d \"" + installDir + "\"";
                p.StartInfo.UseShellExecute = false;
                if (!Program.isVerbose()) p.StartInfo.CreateNoWindow = true;
                else p.StartInfo.Arguments += " --verbose";
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.Verb = "runas";
                p.Start();
                p.WaitForExit();

                cleanInstallFiles(installDir);
                return "Installation Completed!";
                
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }
        else
        {
            return "A connection could not be established with the download server.\nPlease check your internet connection or try again later.";
        }
    }
}
