using System;
using System.IO;
using FAES.AES;
using FAES.AES.Compatibility;
using FAES.Packaging;

namespace FAES
{
    public class FileAES_Decrypt
    {
        protected FAES_File _file;
        protected string _password;
        protected bool _deletePost, _overwriteDuplicate;
        protected decimal _percentDecComplete, _percentDecompComplete;
        protected MetaData _faesMetaData;

        protected bool _debugMode;

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
            Logging.Log($"FAES {FileAES_Utilities.GetVersion()} started!", Severity.DEBUG);

            if (file.IsFileDecryptable())
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
        /// Shows full exception messages if FAES fails for whatever reason
        /// </summary>
        public bool DebugMode
        {
            get => _debugMode;
            set => _debugMode = value;
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
        /// Gets the percent completion of the decompression process
        /// </summary>
        /// <returns>Percent complete (0-100)</returns>
        public decimal GetDecompressionPercentComplete()
        {
            return _percentDecompComplete;
        }

        /// <summary>
        /// Gets the percent completion of the entire FAES process
        /// </summary>
        /// <returns>Percent complete (0-100)</returns>
        public decimal GetPercentComplete()
        {
            return (_percentDecComplete / 2) + (_percentDecompComplete / 2);
        }

        /// <summary>
        /// Decrypts current file
        /// </summary>
        /// <returns>If the decryption was successful</returns>
        public bool DecryptFile(string pathOverride = "")
        {
            bool success;
            string fileOutputPath;
            _percentDecComplete = 0;
            _percentDecompComplete = 0;

            if (String.IsNullOrWhiteSpace(pathOverride))
            {
                fileOutputPath = Path.ChangeExtension(_file.GetPath(), FileAES_Utilities.ExtentionUFAES);
            }
            else
            {
                fileOutputPath = pathOverride;
            }

            if (!String.IsNullOrEmpty(_file.GetOriginalFileName()))
            {
                string fileOverwritePath = Path.ChangeExtension(fileOutputPath, Path.GetExtension(_file.GetOriginalFileName()));

                if (File.Exists(fileOverwritePath) && !String.IsNullOrWhiteSpace(_file.GetOriginalFileName()) && _overwriteDuplicate)
                    File.Delete(fileOverwritePath);
                else if (File.Exists(fileOverwritePath))
                    throw new IOException("Error occurred since the file already exists.");
            }
            else Logging.Log($"Could not find the original filename for '{_file.GetFileName()}'. This may cause some problems if the decrypted file(s) already exist in this location!", Severity.WARN);

            try
            {
                Logging.Log($"Starting Decryption: {_file.GetPath()}", Severity.DEBUG);

                if (!_faesMetaData.IsLegacyVersion())
                {
                    success = crypt.Decrypt(_faesMetaData, _file.GetPath(), fileOutputPath, _password, ref _percentDecComplete);
                }
                else
                {
                    Logging.Log("Using Compatibility Decryption: <=FAESv2 file detected!", Severity.DEBUG);
                    success = new LegacyCrypt().Decrypt(_file.GetPath(), _password, ref _percentDecComplete);
                }
                Logging.Log($"Finished Decryption: {_file.GetPath()}", Severity.DEBUG);
            }
            catch (Exception)
            {
                if (!_debugMode)
                    throw new IOException("Error occurred in the decryption of the FAES file.");
                throw;
            }

            File.SetAttributes(fileOutputPath, FileAttributes.Hidden);

            if (success)
            {
                string decompFileName;
                try
                {
                    Logging.Log($"Starting Decompression: {_file.GetPath()}", Severity.DEBUG);
                    decompFileName = compress.DecompressFAESFile(_file, ref _percentDecompComplete, pathOverride);
                    Logging.Log($"Finished Decompression: {decompFileName}", Severity.DEBUG);
                }
                catch (Exception)
                {
                    if (!_debugMode)
                        throw new IOException("Error occurred in extracting the UFAES file.");
                    throw;
                }

                try
                {
                    File.SetAttributes(decompFileName, FileAttributes.Normal);
                    FileAES_IntUtilities.SafeDeleteFile(fileOutputPath);
                }
                catch (Exception)
                {
                    if (!_debugMode)
                        throw new IOException("Error occurred in deleting the UFAES file.");
                    throw;
                }

                try
                {
                    if (_deletePost) FileAES_IntUtilities.SafeDeleteFile(_file.GetPath());
                }
                catch (Exception)
                {
                    if (!_debugMode)
                        throw new IOException("Error occurred while deleting the original file/folder.");
                    throw;
                }
            }

            FileAES_IntUtilities.SafeDeleteFile(Path.ChangeExtension(_file.GetPath(), FileAES_Utilities.ExtentionUFAES.Replace(".", "")));

            return success;
        }

        #region Obsolete Methods
        /// <summary>
        /// Decrypts current file
        /// </summary>
        /// <returns>If the decryption was successful</returns>
        [Obsolete("decryptFile() has been renamed to DecryptFile()")]
        public bool decryptFile(string pathOverride = "")
        {
            return DecryptFile(pathOverride);
        }

        /// <summary>
        /// Gets the Password Hint for the current file
        /// </summary>
        /// <returns>Current files Password Hint</returns>
        [Obsolete("getPasswordHint() has been renamed to GetPasswordHint()")]
        public string GetPasswordHint()
        {
            return _faesMetaData.GetPasswordHint();
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
        #endregion
    }
}
