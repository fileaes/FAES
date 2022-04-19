using System;
using System.Globalization;
using System.IO;
using System.Linq;
using FAES.Utilities;

namespace FAES
{
    internal class FileAES_IntUtilities
    {
        /// <summary>
        /// Gets current Unix time as a string
        /// </summary>
        /// <returns>String representing total seconds since Unix Epoch</returns>
        internal static string GetUnixTime()
        {
            return DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Safely deletes a file by ensure it exists before deletion
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <returns>If the file is deleted</returns>
        internal static bool SafeDeleteFile(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    File.Delete(path);
                    Logging.Log($"SafeDeleteFile: {path}", Severity.DEBUG);

                    return true;
                }
                catch
                {
                    throw new UnauthorizedAccessException("Error occurred in SafeDeleteFile. File cannot be deleted!");
                }
            }
            return false;
        }

        /// <summary>
        /// Safely deletes a folder by ensure it exists before deletion
        /// </summary>
        /// <param name="path">Path to folder</param>
        /// <param name="recursive">If SafeDeleteFolder should delete contents of the folder</param>
        /// <returns>If the folder is deleted</returns>
        internal static bool SafeDeleteFolder(string path, bool recursive = true)
        {
            if (Directory.Exists(path))
            {
                try
                {
                    Directory.Delete(path, recursive);
                    Logging.Log($"SafeDeleteFolder: {path}", Severity.DEBUG);

                    return true;
                }
                catch
                {
                    throw new UnauthorizedAccessException("Error occurred in SafeDeleteFolder. Folder cannot be deleted!");
                }
            }
            return false;
        }

        /// <summary>
        /// Copies a directory to another location
        /// </summary>
        /// <param name="sourceDirName">Source directory to copy</param>
        /// <param name="destDirName">Destination directory</param>
        /// <param name="firstPass">Whether the current run of this method is the first one</param>
        internal static void DirectoryCopy(string sourceDirName, string destDirName, bool firstPass = true)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists) return;

            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!Directory.Exists(destDirName)) Directory.CreateDirectory(destDirName);
            if (firstPass)
            {
                string newDest = Path.Combine(destDirName, new DirectoryInfo(sourceDirName).Name);
                Directory.CreateDirectory(newDest);
                destDirName = newDest;
            }

            FileInfo[] files = dir.GetFiles();

            foreach (FileInfo file in files) file.CopyTo(Path.Combine(destDirName, file.Name), false);
            foreach (DirectoryInfo subDir in dirs) DirectoryCopy(subDir.FullName, Path.Combine(destDirName, subDir.Name), false);
        }

        /// <summary>
        /// Create the path for a local temp path
        /// </summary>
        /// <param name="file">FAES File</param>
        /// <returns>Temp path</returns>
        internal static string CreateLocalTempPath(FAES_File file)
        {
            return Path.Combine(Directory.GetParent(file.GetPath())?.FullName ?? string.Empty, ".faesEncrypt");
        }

        /// <summary>
        /// Creates a new temp path and adds it to the instancedTempFolders list
        /// </summary>
        /// <param name="file">FAES File</param>
        /// <param name="tempFolder">Temp Folder Root</param>
        /// <param name="InstanceFolder">Folder for current instance of FAES</param>
        /// <param name="mergeDateTime">Merge DateTime folder into the temp folder name</param>
        /// <returns>Temp path created</returns>
        internal static string CreateTempPath(FAES_File file, string tempFolder, string InstanceFolder, bool mergeDateTime = false)
        {
            string dateTime = GetUnixTime();
            string tempInstancePath = tempFolder;

            if (mergeDateTime)
                tempInstancePath = tempFolder + dateTime;
            else
                tempInstancePath = Path.Combine(tempFolder, dateTime);

            string tempPath = Path.Combine(tempInstancePath, InstanceFolder);

            if (FileAES_Utilities._instancedTempFolders.All(tPath => tPath.GetFaesFile().GetFileName() != file.GetFileName()))
            {
                AddToInstancedFolder(file, tempInstancePath);
                Logging.Log($"Created TempPath: {tempPath}", Severity.DEBUG);
            }
            else
            {
                tempPath = FileAES_Utilities._instancedTempFolders.First(tPath => tPath.GetFaesFile().GetFileName() == file.GetFileName()).GetTempPath();
            }

            if (!Directory.Exists(tempPath))
            {
                Directory.CreateDirectory(tempPath);
                File.SetAttributes(tempInstancePath, FileAttributes.Hidden);
            }

            return tempPath;
        }

        /// <summary>
        /// Add a FAES File to the instanced folders
        /// </summary>
        /// <param name="file">FAES file</param>
        /// <param name="folder">Sub-Folder name</param>
        internal static void AddToInstancedFolder(FAES_File file, string folder)
        {
            TempPath path = new TempPath(file, folder);
            FileAES_Utilities._instancedTempFolders.Add(path);
        }

        /// <summary>
        /// Creates the various paths required when encrypting a file
        /// </summary>
        /// <param name="file">FAES File to encrypt</param>
        /// <param name="compressionType">Compression Type</param>
        /// <param name="tempRawPath">Raw temp filepath</param>
        /// <param name="tempRawFile">Raw filepath for file</param>
        /// <param name="tempOutputPath">Raw output filepath</param>
        internal static void CreateEncryptionFilePath(FAES_File file, string compressionType, out string tempRawPath, out string tempRawFile, out string tempOutputPath)
        {
            string tempPath;
            if (FileAES_Utilities.LocalEncrypt)
                tempPath = FileAES_IntUtilities.CreateTempPath(file, FileAES_IntUtilities.CreateLocalTempPath(file), compressionType + "_Compress-" + FileAES_IntUtilities.GetUnixTime(), true);
            else
                tempPath = FileAES_IntUtilities.CreateTempPath(file, FileAES_Utilities.GetFaesTempFolder(), compressionType + "_Compress-" + FileAES_IntUtilities.GetUnixTime());

            tempRawPath = Path.Combine(tempPath, "contents");
            tempRawFile = Path.Combine(tempRawPath, file.GetFileName());
            tempOutputPath = Path.Combine(Directory.GetParent(tempPath)?.FullName ?? throw new InvalidOperationException("An unexpected error occurred when creating an encryption path!"), Path.ChangeExtension(file.GetFileName(), FileAES_Utilities.ExtentionUFAES));

            if (!Directory.Exists(tempRawPath))
                Directory.CreateDirectory(tempRawPath);

            if (file.IsFile())
                File.Copy(file.GetPath(), tempRawFile);
            else
                FileAES_IntUtilities.DirectoryCopy(file.GetPath(), tempRawPath);
        }
    }
}
