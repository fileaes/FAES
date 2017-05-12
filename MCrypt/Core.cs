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
        if (!String.IsNullOrEmpty(path) && !String.IsNullOrEmpty(path) && !String.IsNullOrEmpty(path) && (File.Exists(path) || Directory.Exists(path)))
            return true;
        else
            return false;
    }

    public bool isDecryptFileValid(string path)
    {
        if (!String.IsNullOrEmpty(path) && File.Exists(path) && !String.IsNullOrEmpty(path) && isValidFiletype(path))
            return true;
        else
            return false;
    }

    public bool isValidFiletype(string path)
    {
        if (Path.GetExtension(path) == ".encrypted" || Path.GetExtension(path) == ".aes" || Path.GetExtension(path) == ".secureaes" || Path.GetExtension(path) == ".mcrypt")
            return true;
        else
            return false;
    }

    public void MoveFolder(string folderToMove, string destination)
    {
        if (folderToMove == null)
        {
            throw new ArgumentNullException("folderToMove");
        }
        if (destination == null)
        {
            throw new ArgumentNullException("destination");
        }
        if (string.IsNullOrEmpty(folderToMove))
        {
            throw new ArgumentException("The parameter may not be empty", "folderToMove");
        }
        if (string.IsNullOrEmpty(destination))
        {
            throw new ArgumentException("The parameter may not be empty", "destination");
        }

        String destinationFolder = CreateDestinationFolderName(folderToMove, destination);
        Directory.Move(folderToMove, destinationFolder);
    }
    private string CreateDestinationFolderName(string folderToMove, string destination)
    {
        var directoryInfo = new DirectoryInfo(folderToMove);

        return Path.Combine(destination, directoryInfo.Name);
    }

    DateTime buildDate = new FileInfo(Assembly.GetExecutingAssembly().Location).LastWriteTime;

    private string buildHash()
    {
        string temp = "";

        temp += buildDate.Day;
        temp += buildDate.Month;
        temp += buildDate.Year % 100;
        temp += buildDate.Hour;
        temp += buildDate.Minute;
        temp += buildDate.Second;

        return temp;
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
        else
            return Application.ProductVersion;
    }
}