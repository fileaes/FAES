using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

class Core
{
    private bool flagIsDevBuild = true;

    public bool isEncryptFileValid(string path)
    {
        if (!String.IsNullOrEmpty(path) && !String.IsNullOrEmpty(path) && !String.IsNullOrEmpty(path) && (File.Exists(path) || Directory.Exists(path))) return true;
        else return false;
    }

    public bool isDecryptFileValid(string path)
    {
        if (!String.IsNullOrEmpty(path) && File.Exists(path) && !String.IsNullOrEmpty(path) && isValidFiletype(path)) return true;
        else return false;
    }

    public bool isValidFiletype(string path)
    {
        if (Path.GetExtension(path) == ".mcrypt") return true;
        else  return false;
    }

    public void MoveFolder(string folderToMove, string destination)
    {
        String destinationFolder = CreateDestinationFolderName(folderToMove, destination);
        Directory.Move(folderToMove, destinationFolder);
    }
    private string CreateDestinationFolderName(string folderToMove, string destination)
    {
        return Path.Combine(destination, new DirectoryInfo(folderToMove).Name);
    }

    DateTime buildDate = new FileInfo(Assembly.GetExecutingAssembly().Location).LastWriteTime;

    private string buildHash()
    {
        return (buildDate.Day).ToString("00") + (buildDate.Month).ToString("00") + (buildDate.Year % 100).ToString() + (buildDate.Hour).ToString("00") + (buildDate.Minute).ToString("00") + (buildDate.Second).ToString("00");
    }

    public string tempFolderNameGen(string sourceFolder)
    {
        DateTime now = DateTime.Now;
        return sourceFolder + "_" + (now.Day).ToString("00") + (now.Month).ToString("00") + (now.Year % 100).ToString() + (now.Hour).ToString("00") + (now.Minute).ToString("00") + (now.Second).ToString("00");
    }

    private bool isDebugBuild()
    {
        return this.GetType().Assembly.GetCustomAttributes(false).OfType<DebuggableAttribute>().Select(da => da.IsJITTrackingEnabled).FirstOrDefault();
    }

    public string getVersionInfo(bool raw = false)
    {
        if (!raw)
        {
            if (isDebugBuild() && !flagIsDevBuild) return "v" + Application.ProductVersion + " BETA";
            else if (isDebugBuild() && flagIsDevBuild) return "v" + Application.ProductVersion + " DEV-" + buildHash();
            else return "v" + Application.ProductVersion;
        }
        else return Application.ProductVersion;
    }

    public bool IsDirectoryEmpty(string path)
    {
        return !Directory.EnumerateFiles(path).Any();
    }

    public void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
    {
        DirectoryInfo dir = new DirectoryInfo(sourceDirName);

        if (!dir.Exists) return;

        DirectoryInfo[] dirs = dir.GetDirectories();

        if (!Directory.Exists(destDirName)) Directory.CreateDirectory(destDirName);

        FileInfo[] files = dir.GetFiles();

        foreach (FileInfo file in files) file.CopyTo(Path.Combine(destDirName, file.Name), false);
        if (copySubDirs) foreach (DirectoryInfo subdir in dirs) DirectoryCopy(subdir.FullName, Path.Combine(destDirName, subdir.Name), copySubDirs);
    }
}