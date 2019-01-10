using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using CoreChecksums;
using FAES.AES;
using FAES.Packaging;

namespace FAES
{
    public class FAES_File
    {
        /// <summary>
        /// The appropriate Operation/Action for the FAES File
        /// </summary>
        protected enum Operation
        {
            NULL,
            ENCRYPT,
            DECRYPT
        };

        /// <summary>
        /// The Type of the FAES File
        /// </summary>
        protected enum FAES_Type
        {
            NULL,
            FILE,
            FOLDER
        };

        protected string _filePath, _password, _fileName, _fullPath, _passwordHint;
        protected Operation _op = Operation.NULL;
        protected FAES_Type _type = FAES_Type.NULL;
        protected string _sha1Hash;

        /// <summary>
        /// Creates a FAES File using a file path
        /// </summary>
        /// <param name="filePath">Path to a file</param>
        public FAES_File(string filePath)
        {
            if (File.Exists(filePath) || Directory.Exists(filePath))
            {
                _filePath = filePath;
                Initialise();
            }
            else throw new FileNotFoundException();
        }

        /// <summary>
        /// Creates a FAES File using a file path and automatically executes the appropriate action (Encrypt/Decrypt)
        /// </summary>
        /// <param name="filePath">Path to a file</param>
        /// <param name="password">Password to encrypt/decrypt the file</param>
        /// <param name="success">Output of if the action was successful</param>
        /// <param name="passwordHint">Hint for the password (only used for encryption)</param>
        [Obsolete("This method of creating a FAES_File is deprecated. Please use the alternative method and specify FAES_Encrypt/FAES_Decrypt")]
        public FAES_File(string filePath, string password, ref bool success, string passwordHint = null)
        {
            if (File.Exists(filePath) || Directory.Exists(filePath))
            {
                _filePath = filePath;
                _password = password;
                _passwordHint = passwordHint;
                Initialise();

                if (isFileDecryptable()) _passwordHint = FileAES_Utilities.GetPasswordHint(_filePath);
            }
            else throw new FileNotFoundException();

            Run(ref success);
        }

        /// <summary>
        /// Initialises/Caches the various methods
        /// </summary>
        private void Initialise()
        {
            isFolder();
            isFileDecryptable();
            getSHA1();
            getFileName();
            getPath();
        }

        /// <summary>
        /// Runs the appropriate action (Encrypt/Decrypt)
        /// </summary>
        /// <param name="success">Output of if the action was successful</param>
        [Obsolete("This method of automatically encrypting/decrypting a FAES_File is deprecated. Please use FAES_Encrypt/FAES_Decrypt.")]
        public void Run(ref bool success)
        {
            if (!String.IsNullOrEmpty(_password))
            {
                if (isFileEncryptable())
                {
                    Console.WriteLine("Encrypting '{0}'...", _filePath);
                    FileAES_Encrypt encrypt = new FileAES_Encrypt(new FAES_File(_filePath), _password, _passwordHint);
                    success = encrypt.encryptFile();
                }
                else if (isFileDecryptable())
                {
                    Console.WriteLine("Decrypting '{0}'...", _filePath);
                    FileAES_Decrypt decrypt = new FileAES_Decrypt(new FAES_File(_filePath), _password);
                    success = decrypt.decryptFile();
                }
                else
                {
                    throw new Exception("The file/folder specified is not valid!");
                }
            }
            else throw new Exception("A password has not been set!");
        }

        /// <summary>
        /// Sets the Password used to encrypt/decrypt the current FAES File
        /// </summary>
        /// <param name="password">Chosen Password</param>
        [Obsolete("This method of automatically encrypting/decrypting a FAES_File is deprecated. Please use FAES_Encrypt/FAES_Decrypt.")]
        public void setPassword(string password)
        {
            _password = password;
        }

        /// <summary>
        /// Gets if the chosen FAES File is encryptable
        /// </summary>
        /// <returns>If the current FAES File is encryptable</returns>
        public bool isFileEncryptable()
        {
            if (_op == Operation.NULL) isFileDecryptable();

            return (_op == Operation.ENCRYPT);
        }

        /// <summary>
        /// Gets if the chosen FAES File is decryptable
        /// </summary>
        /// <returns>If the current FAES File is decryptable</returns>
        public bool isFileDecryptable()
        {
            if (_op == Operation.NULL)
            {
                if (FileAES_Utilities.isFileDecryptable(getPath()))
                    _op = Operation.DECRYPT;
                else
                    _op = Operation.ENCRYPT;
            }

            return (_op == Operation.DECRYPT);
        }

        /// <summary>
        /// Gets the SHA1 hash of the selected FAES File
        /// </summary>
        /// <returns>SHA1 Hash of FAES File</returns>
        public string getSHA1()
        {
            if (isFile())
            {
                if (_sha1Hash == null) _sha1Hash = Checksums.convertHashToString(Checksums.getSHA1(getPath()));
                return _sha1Hash;
            }
            else return null;
        }

        /// <summary>
        /// Gets the filename of the selected FAES File
        /// </summary>
        /// <returns>Filename of FAES File</returns>
        public string getFileName()
        {
            if (_fileName == null) _fileName = Path.GetFileName(getPath());
            return _fileName;
        }

        /// <summary>
        /// Gets if the current FAES File is a folder
        /// </summary>
        /// <returns>If the FAES File is a folder</returns>
        public bool isFolder()
        {
            if (_type == FAES_Type.NULL)
            {
                FileAttributes attr = File.GetAttributes(getPath());

                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                    _type = FAES_Type.FOLDER;
                else
                    _type = FAES_Type.FILE;
            }
            return (_type == FAES_Type.FOLDER);
        }

        /// <summary>
        /// Gets if the current FAES File is a file
        /// </summary>
        /// <returns>Gets if the current FAES File is a file</returns>
        public bool isFile()
        {
            return !isFolder();
        }

        /// <summary>
        /// Gets the path of the selected FAES File
        /// </summary>
        /// <returns>Path of FAES File</returns>
        public string getPath()
        {
            if (_fullPath == null) _fullPath = Path.GetFullPath(_filePath);
            return _fullPath;
        }

        /// <summary>
        /// Gets the path of the selected FAES File
        /// </summary>
        /// <returns>Path of FAES File</returns>
        public override string ToString()
        {
            return getPath();
        }

        /// <summary>
        /// Gets the FAES Type of the FAES File
        /// </summary>
        /// <returns>FAES Type of FAES File</returns>
        public string getFaesType()
        {
            if (_type == FAES_Type.FILE) return "File";
            else return "Folder";
        }

        /// <summary>
        /// Gets the appropriate Operation/Action for the FAES File
        /// </summary>
        /// <returns>Appropriate Operation/Action for the FAES File</returns>
        public string getOperation()
        {
            if (_op == Operation.ENCRYPT) return "Encrypt";
            else return "Decrypt";
        }

        /// <summary>
        /// Gets the Password Hint for the current file
        /// </summary>
        /// <returns>Current files Password Hint</returns>
        public string getPasswordHint()
        {
            return _passwordHint;
        }
    }

    public class FileAES_Encrypt
    {
        protected string tempPath = FileAES_IntUtilities.getDynamicTempFolder("Encrypt");
        protected FAES_File _file;
        protected string _password, _passwordHint;
        internal Crypt crypt = new Crypt();
        internal Compress compress;

        /// <summary>
        /// Encrypts a selected FAES File using a password
        /// </summary>
        /// <param name="file">Encryptable FAES File</param>
        /// <param name="password">Password to encrypt file</param>
        /// <param name="passwordHint">Hint for the password</param>
        public FileAES_Encrypt(FAES_File file, string password, string passwordHint = null, Optimise compression = Optimise.Balanced, byte[] UserSpecifiedSalt = null)
        {
            if (file.isFileEncryptable())
            {
                _file = file;
                _password = password;
                _passwordHint = passwordHint;
                compress = new Compress(compression);
                if (UserSpecifiedSalt != null) crypt.SetUserSalt(UserSpecifiedSalt);
            }
            else throw new Exception("This filetype cannot be encrypted!");
        }

        /// <summary>
        /// Sets the user specified salt.
        /// </summary>
        /// <param name="salt">User-specified salt</param>
        public void SetUserSalt(byte[] salt)
        {
            crypt.SetUserSalt(salt);
        }

        /// <summary>
        /// Gets the user specified salt.
        /// </summary>
        /// <returns>User-specified salt</returns>
        public byte[] GetUserSalt()
        {
            return crypt.GetUserSalt();
        }

        /// <summary>
        /// Removes the user specified salt and returns to using a randomly generated one each encryption.
        /// </summary>
        public void RemoveUserSalt()
        {
            crypt.RemoveUserSalt();
        }

        /// <summary>
        /// Gets if the user specified salt is active.
        /// </summary>
        /// <returns>If the user-specified salt is active</returns>
        public bool IsUserSaltActive()
        {
            return crypt.IsUserSaltActive();
        }

        /// <summary>
        /// Set the compression method used for creating the .UFAES file
        /// </summary>
        /// <param name="optimisedCompression">How to optimise the compression</param>
        public void SetCompressionMode(Optimise optimisedCompression)
        {
            compress = new Compress(optimisedCompression);
        }

        /// <summary>
        /// Set the compression method used for creating the .UFAES file
        /// </summary>
        /// <param name="compressionMode">Compression Mode to use</param>
        /// <param name="compressionLevel">Compression Level to use</param>
        public void SetCompressionMode(FAES.Packaging.CompressionMode compressionMode, FAES.Packaging.CompressionLevel compressionLevel)
        {
            compress = new Compress(compressionMode, compressionLevel);
        }

        /// <summary>
        /// Set the compression method used for creating the .UFAES file
        /// </summary>
        /// <param name="compressionMode">Compression Mode to use</param>
        /// <param name="compressionLevel">Raw Compression Level to use</param>
        public void SetCompressionMode(FAES.Packaging.CompressionMode compressionMode, int compressionLevel)
        {
            compress = new Compress(compressionMode, compressionLevel);
        }

        /// <summary>
        /// Encrypts current file
        /// </summary>
        /// <returns>If the encryption was successful</returns>
        public bool encryptFile()
        {
            string fileToDelete = Path.Combine(tempPath, _file.getFileName() + FileAES_Utilities.ExtentionUFAES);
            string tempFolderName = "";
            bool success;
            try
            {
                try
                {
                    compress.CompressFAESFile(_file, tempPath, Path.Combine(tempPath, _file.getFileName()) + FileAES_Utilities.ExtentionUFAES);
                }
                catch
                {
                    throw new IOException("Error occured in creating the UFAES file.");
                }

                try
                {
                    success = crypt.Encrypt(Path.Combine(tempPath, _file.getFileName()) + FileAES_Utilities.ExtentionUFAES, _password, compress.GetCompressionModeAsString(), _passwordHint);
                }
                catch
                {
                    throw new IOException("Error occured in encrypting the UFAES file.");
                }
                
                try
                {
                    FileAES_IntUtilities.SafeDeleteFile(fileToDelete);
                }
                catch
                {
                    throw new IOException("Error occured in deleting the UFAES file.");
                }

                try
                {
                    if (_file.isFile()) File.Move(Path.Combine(tempPath, _file.getFileName().Substring(0, _file.getFileName().Length - Path.GetExtension(_file.getFileName()).Length) + ".faes"), Path.Combine(Directory.GetParent(_file.getPath()).FullName, _file.getFileName().Substring(0, _file.getFileName().Length - Path.GetExtension(_file.getFileName()).Length) + ".faes"));
                    else File.Move(Path.Combine(tempPath, _file.getFileName() + FileAES_Utilities.ExtentionFAES), Path.ChangeExtension(_file.getPath(), FileAES_Utilities.ExtentionFAES));

                    if (Directory.Exists(Path.Combine(tempPath, tempFolderName))) FileAES_IntUtilities.SafeDeleteFolder(Path.Combine(tempPath, tempFolderName), true);

                    if (_file.isFolder() && Directory.Exists(Path.Combine(tempPath, tempFolderName))) FileAES_IntUtilities.SafeDeleteFolder(Path.Combine(tempPath, tempFolderName), true);

                    if (_file.isFile()) FileAES_IntUtilities.SafeDeleteFile(_file.getPath());
                    else FileAES_IntUtilities.SafeDeleteFolder(_file.getPath(), true);

                    File.SetAttributes(_file.getFileName().Substring(0, _file.getFileName().Length - Path.GetExtension(_file.getFileName()).Length) + ".faes", FileAttributes.Encrypted);
                }
                catch
                {
                    throw new IOException("Error occured in moving the FAES file after encryption.");
                }

            }
            finally
            {
                FileAES_IntUtilities.SafeDeleteFile(fileToDelete);

                FileAES_IntUtilities.DeleteTempPath(_file);
            }
            return success;
        }
    }

    public class FileAES_Decrypt
    {
        protected string tempPath = FileAES_IntUtilities.getDynamicTempFolder("Decrypt");
        protected FAES_File _file;
        protected string _password;
        internal Crypt crypt = new Crypt();
        internal Compress compress = new Compress(Optimise.Balanced);

        /// <summary>
        /// Decrypts a selected FAES File using a password
        /// </summary>
        /// <param name="file">Decryptable FAES File</param>
        /// <param name="password">Password to decrypt file</param>
        public FileAES_Decrypt(FAES_File file, string password)
        {
            if (file.isFileDecryptable())
            {
                _file = file;
                _password = password;

                FileAES_Utilities._instancedTempFolders.Add(tempPath);
            }
            else throw new Exception("This filetype cannot be decrypted!");
        }

        /// <summary>
        /// Decrypts current file
        /// </summary>
        /// <returns>If the decryption was successful</returns>
        public bool decryptFile()
        {
            bool success;

            success = crypt.Decrypt(_file.getPath(), _password);

            File.SetAttributes(Path.ChangeExtension(_file.getPath(), FileAES_Utilities.ExtentionUFAES), FileAttributes.Hidden);

            if (success)
            {
                try
                {
                    compress.UncompressFAESFile(_file, Path.ChangeExtension(_file.getPath(), FileAES_Utilities.ExtentionUFAES));
                }
                catch
                {
                    throw new IOException("Error occured in extracting the UFAES file.");
                }

                try
                {
                    File.SetAttributes(Directory.GetParent(_file.getPath()).FullName, FileAttributes.Hidden);
                    FileAES_IntUtilities.SafeDeleteFile(Path.Combine(Directory.GetParent(_file.getPath()).FullName, _file.getFileName().Substring(0, _file.getFileName().Length - Path.GetExtension(_file.getFileName()).Length) + FileAES_Utilities.ExtentionUFAES));
                    FileAES_IntUtilities.SafeDeleteFile(_file.getPath());
                    File.SetAttributes(Directory.GetParent(_file.getPath()).FullName, FileAttributes.Normal);
                }
                catch
                {
                    throw new IOException("Error occured in deleting the UFAES file.");
                }
            }

            FileAES_IntUtilities.DeleteTempPath(_file);
            FileAES_IntUtilities.SafeDeleteFile(Path.ChangeExtension(_file.getPath(), FileAES_Utilities.ExtentionUFAES.Replace(".", "")));

            return success;
        }

        /// <summary>
        /// Gets the Password Hint for the current file
        /// </summary>
        /// <returns>Current files Password Hint</returns>
        public string getPasswordHint()
        {
            return crypt.GetPasswordHint(_file);
        }
    }

    public class FileAES_Utilities
    {
        public const string ExtentionFAES = ".faes";
        public const string ExtentionUFAES = ".ufaes";
        private static string[] _supportedEncExtentions = new string[3] { ExtentionFAES, ".faes", ".mcrypt" };

        internal static List<string> _instancedTempFolders = new List<string>();

        /// <summary>
        /// Gets if the chosen file is encryptable
        /// </summary>
        /// <param name="filePath">Chosen File</param>
        /// <returns>If the file is encryptable</returns>
        public static bool isFileEncryptable(string filePath)
        {
            return !isFileDecryptable(filePath);
        }
        /// <summary>
        /// Gets if the chosen file is decryptable
        /// </summary>
        /// <param name="filePath">Chosen File</param>
        /// <returns>If the file is decryptable</returns>
        public static bool isFileDecryptable(string filePath)
        {
            if (_supportedEncExtentions.Any(Path.GetExtension(filePath).Contains))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Recursively Deleted the FileAES Temp folder to fix any potential issues related to lingering files
        /// </summary>
        public static void PurgeTempFolder()
        {
            if (Directory.Exists(Path.Combine(Path.GetTempPath(), "FileAES"))) Directory.Delete(Path.Combine(Path.GetTempPath(), "FileAES"), true);
        }

        public static void PurgeInstancedTempFolders()
        {
            foreach (string iTempFolder in _instancedTempFolders)
            {
                if (Directory.Exists(iTempFolder)) Directory.Delete(iTempFolder, true);
            }
            _instancedTempFolders.Clear();
        }

        /// <summary>
        /// Gets the FAES Version
        /// </summary>
        /// <returns>FAES Version</returns>
        public static string GetVersion()
        {
            string[] ver = (typeof(FAES_File).Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version).Split('.');
            return "v" + ver[0] + "." + ver[1] + "." + ver[2];
        }

        /// <summary>
        /// Gets the Password Hint of a chosen encrypted file
        /// </summary>
        /// <param name="filePath">Encrypted File</param>
        /// <returns>Password Hint</returns>
        public static string GetPasswordHint(string filePath)
        {
            return new Crypt().GetPasswordHint(new FAES_File(filePath));
        }

        /// <summary>
        /// Gets the Encryption Timestamp (UNIX UTC) of when the chosen file was encrypted
        /// </summary>
        /// <param name="filePath">Encrypted File</param>
        /// <returns>Encryption Timestamp (UNIX UTC)</returns>
        public static int GetEncryptionTimeStamp(string filePath)
        {
            return new Crypt().GetEncryptionTimestamp(new FAES_File(filePath));
        }

        /// <summary>
        /// Converts UNIX Timestamp to DateTime
        /// </summary>
        /// <param name="unixTimeStamp">UNIX Timestamp</param>
        /// <returns>Localised DateTime</returns>
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        /// <summary>
        /// Gets the FAES Version used to encrypt the chosen file
        /// </summary>
        /// <param name="filePath">Encrypted File</param>
        /// <returns>FAES Version</returns>
        public static string GetEncryptionVersion(string filePath)
        {
            return new Crypt().GetEncryptionVersion(new FAES_File(filePath));
        }

        /// <summary>
        /// Gets the Compression Mode of a chosen encrypted file
        /// </summary>
        /// <param name="filePath">Encrypted File</param>
        /// <returns>Compression Mode Type</returns>
        public static string GetCompressionMode(string filePath)
        {
            return new Crypt().GetCompressionMode(new FAES_File(filePath));
        }

        /// <summary>
        /// Attempts to convert an Exception Thrown by FAES into a human-readable error
        /// </summary>
        /// <param name="exception">Exception</param>
        /// <returns>Human-Readable Error</returns>
        public static string FAES_ExceptionHandling(Exception exception)
        {
            if (exception.ToString().Contains("Error occured in creating the UFAES file."))
                return "ERROR: The chosen file(s) could not be compressed as a compressed version already exists in the Temp files! Consider using '--purgeTemp' if you are not using another instance of FileAES and this error persists.";
            else if (exception.ToString().Contains("Error occured in encrypting the UFAES file."))
                return "ERROR: The compressed file could not be encrypted. Please close any other instances of FileAES and try again. Consider using '--purgeTemp' if you are not using another instance of FileAES and this error persists.";
            else if (exception.ToString().Contains("Error occured in deleting the UFAES file."))
                return "ERROR: The compressed file could not be deleted. Please close any other instances of FileAES and try again. Consider using '--purgeTemp' if you are not using another instance of FileAES and this error persists.";
            else if (exception.ToString().Contains("Error occured in moving the FAES file after encryption."))
                return "ERROR: The encrypted file could not be moved to the original destination! Please ensure that a file with the same name does not already exist.";
            else if (exception.ToString().Contains("Error occured in decrypting the FAES file."))
                return "ERROR: The encrypted file could not be decrypted. Please try again.";
            else if (exception.ToString().Contains("Error occured in extracting the UFAES file."))
                return "ERROR: The compressed file could not be extracted! Consider using '--purgeTemp' if you are not using another instance of FileAES and this error persists.";
            else if (exception.ToString().Contains("Password hint contains invalid characters."))
                return "ERROR: Password Hint contains invalid characters. Please choose another password hint.";
            else if (exception.ToString().Contains("FAES File was compressed using an unsupported file format."))
                return "ERROR: The encrypted file was compressed using an unsupported file format. You are likely using an outdated version of FAES!";
            else if (exception.ToString().Contains("This method only supports encrypted FAES Files!"))
                return "ERROR: The chosen file does not contain any MetaData since it is not an encrypted FAES File!";
            else
                return exception.ToString();
        }
    }

    internal class FileAES_IntUtilities
    {
        /// <summary>
        /// The current Dynamic Temp folder for the current instance of FAES
        /// </summary>
        /// <param name="label">Required Subfolder</param>
        /// <returns>Path to the current Dynamic Temp Folder</returns>
        internal static string getDynamicTempFolder(string label)
        {
            return Path.Combine(Path.GetTempPath(), "FileAES", genRandomTempFolder(label));
        }

        /// <summary>
        /// Generates the name for a pseudo-random temp folder to allow multiple instances of FAES to run without having conflicting temp files
        /// </summary>
        /// <param name="sourceFolder">Source Folder of Pseudo-Random Subdirectory</param>
        /// <returns>Path of pseudo-random temp folder</returns>
        internal static string genRandomTempFolder(string sourceFolder)
        {
            DateTime now = DateTime.Now;
            return sourceFolder + "_" + (now.Day).ToString("00") + (now.Month).ToString("00") + (now.Year % 100).ToString() + (now.Hour).ToString("00") + (now.Minute).ToString("00") + (now.Second).ToString("00");
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
                File.Delete(path);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Safely deletes a folder by ensure it exists before deletion
        /// </summary>
        /// <param name="path">Path to folder</param>
        /// <returns>If the folder is deleted</returns>
        internal static bool SafeDeleteFolder(string path, bool recursive = true)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, recursive);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Copies a directory to another location
        /// </summary>
        /// <param name="sourceDirName">Source directory to copy</param>
        /// <param name="destDirName">Destination directory</param>
        /// <param name="copySubDirs">Recursively copy sub-directories</param>
        internal static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists) return;

            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!Directory.Exists(destDirName)) Directory.CreateDirectory(destDirName);

            FileInfo[] files = dir.GetFiles();

            foreach (FileInfo file in files) file.CopyTo(Path.Combine(destDirName, file.Name), false);
            if (copySubDirs) foreach (DirectoryInfo subdir in dirs) DirectoryCopy(subdir.FullName, Path.Combine(destDirName, subdir.Name), copySubDirs);
        }

        /// <summary>
        /// Creates a new temp path and adds it to the instancedTempFolders list
        /// </summary>
        internal static void CreateTempPath(FAES_File file)
        {
            string tempPath;

            if (file.isFileEncryptable())
                tempPath = FileAES_IntUtilities.getDynamicTempFolder("Encrypt");
            else
                tempPath = FileAES_IntUtilities.getDynamicTempFolder("Decrypt");

            if (!Directory.Exists(tempPath))
            {
                Directory.CreateDirectory(Path.Combine(tempPath));
                FileAES_Utilities._instancedTempFolders.Add(tempPath);
            }
        }

        /// <summary>
        /// Deletes the temp path after use and removes it from the instancedTempFolders list
        /// </summary>
        internal static void DeleteTempPath(FAES_File file)
        {
            string tempPath;

            if (file.isFileEncryptable())
                tempPath = FileAES_IntUtilities.getDynamicTempFolder("Encrypt");
            else
                tempPath = FileAES_IntUtilities.getDynamicTempFolder("Decrypt");

            if (SafeDeleteFolder(tempPath, true))
                FileAES_Utilities._instancedTempFolders.Remove(tempPath);
        }
    }
}
