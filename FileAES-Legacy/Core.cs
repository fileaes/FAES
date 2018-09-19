using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

class Core
{
    private const bool flagIsDevBuild = false;

    public static bool isEncryptFileValid(string path)
    {
        if (!String.IsNullOrEmpty(path) && !String.IsNullOrEmpty(path) && !String.IsNullOrEmpty(path) && (File.Exists(path) || Directory.Exists(path))) return true;
        else return false;
    }

    public static bool isDecryptFileValid(string path)
    {
        if (!String.IsNullOrEmpty(path) && File.Exists(path) && !String.IsNullOrEmpty(path) && isValidFiletype(path)) return true;
        else return false;
    }

    public static bool isValidFiletype(string path)
    {
        if (Path.GetExtension(path) == ".faes") return true;
        if (Path.GetExtension(path) == ".mcrypt") return true;
        else return false;
    }

    DateTime buildDate = new FileInfo(Assembly.GetExecutingAssembly().Location).LastWriteTime;

    private string buildHash()
    {
        return (buildDate.Year % 100).ToString() + (buildDate.Month).ToString("00") + (buildDate.Day).ToString("00") + (buildDate.Hour).ToString("00") + (buildDate.Minute).ToString("00");
    }

    private bool isDebugBuild()
    {
        return this.GetType().Assembly.GetCustomAttributes(false).OfType<DebuggableAttribute>().Select(da => da.IsJITTrackingEnabled).FirstOrDefault();
    }

    public string getVersionInfo(bool raw = false, bool formatted = false)
    {
        if (formatted)
        {
            if (isDebugBuild() && flagIsDevBuild) return "v" + Application.ProductVersion + "-DEV" + buildHash();
            else return "v" + Application.ProductVersion;
        }
        if (!raw)
        {
            if (isDebugBuild() && flagIsDevBuild) return "v" + Application.ProductVersion + "-DEV" + buildHash();
            else return "v" + Application.ProductVersion;
        }
        else return Application.ProductVersion;
    }

    public bool IsDirectoryEmpty(string path)
    {
        return !Directory.EnumerateFiles(path).Any();
    }

    public void setIgnoreUpdate(bool state)
    {
        string path = null;
        if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"mullak99\FileAES\config\launchParams.cfg"))) path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"mullak99\FileAES\config\launchParams.cfg");
        else if (File.Exists(@"Config\launchParams.cfg")) path = @"Config\launchParams.cfg";
        if (path != null)
        {
            if (state)
            {
                string[] param = File.ReadAllLines(path);

                for (int i = 0; i < param.Length; i++)
                {
                    if (param[i] == "--skipupdate") return;
                }
                File.AppendAllText(path, "--skipupdate");
            }
            else
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
}