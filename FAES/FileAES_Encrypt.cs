using System;
using System.IO;
using FAES.AES;
using FAES.Packaging;

namespace FAES
{
    public class FileAES_Encrypt
    {
        protected FAES_File _file;
        protected string _password, _passwordHint;
        protected bool _deletePost, _overwriteDuplicate;
        protected decimal _percentEncComplete, _percentCompComplete;
        protected MetaData _faesMetaData;
        protected Checksums.ChecksumType _checksumType = Checksums.ChecksumType.SHA256;

        protected bool _debugMode;

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
            Logging.Log($"FAES {FileAES_Utilities.GetVersion()} started!", Severity.DEBUG);

            if (file.IsFileEncryptable())
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
        /// Shows full exception messages if FAES fails for whatever reason
        /// </summary>
        public bool DebugMode
        {
            get => _debugMode;
            set => _debugMode = value;
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
        /// Set the type of hash algorithm to use for the file checksum
        /// </summary>
        /// <param name="checksumType">Checksum Type</param>
        public void SetChecksumType(Checksums.ChecksumType checksumType)
        {
            _checksumType = checksumType;
        }

        /// <summary>
        /// Get the type of hash algorithm to use for the file checksum
        /// </summary>
        /// <returns>Checksum Type</returns>
        public Checksums.ChecksumType GetChecksumType()
        {
            return _checksumType;
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
        public void SetCompressionMode(CompressionMode compressionMode, CompressionLevel compressionLevel)
        {
            compress = new Compress(compressionMode, compressionLevel);
        }

        /// <summary>
        /// Set the compression method used for creating the .UFAES file
        /// </summary>
        /// <param name="compressionMode">Compression Mode to use</param>
        /// <param name="compressionLevel">Raw Compression Level to use</param>
        public void SetCompressionMode(CompressionMode compressionMode, int compressionLevel)
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
        /// Gets the percent completion of the decompression process
        /// </summary>
        /// <returns>Percent complete (0-100)</returns>
        public decimal GetDecompressionPercentComplete()
        {
            return _percentCompComplete;
        }

        /// <summary>
        /// Gets the percent completion of the entire FAES process
        /// </summary>
        /// <returns>Percent complete (0-100)</returns>
        public decimal GetPercentComplete()
        {
            return (_percentEncComplete / 2) + (_percentCompComplete / 2);
        }

        /// <summary>
        /// Encrypts current file
        /// </summary>
        /// <returns>If the encryption was successful</returns>
        public bool EncryptFile()
        {
            bool success;
            _percentEncComplete = 0;

            try
            {
                string file;
                byte[] fileHash;

                try
                {
                    Logging.Log($"Starting Compression: {_file.GetPath()}", Severity.DEBUG);
                    file = compress.CompressFAESFile(_file, ref _percentCompComplete);
                    Logging.Log($"Finished Compression: {_file.GetPath()}", Severity.DEBUG);

                    Logging.Log($"Getting File Hash: {file}", Severity.DEBUG);

                    switch (_checksumType)
                    {
                        case Checksums.ChecksumType.SHA1:
                            fileHash = Checksums.GetSHA1(file);
                            break;
                        case Checksums.ChecksumType.SHA256:
                            fileHash = Checksums.GetSHA256(file);
                            break;
                        case Checksums.ChecksumType.SHA384:
                            fileHash = Checksums.GetSHA384(file);
                            break;
                        case Checksums.ChecksumType.SHA512:
                            fileHash = Checksums.GetSHA512(file);
                            break;
                        default:
                            fileHash = Checksums.GetSHA256(file);
                            break;
                    }
                }
                catch (Exception)
                {
                    if (!_debugMode)
                        throw new IOException("Error occurred in creating the UFAES file.");
                    else throw;
                }

                try
                {
                    Logging.Log($"Starting Encryption: {file}", Severity.DEBUG);

                    _faesMetaData = new MetaData(_checksumType, fileHash, _passwordHint, compress.GetCompressionModeAsString(), Path.GetFileName(_file.GetPath()));

                    success = crypt.Encrypt(_faesMetaData.GetMetaData(), file, Path.ChangeExtension(file, FileAES_Utilities.ExtentionFAES), _password, ref _percentEncComplete);

                    Logging.Log($"Finished Encryption: {file}", Severity.DEBUG);
                }
                catch (Exception)
                {
                    if (!_debugMode)
                        throw new IOException("Error occurred in encrypting the UFAES file.");
                    else throw;
                }

                try
                {
                    FileAES_IntUtilities.SafeDeleteFile(file);
                }
                catch (Exception)
                {
                    if (!_debugMode)
                        throw new IOException("Error occurred in deleting the UFAES file.");
                    else throw;
                }

                string faesInputPath = Path.ChangeExtension(file, FileAES_Utilities.ExtentionFAES);
                string faesOutputPath = Path.Combine(Directory.GetParent(_file.GetPath())?.FullName ??
                                                     throw new InvalidOperationException("An unexpected error occurred when creating an encryption path!"),
                                                     Path.ChangeExtension(_file.GetFileName(), FileAES_Utilities.ExtentionFAES));

                if (File.Exists(faesOutputPath) && _overwriteDuplicate) FileAES_IntUtilities.SafeDeleteFile(faesOutputPath);
                else if (File.Exists(faesOutputPath)) throw new IOException("Error occurred since the file already exists.");

                try
                {
                    File.SetAttributes(faesInputPath, FileAttributes.Encrypted);
                    File.Move(faesInputPath, faesOutputPath);
                }
                catch (Exception)
                {
                    if (!_debugMode)
                        throw new IOException("Error occurred in moving the FAES file after encryption.");
                    else throw;
                }

                try
                {
                    if (_deletePost)
                    {
                        if (_file.IsFile()) FileAES_IntUtilities.SafeDeleteFile(_file.GetPath());
                        else FileAES_IntUtilities.SafeDeleteFolder(_file.GetPath());
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    throw;
                }
                catch (Exception)
                {
                    if (!_debugMode)
                        throw new IOException("Error occurred while deleting the original file/folder.");
                    else throw;
                }
            }
            finally
            {
                FileAES_Utilities.RemoveInstancedTempFolder(_file);
            }
            return success;
        }
        #region Obsolete Methods
        /// <summary>
        /// Encrypts current file
        /// </summary>
        /// <returns>If the encryption was successful</returns>
        [Obsolete("encryptFile() has been renamed to EncryptFile()")]
        public bool encryptFile()
        {
            return EncryptFile();
        }
        #endregion
    }
}
