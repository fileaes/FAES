using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using FAES.AES;
using FAES.Packaging;
using FAES.AES.Compatability;

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
        protected string _fileHash;
        protected MetaData _faesMetaData;

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
            else throw new FileNotFoundException("File/Folder not found at the specified path!");
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
            }
            else throw new FileNotFoundException("File/Folder not found at the specified path!");

            Run(ref success);
        }

        /// <summary>
        /// Initialises/Caches the various methods
        /// </summary>
        private void Initialise()
        {
            isFile();
            isFileDecryptable();
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
                    Logging.Log(String.Format("Encrypting '{0}'...", _filePath), Severity.INFO);
                    FileAES_Encrypt encrypt = new FileAES_Encrypt(new FAES_File(_filePath), _password, _passwordHint);
                    success = encrypt.encryptFile();
                }
                else if (isFileDecryptable())
                {
                    Logging.Log(String.Format("Decrypting '{0}'...", _filePath), Severity.INFO);
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
                {
                    _op = Operation.DECRYPT;
                    _faesMetaData = new MetaData(this);
                }
                else
                {
                    _op = Operation.ENCRYPT;
                    _faesMetaData = null;
                }
            }

            return (_op == Operation.DECRYPT);
        }

        /// <summary>
        /// Gets the selected hash of the current FAES File
        /// </summary>
        /// <param name="hashType">Type of hash</param>
        /// <returns>Selected hash of FAES File</returns>
        public string getFileHash(Checksums.ChecksumType hashType)
        {
            if (isFile())
            {
                switch (hashType)
                {
                    case Checksums.ChecksumType.SHA1:
                        return Checksums.ConvertHashToString(Checksums.GetSHA1(getPath()));
                    case Checksums.ChecksumType.SHA256:
                        return Checksums.ConvertHashToString(Checksums.GetSHA256(getPath()));
                    case Checksums.ChecksumType.SHA512:
                        return Checksums.ConvertHashToString(Checksums.GetSHA512(getPath()));
                    default:
                        return null;
                }
            }
            else return null;
        }

        /// <summary>
        /// Gets the SHA1 hash of the selected FAES File
        /// </summary>
        /// <returns>SHA1 Hash of FAES File</returns>
        [Obsolete("This method of getting a files hash is deprecated. Please use getFileHash.")]
        public string getSHA1()
        {
            return getFileHash(Checksums.ChecksumType.SHA1);
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
        [Obsolete("getPasswordHint() has been renamed to GetPasswordHint()")]
        public string getPasswordHint()
        {
            return GetPasswordHint();
        }

        /// <summary>
        /// Gets the Password Hint for the current file
        /// </summary>
        /// <returns>Current files Password Hint</returns>
        public string GetPasswordHint()
        {
            if (_faesMetaData != null)
            {
                return _faesMetaData.GetPasswordHint();
            }
            else return _passwordHint;
        }

        /// <summary>
        /// Gets the Version of FAES used to encrypt the file
        /// </summary>
        /// <returns>FAES Version</returns>
        public string GetEncryptionVersion()
        {
            if (_faesMetaData != null)
            {
                return _faesMetaData.GetEncryptionVersion();
            }
            else throw new NotSupportedException("Cannot read MetaData of an unencryted file.");
        }

        /// <summary>
        /// Gets the Compression Method used to compress the encrypted file
        /// </summary>
        /// <returns>Compression Mode Type</returns>
        public string GetEncryptionCompressionMode()
        {
            if (_faesMetaData != null)
            {
                return _faesMetaData.GetCompressionMode();
            }
            else throw new NotSupportedException("Cannot read MetaData of an unencryted file.");
        }

        /// <summary>
        /// Gets the UNIX Timestamp of when the file was encrypted
        /// </summary>
        /// <returns>UNIX Timestamp</returns>
        public long GetEncryptionTimeStamp()
        {
            if (_faesMetaData != null)
            {
                return _faesMetaData.GetEncryptionTimestamp();
            }
            else throw new NotSupportedException("Cannot read MetaData of an unencryted file.");
        }

        public string GetOriginalFileName()
        {
            if (_faesMetaData != null)
            {
                return _faesMetaData.GetOriginalFileName();
            }
            else throw new NotSupportedException("Cannot read MetaData of an unencryted file.");
        }
    }

    public class FileAES_Encrypt
    {
        protected FAES_File _file;
        protected string _password, _passwordHint;
        protected bool _deletePost, _overwriteDuplicate;
        protected decimal _percentEncComplete = 0;
        protected MetaData _faesMetaData;
        protected Checksums.ChecksumType _checksumType = Checksums.ChecksumType.SHA256;

        internal Crypt crypt = new Crypt();
        internal Compress compress;

        /// <summary>
        /// Encrypts a selected FAES File using a password
        /// </summary>
        /// <param name="file">Encryptable FAES File</param>
        /// <param name="password">Password to encrypt file</param>
        /// <param name="passwordHint">Hint for the password</param>
        /// <param name="compression">Compression level to use</param>
        /// <param name="UserSpecifiedSalt">User specified salt</param>
        /// <param name="deleteAfterEncrypt">Whether the original file/folder should be deleted after a successful encryption</param>
        /// <param name="overwriteDuplicate">Whether duplicate files should be forcibly overwritten</param>
        public FileAES_Encrypt(FAES_File file, string password, string passwordHint = null, Optimise compression = Optimise.Balanced, byte[] UserSpecifiedSalt = null, bool deleteAfterEncrypt = true, bool overwriteDuplicate = true)
        {
            Logging.Log(String.Format("FAES {0} started!", FileAES_Utilities.GetVersion()), Severity.DEBUG);

            if (file.isFileEncryptable())
            {
                _file = file;
                _password = password;
                _passwordHint = passwordHint;
                _deletePost = deleteAfterEncrypt;
                _overwriteDuplicate = overwriteDuplicate;
                compress = new Compress(compression);
                if (UserSpecifiedSalt != null) crypt.SetUserSalt(UserSpecifiedSalt);
            }
            else throw new Exception("This filetype cannot be encrypted!");
        }

        /// <summary>
        /// Sets whether the original file/folder should be deleted after a successful encryption
        /// </summary>
        /// <param name="delete">If the file/folder should be deleted</param>
        public void SetDeleteAfterEncrypt(bool delete)
        {
            _deletePost = delete;
        }

        /// <summary>
        /// Gets whether the original file/folder will be deleted after a successful encryption
        /// </summary>
        /// <returns>If the file/folder will be deleted</returns>
        public bool GetDeleteAfterEncrypt()
        {
            return _deletePost;
        }

        /// <summary>
        /// Sets whether duplicate files should be forcibly overwritten
        /// </summary>
        /// <param name="overwrite">If duplicate files should be overwritten</param>
        public void SetOverwriteDuplicate(bool overwrite)
        {
            _overwriteDuplicate = overwrite;
        }

        /// <summary>
        /// Gets whether duplicate files will be forcibly overwritten
        /// </summary>
        /// <returns>If duplicate files will be overwritten</returns>
        public bool GetOverwriteDuplicate()
        {
            return _overwriteDuplicate;
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
        /// Gets the percent completion of the encryption process
        /// </summary>
        /// <returns>Percent complete (0-100)</returns>
        public decimal GetEncryptionPercentComplete()
        {
            return _percentEncComplete;
        }

        /// <summary>
        /// Encrypts current file
        /// </summary>
        /// <returns>If the encryption was successful</returns>
        public bool encryptFile()
        {
            string file;
            byte[] fileHash;
            bool success = false;
            _percentEncComplete = 0;

            try
            {
                try
                {
                    Logging.Log(String.Format("Getting File Hash: {0}", _file.getPath()), Severity.DEBUG);
                    
                    switch (_checksumType)
                    {
                        case Checksums.ChecksumType.SHA1:
                            fileHash = Checksums.GetSHA1(_file.getPath());
                            break;
                        case Checksums.ChecksumType.SHA256:
                            fileHash = Checksums.GetSHA256(_file.getPath());
                            break;
                        case Checksums.ChecksumType.SHA512:
                            fileHash = Checksums.GetSHA512(_file.getPath());
                            break;
                        default:
                            fileHash = Checksums.GetSHA256(_file.getPath());
                            break;
                    }

                    Logging.Log(String.Format("Starting Compression: {0}", _file.getPath()), Severity.DEBUG);
                    file = compress.CompressFAESFile(_file);
                    Logging.Log(String.Format("Finished Compression: {0}", _file.getPath()), Severity.DEBUG);
                }
                catch
                {
                    throw new IOException("Error occured in creating the UFAES file.");
                }

                try
                {
                    Logging.Log(String.Format("Starting Encryption: {0}", file), Severity.DEBUG);

                    _faesMetaData = new MetaData(_checksumType, fileHash, _passwordHint, compress.GetCompressionModeAsString(), Path.GetFileName(_file.getPath()));

                    success = crypt.Encrypt(_faesMetaData.GetMetaData(), file, Path.ChangeExtension(file, FileAES_Utilities.ExtentionFAES), _password, ref _percentEncComplete);

                    Logging.Log(String.Format("Finished Encryption: {0}", file), Severity.DEBUG);
                }
                catch
                {
                    throw new IOException("Error occured in encrypting the UFAES file.");
                }
                
                try
                {
                    FileAES_IntUtilities.SafeDeleteFile(file);
                }
                catch
                {
                    throw new IOException("Error occured in deleting the UFAES file.");
                }

                string faesInputPath = Path.ChangeExtension(file, FileAES_Utilities.ExtentionFAES);
                string faesOutputPath = Path.Combine(Directory.GetParent(_file.getPath()).FullName, Path.ChangeExtension(_file.getFileName(), FileAES_Utilities.ExtentionFAES));

                if (File.Exists(faesOutputPath) && _overwriteDuplicate) FileAES_IntUtilities.SafeDeleteFile(faesOutputPath);
                else if (File.Exists(faesOutputPath)) throw new IOException("Error occured since the file already exists.");

                try
                {
                    File.SetAttributes(faesInputPath, FileAttributes.Encrypted);
                    File.Move(faesInputPath, faesOutputPath);
                }
                catch
                {
                    throw new IOException("Error occured in moving the FAES file after encryption.");
                }

                try
                {
                    if (_deletePost)
                    {
                        if (_file.isFile()) FileAES_IntUtilities.SafeDeleteFile(_file.getPath());
                        else FileAES_IntUtilities.SafeDeleteFolder(_file.getPath(), true);
                    }
                }
                catch (UnauthorizedAccessException e)
                {
                    throw new UnauthorizedAccessException(e.Message);
                }
                catch
                {
                    throw new IOException("Error occured while deleting the original file/folder.");
                }

            }
            finally
            {
                FileAES_Utilities.PurgeInstancedTempFolders();
            }
            return success;
        }
    }

    public class FileAES_Decrypt
    {
        protected FAES_File _file;
        protected string _password;
        protected bool _deletePost, _overwriteDuplicate;
        protected decimal _percentDecComplete = 0;
        protected MetaData _faesMetaData;

        internal Crypt crypt = new Crypt();
        internal Compress compress = new Compress(Optimise.Balanced);

        /// <summary>
        /// Decrypts a selected FAES File using a password
        /// </summary>
        /// <param name="file">Decryptable FAES File</param>
        /// <param name="password">Password to decrypt file</param>
        /// <param name="deleteAfterDecrypt">Whether the original file/folder should be deleted after a successful decryption</param>
        /// <param name="overwriteDuplicate">Whether duplicate files should be forcibly overwritten</param>
        public FileAES_Decrypt(FAES_File file, string password, bool deleteAfterDecrypt = true, bool overwriteDuplicate = true)
        {
            Logging.Log(String.Format("FAES {0} started!", FileAES_Utilities.GetVersion()), Severity.DEBUG);

            if (file.isFileDecryptable())
            {
                _file = file;
                _password = password;
                _deletePost = deleteAfterDecrypt;
                _overwriteDuplicate = overwriteDuplicate;
                _faesMetaData = new MetaData(_file);
            }
            else throw new Exception("This filetype cannot be decrypted!");
        }

        /// <summary>
        /// Sets whether the original file/folder should be deleted after a successful decryption
        /// </summary>
        /// <param name="delete">If the file/folder should be deleted</param>
        public void SetDeleteAfterDecrypt(bool delete)
        {
            _deletePost = delete;
        }

        /// <summary>
        /// Gets whether the original file/folder will be deleted after a successful decryption
        /// </summary>
        /// <returns>If the file/folder will be deleted</returns>
        public bool GetDeleteAfterDecrypt()
        {
            return _deletePost;
        }

        /// <summary>
        /// Sets whether duplicate files should be forcibly overwritten
        /// </summary>
        /// <param name="overwrite">If duplicate files should be overwritten</param>
        public void SetOverwriteDuplicate(bool overwrite)
        {
            _overwriteDuplicate = overwrite;
        }

        /// <summary>
        /// Gets whether duplicate files will be forcibly overwritten
        /// </summary>
        /// <returns>If duplicate files will be overwritten</returns>
        public bool GetOverwriteDuplicate()
        {
            return _overwriteDuplicate;
        }

        /// <summary>
        /// Gets the percent completion of the decryption process
        /// </summary>
        /// <returns>Percent complete (0-100)</returns>
        public decimal GetDecryptionPercentComplete()
        {
            return _percentDecComplete;
        }

        /// <summary>
        /// Decrypts current file
        /// </summary>
        /// <returns>If the decryption was successful</returns>
        public bool decryptFile()
        {
            bool success;
            _percentDecComplete = 0;

            string fileInputPath = _file.getPath();
            string fileOutputPath = Path.ChangeExtension(_file.getPath(), FileAES_Utilities.ExtentionUFAES);

            if (!String.IsNullOrEmpty(_file.GetOriginalFileName()))
            {
                string fileOverwritePath = Path.Combine(Path.GetDirectoryName(_file.getPath()), _file.GetOriginalFileName());

                if (File.Exists(fileOverwritePath) && !String.IsNullOrWhiteSpace(_file.GetOriginalFileName()) && _overwriteDuplicate)
                    File.Delete(fileOverwritePath);
                else if (File.Exists(fileOverwritePath))
                    throw new IOException("Error occured since the file already exists.");
            }
            else Logging.Log(String.Format("Could not find the original filename for '{0}' (). This may cause some problems if the decrypted file(s) already exist in this location!", _file.getFileName()), Severity.WARN);

            try
            {
                Logging.Log(String.Format("Starting Decryption: {0}", _file.getPath()), Severity.DEBUG);


                if (!_faesMetaData.IsLegacyVersion())
                {
                    success = crypt.Decrypt(_faesMetaData, _file.getPath(), fileOutputPath, _password, ref _percentDecComplete);
                }
                else
                {
                    Logging.Log(String.Format("Using Compatability Decryption: <=FAESv2 file detected!", _file.getPath()), Severity.DEBUG);
                    success = new LegacyCrypt().Decrypt(_file.getPath(), _password, ref _percentDecComplete);
                }
                Logging.Log(String.Format("Finished Decryption: {0}", _file.getPath()), Severity.DEBUG);
            }
            catch
            {
                throw new IOException("Error occured in the decryption of the FAES file.");
            }

            File.SetAttributes(Path.ChangeExtension(_file.getPath(), FileAES_Utilities.ExtentionUFAES), FileAttributes.Hidden);

            if (success)
            {
                try
                {
                    Logging.Log(String.Format("Starting Uncompression: {0}", Path.ChangeExtension(_file.getPath(), FileAES_Utilities.ExtentionUFAES)), Severity.DEBUG);
                    compress.UncompressFAESFile(_file);
                    Logging.Log(String.Format("Finished Uncompression: {0}", Path.ChangeExtension(_file.getPath(), FileAES_Utilities.ExtentionUFAES)), Severity.DEBUG);
                }
                catch
                {
                    throw new IOException("Error occured in extracting the UFAES file.");
                }

                try
                {
                    File.SetAttributes(Directory.GetParent(_file.getPath()).FullName, FileAttributes.Normal);
                    FileAES_IntUtilities.SafeDeleteFile(Path.Combine(Directory.GetParent(_file.getPath()).FullName, _file.getFileName().Substring(0, _file.getFileName().Length - Path.GetExtension(_file.getFileName()).Length) + FileAES_Utilities.ExtentionUFAES));
                }
                catch
                {
                    throw new IOException("Error occured in deleting the UFAES file.");
                }

                try
                {
                    if (_deletePost) FileAES_IntUtilities.SafeDeleteFile(_file.getPath());
                }
                catch
                {
                    throw new IOException("Error occured while deleting the original file/folder.");
                }
            }

            FileAES_IntUtilities.SafeDeleteFile(Path.ChangeExtension(_file.getPath(), FileAES_Utilities.ExtentionUFAES.Replace(".", "")));

            return success;
        }

        /// <summary>
        /// Gets the Password Hint for the current file
        /// </summary>
        /// <returns>Current files Password Hint</returns>
        public string getPasswordHint()
        {
            return _faesMetaData.GetPasswordHint();
        }
    }

    public class FileAES_Utilities
    {
        public static string ExtentionFAES = ".faes";
        public static string ExtentionUFAES = ".ufaes";

        private const bool IsPreReleaseBuild = true;
        private const string PreReleaseTag = "BETA_5";

        private static string[] _supportedEncExtentions = new string[3] { ExtentionFAES, ".faes", ".mcrypt" };
        private static string _FileAES_TempRoot = Path.Combine(Path.GetTempPath(), "FileAES");
        private static bool _verboseLogging = false;
        private static uint _cryptoBuffer = 1048576;

        internal static List<TempPath> _instancedTempFolders = new List<TempPath>();

        /// <summary>
        /// Overrides the default extentions used by FAES. Useful if you are using FAES in a specialised environment
        /// </summary>
        /// <param name="encryptedFAES">Extension of the final, encrypted file</param>
        /// <param name="unencryptedFAES">Extension of the compressed, but not encrypted, file</param>
        /// <param name="limitSupportedExtensions">Limits the supported encryption file extensions to only the provided one</param>
        public static void OverrideDefaultExtentions(string encryptedFAES, string unencryptedFAES, bool limitSupportedExtensions = false)
        {
            ExtentionFAES = encryptedFAES;
            ExtentionUFAES = unencryptedFAES;

            if (limitSupportedExtensions)
                _supportedEncExtentions = new string[1] { ExtentionFAES };
            else
                _supportedEncExtentions = new string[3] { ExtentionFAES, ".faes", ".mcrypt" };
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
        public static bool isFileEncryptable(string filePath)
        {
            return !_supportedEncExtentions.Any(Path.GetExtension(filePath).Contains);
        }
        /// <summary>
        /// Gets if the chosen file is decryptable
        /// </summary>
        /// <param name="filePath">Chosen File</param>
        /// <returns>If the file is decryptable</returns>
        public static bool isFileDecryptable(string filePath)
        {
            return (_supportedEncExtentions.Any(Path.GetExtension(filePath).Contains) && new MetaData(filePath).IsDecryptable(filePath));
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
                Logging.Log(String.Format("Purged FAES Temp Folder: {0}", GetFaesTempFolder()), Severity.DEBUG);
            }
        }

        /// <summary>
        /// Remove InstancedTempFolder with specific path
        /// </summary>
        /// <param name="tempPath">Remove selected path from InstancedTempFolders</param>
        /// <returns>If a value was successfully removed</returns>
        public static bool RemoveInstancedTempFolder(string tempPath)
        {
            TempPath tmp = _instancedTempFolders.Where(tPath => tPath.GetTempPath() == tempPath).First();

            return RemoveInstancedTempPath(tmp);
        }

        /// <summary>
        /// Remove InstancedTempFolder with specific FAES_File filename
        /// </summary>
        /// <param name="tempPath">Remove selected FAES_File filename from InstancedTempFolders</param>
        /// <returns>If a value was successfully removed</returns>
        public static bool RemoveInstancedTempFolder(FAES_File faesFile)
        {
            TempPath tmp = _instancedTempFolders.Where(tPath => tPath.GetFaesFile().getFileName() == faesFile.getFileName()).First();

            return RemoveInstancedTempPath(tmp);
        }

        private static bool RemoveInstancedTempPath(TempPath tmp)
        {
            if (tmp != null)
            {
                _instancedTempFolders.Remove(tmp);
                Logging.Log(String.Format("Deleted InstancedTempFolder: {0}", tmp.GetTempPath()), Severity.DEBUG);

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
                    Logging.Log(String.Format("Deleted InstancedTempFolder[{0}]: {1}", totalDeleted, pTempPath), Severity.DEBUG);
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
            if (!IsPreReleaseBuild)
                return "v" + ver[0] + "." + ver[1] + "." + ver[2];
            else
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
        /// <returns>Human-Readable Error</returns>
        public static string FAES_ExceptionHandling(Exception exception, bool showRawException = false)
        {
            if (!showRawException)
            {
                if (exception.ToString().Contains("Error occured in creating the UFAES file."))
                    return "ERROR: The chosen file(s) could not be compressed as a compressed version already exists in the Temp files! Consider using '--purgeTemp' if you are not using another instance of FileAES and this error persists.";
                else if (exception.ToString().Contains("Error occured in encrypting the UFAES file."))
                    return "ERROR: The compressed file could not be encrypted. Please close any other instances of FileAES and try again. Consider using '--purgeTemp' if you are not using another instance of FileAES and this error persists.";
                else if (exception.ToString().Contains("Error occured in the decryption of the FAES file."))
                    return "ERROR: The encrypted file could not be decrypted. Please close any other instances of FileAES and try again. Consider using '--purgeTemp' if you are not using another instance of FileAES and this error persists.";
                else if (exception.ToString().Contains("Error occured in deleting the UFAES file."))
                    return "ERROR: The compressed file could not be deleted. Please close any other instances of FileAES and try again. Consider using '--purgeTemp' if you are not using another instance of FileAES and this error persists.";
                else if (exception.ToString().Contains("Error occured in moving the FAES file after encryption."))
                    return "ERROR: The encrypted file could not be moved to the original destination! Please ensure that a file with the same name does not already exist.";
                else if (exception.ToString().Contains("Error occured while deleting the original file/folder."))
                    return "ERROR: The original file/folder could not be deleted after encryption! Please ensure that they are not in-use!";
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
                else if (exception.ToString().Contains("Error occured in SafeDeleteFile."))
                    return "ERROR: A file could not be deleted! Is the file in use?";
                else if (exception.ToString().Contains("Error occured in SafeDeleteFolder."))
                    return "ERROR: A folder could not be deleted! Is the folder in use?";
                else if (exception.ToString().Contains("File/Folder not found at the specified path!"))
                    return "ERROR: A file/folder was not found at the specified path!";
                else if (exception.ToString().Contains("Error occured since the file already exists."))
                    return "ERROR: File already exists at destination and overwriting is disabled!";
            }
            return exception.ToString();
        }
    }

    internal class FileAES_IntUtilities
    {
        internal static Random rand = new Random();

        internal static string GetDateTimeString()
        {
            return DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds.ToString();
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
                    Logging.Log(String.Format("SafeDeleteFile: {0}", path), Severity.DEBUG);

                    return true;
                }
                catch
                {
                    throw new UnauthorizedAccessException("Error occured in SafeDeleteFile. File cannot be deleted!");
                }
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
                try
                {
                    Directory.Delete(path, recursive);
                    Logging.Log(String.Format("SafeDeleteFolder: {0}", path), Severity.DEBUG);

                    return true;
                }
                catch
                {
                    throw new UnauthorizedAccessException("Error occured in SafeDeleteFolder. Folder cannot be deleted!");
                }
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
        internal static string CreateTempPath(FAES_File file, string InstanceFolder)
        {
            string dateTime = GetDateTimeString();
            string tempPath = Path.Combine(FileAES_Utilities.GetFaesTempFolder(), dateTime, InstanceFolder);

            if (FileAES_Utilities._instancedTempFolders.Where(tPath => tPath.GetFaesFile().getFileName() == file.getFileName()).Count() == 0)
            {
                tempPath = Path.Combine(tempPath, dateTime);
                FileAES_Utilities._instancedTempFolders.Add(new TempPath(file, Path.Combine(FileAES_Utilities.GetFaesTempFolder(), dateTime)));

                Logging.Log(String.Format("Created TempPath: {0}", tempPath), Severity.DEBUG);
            }
            else
            {
                tempPath = FileAES_Utilities._instancedTempFolders.Where(tPath => tPath.GetFaesFile().getFileName() == file.getFileName()).First().GetTempPath();
            }

            if (!Directory.Exists(tempPath)) Directory.CreateDirectory(tempPath);

            return tempPath;
        }

        /// <summary>
        /// Creates a new temp path and adds it to the instancedTempFolders list
        /// </summary>
        internal static string CreateTempPath(FAES_File file)
        {
            return CreateTempPath(file, GetDateTimeString());
        }
    }

    internal class TempPath
    {
        protected FAES_File _faesFile;
        protected string _tempPath;

        /// <summary>
        /// Creates a TempPath
        /// </summary>
        /// <param name="faesFile">FAES File to link to the TempPath</param>
        /// <param name="tempPath">Actual path to link to the TempPath</param>
        internal TempPath(FAES_File faesFile, string tempPath)
        {
            _faesFile = faesFile;
            _tempPath = tempPath;
        }

        /// <summary>
        /// Gets the FAES File linked to the TempPath
        /// </summary>
        /// <returns>Linked FAES File</returns>
        internal FAES_File GetFaesFile()
        {
            return _faesFile;
        }

        /// <summary>
        /// Gets the actual path linked to the TempPath
        /// </summary>
        /// <returns>Temp path</returns>
        internal string GetTempPath()
        {
            return _tempPath;
        }
    }
}