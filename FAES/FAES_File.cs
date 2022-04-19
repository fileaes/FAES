using System;
using System.IO;
using FAES.AES;

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
            IsFile();
            IsFileDecryptable();
            GetFileName();
            GetPath();
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
                    Logging.Log($"Encrypting '{_filePath}'...");
                    FileAES_Encrypt encrypt = new FileAES_Encrypt(new FAES_File(_filePath), _password, _passwordHint);
                    success = encrypt.encryptFile();
                }
                else if (isFileDecryptable())
                {
                    Logging.Log($"Decrypting '{_filePath}'...");
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
        public bool IsFileEncryptable()
        {
            if (_op == Operation.NULL) IsFileDecryptable();

            return (_op == Operation.ENCRYPT);
        }

        /// <summary>
        /// Gets if the chosen FAES File is encryptable
        /// </summary>
        /// <returns>If the current FAES File is encryptable</returns>
        [Obsolete("isFileEncryptable() has been renamed to IsFileEncryptable()")]
        public bool isFileEncryptable()
        {
            return IsFileEncryptable();
        }

        /// <summary>
        /// Gets if the chosen FAES File is decryptable
        /// </summary>
        /// <returns>If the current FAES File is decryptable</returns>
        public bool IsFileDecryptable()
        {
            if (_op == Operation.NULL)
            {
                if (FileAES_Utilities.IsFileDecryptable(GetPath()))
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
        /// Gets if the chosen FAES File is decryptable
        /// </summary>
        /// <returns>If the current FAES File is decryptable</returns>
        [Obsolete("isFileDecryptable() has been renamed to IsFileDecryptable()")]
        public bool isFileDecryptable()
        {
            return IsFileDecryptable();
        }

        /// <summary>
        /// Gets the selected hash of the current FAES File
        /// </summary>
        /// <param name="hashType">Type of hash</param>
        /// <returns>Selected hash of FAES File</returns>
        public string GetFileHash(Checksums.ChecksumType hashType)
        {
            if (IsFile())
            {
                switch (hashType)
                {
                    case Checksums.ChecksumType.SHA1:
                        return Checksums.ConvertHashToString(Checksums.GetSHA1(GetPath()));

                    case Checksums.ChecksumType.SHA256:
                        return Checksums.ConvertHashToString(Checksums.GetSHA256(GetPath()));

                    case Checksums.ChecksumType.SHA512:
                        return Checksums.ConvertHashToString(Checksums.GetSHA512(GetPath()));

                    default:
                        return null;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the SHA1 hash of the selected FAES File
        /// </summary>
        /// <returns>SHA1 Hash of FAES File</returns>
        [Obsolete("This method of getting a files hash is deprecated. Please use GetFileHash.")]
        public string getSHA1()
        {
            return GetFileHash(Checksums.ChecksumType.SHA1);
        }

        /// <summary>
        /// Gets the filename of the selected FAES File
        /// </summary>
        /// <returns>Filename of FAES File</returns>
        public string GetFileName()
        {
            if (_fileName == null) _fileName = Path.GetFileName(GetPath());
            return _fileName;
        }

        /// <summary>
        /// Gets the filename of the selected FAES File
        /// </summary>
        /// <returns>Filename of FAES File</returns>
        [Obsolete("getFileName() has been renamed to GetFileName()")]
        public string getFileName()
        {
            return GetFileName();
        }

        /// <summary>
        /// Gets if the current FAES File is a folder
        /// </summary>
        /// <returns>If the FAES File is a folder</returns>
        public bool IsFolder()
        {
            if (_type == FAES_Type.NULL)
            {
                FileAttributes attr = File.GetAttributes(GetPath());

                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                    _type = FAES_Type.FOLDER;
                else
                    _type = FAES_Type.FILE;
            }
            return (_type == FAES_Type.FOLDER);
        }

        /// <summary>
        /// Gets if the current FAES File is a folder
        /// </summary>
        /// <returns>If the FAES File is a folder</returns>
        [Obsolete("isFolder() has been renamed to IsFolder()")]
        public bool isFolder()
        {
            return IsFolder();
        }

        /// <summary>
        /// Gets if the current FAES File is a file
        /// </summary>
        /// <returns>Gets if the current FAES File is a file</returns>
        public bool IsFile()
        {
            return !IsFolder();
        }

        /// <summary>
        /// Gets if the current FAES File is a file
        /// </summary>
        /// <returns>Gets if the current FAES File is a file</returns>
        [Obsolete("isFile() has been renamed to IsFile()")]
        public bool isFile()
        {
            return IsFile();
        }

        /// <summary>
        /// Gets the path of the selected FAES File
        /// </summary>
        /// <returns>Path of FAES File</returns>
        public string GetPath()
        {
            if (_fullPath == null) _fullPath = Path.GetFullPath(_filePath);
            return _fullPath;
        }

        /// <summary>
        /// Gets the path of the selected FAES File
        /// </summary>
        /// <returns>Path of FAES File</returns>
        [Obsolete("getPath() has been renamed to GetPath()")]
        public string getPath()
        {
            return GetPath();
        }

        /// <summary>
        /// Gets the path of the selected FAES File
        /// </summary>
        /// <returns>Path of FAES File</returns>
        public override string ToString()
        {
            return GetPath();
        }

        /// <summary>
        /// Gets the FAES Type of the FAES File
        /// </summary>
        /// <returns>FAES Type of FAES File</returns>
        public string getFaesType()
        {
            if (_type == FAES_Type.FILE) return "File";
            return "Folder";
        }

        /// <summary>
        /// Gets the appropriate Operation/Action for the FAES File
        /// </summary>
        /// <returns>Appropriate Operation/Action for the FAES File</returns>
        public string getOperation()
        {
            if (_op == Operation.ENCRYPT) return "Encrypt";
            return "Decrypt";
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

            return _passwordHint;
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

            throw new NotSupportedException("Cannot read MetaData of an unencrypted file.");
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

            throw new NotSupportedException("Cannot read MetaData of an unencrypted file.");
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

            throw new NotSupportedException("Cannot read MetaData of an unencrypted file.");
        }

        /// <summary>
        /// Get the original filename (of the unencrypted file)
        /// </summary>
        /// <returns>Original Filename</returns>
        public string GetOriginalFileName()
        {
            if (_faesMetaData != null)
            {
                return _faesMetaData.GetOriginalFileName();
            }

            throw new NotSupportedException("Cannot read MetaData of an unencrypted file.");
        }

        /// <summary>
        /// Gets the hash type used to hash the ufaes file
        /// </summary>
        /// <returns>Checksum Type</returns>
        public Checksums.ChecksumType GetChecksumType()
        {
            if (_faesMetaData != null)
            {
                return _faesMetaData.GetHashType();
            }

            throw new NotSupportedException("Cannot read MetaData of an unencrypted file.");
        }
    }
}
