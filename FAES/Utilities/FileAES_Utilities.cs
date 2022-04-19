using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using FAES.AES;
using FAES.Utilities;

namespace FAES
{
    public class FileAES_Utilities
    {
        public static string ExtentionFAES = ".faes";
        public static string ExtentionUFAES = ".ufaes";

        private const bool IsPreReleaseBuild = true;
        private const string PreReleaseTag = "RC_3";

        private static string[] _supportedEncExtensions = new string[] { ExtentionFAES, ".faes", ".mcrypt" };
        private static string _FileAES_TempRoot = Path.Combine(Path.GetTempPath(), "FileAES");
        private static bool _verboseLogging;
        private static uint _cryptoBuffer = 1048576;
        private static bool _localEncrypt = true;

        internal static List<TempPath> _instancedTempFolders = new List<TempPath>();

        /// <summary>
        /// Overrides the default extensions used by FAES. Useful if you are using FAES in a specialised environment
        /// </summary>
        /// <param name="encryptedFAES">Extension of the final, encrypted file</param>
        /// <param name="unencryptedFAES">Extension of the compressed, but not encrypted, file</param>
        /// <param name="limitSupportedExtensions">Limits the supported encryption file extensions to only the provided one</param>
        public static void OverrideDefaultExtensions(string encryptedFAES, string unencryptedFAES, bool limitSupportedExtensions = false)
        {
            ExtentionFAES = encryptedFAES;
            ExtentionUFAES = unencryptedFAES;

            if (limitSupportedExtensions)
                _supportedEncExtensions = new[] { ExtentionFAES };
            else
                _supportedEncExtensions = new[] { ExtentionFAES, ".faes", ".mcrypt" };
        }

        /// <summary>
        /// Whether files should be encrypted in the local folder
        /// </summary>
        public static bool LocalEncrypt
        {
            get => _localEncrypt;
            set
            {
                _localEncrypt = value;
                Logging.Log($"Use Local Encryption: {_localEncrypt}", Severity.DEBUG);
            }
        }

        /// <summary>
        /// Whether files should be encrypted in the OS' Temp folder
        /// </summary>
        public static bool TempEncrypt
        {
            get => !_localEncrypt;
            set
            {
                _localEncrypt = !value;
                Logging.Log($"Use Local Encryption: {_localEncrypt}", Severity.DEBUG);
            }
        }

        /// <summary>
        /// Overrides the default temp folder used by FAES. Useful if you are using FAES in a specialised environment
        /// </summary>
        /// <param name="path">Path to use as FAES' temp folder</param>
        public static void SetFaesTempFolder(string path)
        {
            _FileAES_TempRoot = path;
        }

        /// <summary>
        /// Get FileAES temp folder path
        /// </summary>
        /// <returns>Temp folder path</returns>
        public static string GetFaesTempFolder()
        {
            return _FileAES_TempRoot;
        }

        /// <summary>
        /// Gets if FAES has debug logging enabled (Console.WriteLine)
        /// </summary>
        /// <returns>If verbose logging is enabled</returns>
        public static bool GetVerboseLogging()
        {
            return _verboseLogging;
        }

        /// <summary>
        /// Sets if FAES should log verbosely
        /// </summary>
        /// <param name="logging">If verbose logging should be enabled</param>
        public static void SetVerboseLogging(bool logging)
        {
            _verboseLogging = logging;
        }

        /// <summary>
        /// Gets if the chosen file is encryptable
        /// </summary>
        /// <param name="filePath">Chosen File</param>
        /// <returns>If the file is encryptable</returns>
        public static bool IsFileEncryptable(string filePath)
        {
            return !_supportedEncExtensions.Any(Path.GetExtension(filePath).Contains);
        }

        /// <summary>
        /// Gets if the chosen file is encryptable
        /// </summary>
        /// <param name="filePath">Chosen File</param>
        /// <returns>If the file is encryptable</returns>
        [Obsolete("isFileEncryptable() has been renamed to IsFileEncryptable()")]
        public static bool isFileEncryptable(string filePath)
        {
            return IsFileEncryptable(filePath);
        }

        /// <summary>
        /// Gets if the chosen file is decryptable
        /// </summary>
        /// <param name="filePath">Chosen File</param>
        /// <returns>If the file is decryptable</returns>
        public static bool IsFileDecryptable(string filePath)
        {
            return (_supportedEncExtensions.Any(Path.GetExtension(filePath).Contains) && new MetaData(filePath).IsDecryptable(filePath));
        }

        /// <summary>
        /// Gets if the chosen file is decryptable
        /// </summary>
        /// <param name="filePath">Chosen File</param>
        /// <returns>If the file is decryptable</returns>
        [Obsolete("isFileEncryptable() has been renamed to IsFileEncryptable()")]
        public static bool isFileDecryptable(string filePath)
        {
            return IsFileDecryptable(filePath);
        }

        /// <summary>
        /// Recursively delete the FileAES temp folder of ALL files/folders
        /// Should fix all issues related to lingering files that were not deleted by any FAES instance automatically
        /// WARNING: Can cause issues if ran when other FAES instances are running
        /// </summary>
        public static void PurgeTempFolder()
        {
            if (Directory.Exists(GetFaesTempFolder()))
            {
                Directory.Delete(GetFaesTempFolder(), true);
                Logging.Log($"Purged FAES Temp Folder: {GetFaesTempFolder()}", Severity.DEBUG);
            }
        }

        /// <summary>
        /// Remove InstancedTempFolder with specific path
        /// </summary>
        /// <param name="tempPath">Remove selected path from InstancedTempFolders</param>
        /// <returns>If a value was successfully removed</returns>
        public static bool RemoveInstancedTempFolder(string tempPath)
        {
            TempPath tmp = _instancedTempFolders.First(tPath => tPath.GetTempPath() == tempPath);

            return RemoveInstancedTempPath(tmp);
        }

        /// <summary>
        /// Remove InstancedTempFolder with specific FAES_File filename
        /// </summary>
        /// <param name="faesFile">Remove selected FAES_File from InstancedTempFolders</param>
        /// <returns>If a value was successfully removed</returns>
        public static bool RemoveInstancedTempFolder(FAES_File faesFile)
        {
            TempPath tmp = _instancedTempFolders.First(tPath => tPath.GetFaesFile().GetFileName() == faesFile.GetFileName());

            return RemoveInstancedTempPath(tmp);
        }

        /// <summary>
        /// Remove InstancedTempFolder with specific TempPath
        /// </summary>
        /// <param name="tmp">Temp Path</param>
        /// <returns></returns>
        private static bool RemoveInstancedTempPath(TempPath tmp)
        {
            if (tmp != null)
            {
                FileAES_IntUtilities.SafeDeleteFolder(tmp.GetTempPath());
                _instancedTempFolders.Remove(tmp);
                Logging.Log($"Deleted InstancedTempFolder: {tmp.GetTempPath()}", Severity.DEBUG);

                return true;
            }
            return false;
        }

        /// <summary>
        /// Recursively delete the FileAES temp folder of files/folders created by the current instance of FAES
        /// Should fix most issues related to lingering files that were not deleted by FAES automatically
        /// WARNING: Should NOT cause issues with other running FAES instances. Still not recommended unless all FAES instances are closed
        /// </summary>
        /// <returns>Total number of InstancedTempFolders deleted</returns>
        public static int PurgeInstancedTempFolders()
        {
            int totalDeleted = 0;
            foreach (TempPath tempPath in _instancedTempFolders)
            {
                string pTempPath = tempPath.GetTempPath();
                if (Directory.Exists(pTempPath))
                {
                    Logging.Log($"Deleted InstancedTempFolder[{totalDeleted}]: {pTempPath}", Severity.DEBUG);
                    Directory.Delete(pTempPath, true);
                    totalDeleted++;
                }
            }
            _instancedTempFolders.Clear();
            return totalDeleted;
        }

        /// <summary>
        /// Gets the FAES Version
        /// </summary>
        /// <returns>FAES Version</returns>
        public static string GetVersion()
        {
#pragma warning disable CS0162 //Unreachable code detected
            string[] ver = (typeof(FAES_File).Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version).Split('.');
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (!IsPreReleaseBuild)
                // ReSharper disable once HeuristicUnreachableCode
                return "v" + ver[0] + "." + ver[1] + "." + ver[2];
            return "v" + ver[0] + "." + ver[1] + "." + ver[2] + "-" + PreReleaseTag;
#pragma warning restore CS0162 //Unreachable code detected
        }

        /// <summary>
        /// Gets if the current version of FAES is a Pre-Release version
        /// </summary>
        /// <returns>If the current FAES version is a Pre-Release build</returns>
        public static bool IsPreReleaseVersion()
        {
            return IsPreReleaseBuild;
        }

        /// <summary>
        /// Gets the Password Hint of a chosen encrypted file
        /// </summary>
        /// <param name="filePath">Encrypted File</param>
        /// <returns>Password Hint</returns>
        public static string GetPasswordHint(string filePath)
        {
            return new FAES_File(filePath).GetPasswordHint();
        }

        /// <summary>
        /// Gets the Encryption Timestamp (UNIX UTC) of when the chosen file was encrypted
        /// </summary>
        /// <param name="filePath">Encrypted File</param>
        /// <returns>Encryption Timestamp (UNIX UTC)</returns>
        public static long GetEncryptionTimeStamp(string filePath)
        {
            return new FAES_File(filePath).GetEncryptionTimeStamp();
        }

        /// <summary>
        /// Converts UNIX Timestamp to DateTime
        /// </summary>
        /// <param name="unixTimeStamp">UNIX Timestamp</param>
        /// <returns>Localised DateTime</returns>
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
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
            return new FAES_File(filePath).GetEncryptionVersion();
        }

        /// <summary>
        /// Gets the Compression Mode of a chosen encrypted file
        /// </summary>
        /// <param name="filePath">Encrypted File</param>
        /// <returns>Compression Mode Type</returns>
        public static string GetCompressionMode(string filePath)
        {
            return new FAES_File(filePath).GetEncryptionCompressionMode();
        }

        /// <summary>
        /// Gets the hash type used to hash the ufaes file
        /// </summary>
        /// <returns>Checksum Type</returns>
        public static Checksums.ChecksumType GetChecksumType(string filePath)
        {
            return new FAES_File(filePath).GetChecksumType();
        }

        /// <summary>
        /// Gets the size (in bytes) of the buffer used for the CryptoStream
        /// </summary>
        /// <returns>Size in bytes</returns>
        public static uint GetCryptoStreamBuffer()
        {
            return _cryptoBuffer;
        }

        /// <summary>
        /// Sets the size (in bytes) of the buffer used for the CryptoStream
        /// </summary>
        /// <param name="bufferSize">Size in bytes</param>
        public static void SetCryptoStreamBuffer(uint bufferSize)
        {
            _cryptoBuffer = bufferSize;
        }

        /// <summary>
        /// Attempts to convert an Exception Thrown by FAES into a human-readable error
        /// </summary>
        /// <param name="exception">Exception</param>
        /// <param name="showRawException">Show the raw exception</param>
        /// <returns>Human-Readable Error</returns>
        public static string FAES_ExceptionHandling(Exception exception, bool showRawException = false)
        {
            if (!showRawException)
            {
                switch (exception.Message)
                {
                    case "Error occurred in creating the UFAES file.":
                        return "ERROR: The chosen file(s) could not be compressed as a compressed version already exists in the Temp files! Consider using '--purgeTemp' if you are not using another instance of FileAES and this error persists.";

                    case "Error occurred in encrypting the UFAES file.":
                        return "ERROR: The compressed file could not be encrypted. Please close any other instances of FileAES and try again. Consider using '--purgeTemp' if you are not using another instance of FileAES and this error persists.";

                    case "Error occurred in the decryption of the FAES file.":
                        return "ERROR: The encrypted file could not be decrypted. Please close any other instances of FileAES and try again. Consider using '--purgeTemp' if you are not using another instance of FileAES and this error persists.";

                    case "Error occurred in deleting the UFAES file.":
                        return "ERROR: The compressed file could not be deleted. Please close any other instances of FileAES and try again. Consider using '--purgeTemp' if you are not using another instance of FileAES and this error persists.";

                    case "Error occurred in moving the FAES file after encryption.":
                        return "ERROR: The encrypted file could not be moved to the original destination! Please ensure that a file with the same name does not already exist.";

                    case "Error occurred while deleting the original file/folder.":
                        return "ERROR: The original file/folder could not be deleted after encryption! Please ensure that they are not in-use!";

                    case "Error occurred in decrypting the FAES file.":
                        return "ERROR: The encrypted file could not be decrypted. Please try again.";

                    case "Error occurred in extracting the UFAES file.":
                        return "ERROR: The compressed file could not be extracted! Consider using '--purgeTemp' if you are not using another instance of FileAES and this error persists.";

                    case "Password hint contains invalid characters.":
                        return "ERROR: Password Hint contains invalid characters. Please choose another password hint.";

                    case "FAES File was compressed using an unsupported file format.":
                        return "ERROR: The encrypted file was compressed using an unsupported file format. You are likely using an outdated version of FAES!";

                    case "This method only supports encrypted FAES Files!":
                        return "ERROR: The chosen file does not contain any MetaData since it is not an encrypted FAES File!";

                    case "Error occurred in SafeDeleteFile.":
                        return "ERROR: A file could not be deleted! Is the file in use?";

                    case "Error occurred in SafeDeleteFolder.":
                        return "ERROR: A folder could not be deleted! Is the folder in use?";

                    case "File/Folder not found at the specified path!":
                        return "ERROR: A file/folder was not found at the specified path!";

                    case "Error occurred since the file already exists.":
                        return "ERROR: File already exists at destination and overwriting is disabled!";

                    case "An unexpected error occurred when getting metadata!":
                        return "ERROR: An unexpected error occurred when getting metadata for the selected FAES file!";

                    case "An unexpected error occurred when creating an encryption path!":
                        return "ERROR: An unexpected error occurred when creating an encryption path for the chosen FAES File!";

                    case "Metadata cannot be found at this offset!":
                        return "ERROR: An unexpected error occured when reading FAESv3 metadata! A data chunk ended unexpectedly. Source file may be corrupted!";

                    default:
                        return exception.Message;
                }
            }
            return exception.ToString();
        }
    }
}
