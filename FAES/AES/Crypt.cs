using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace FAES.AES
{
    internal class Crypt
    {
        private const int _keySize = 256;
        private const int _blockSize = 128;
        private const PaddingMode _paddingMode = PaddingMode.PKCS7;
        private const CipherMode _cipher = CipherMode.CBC;
        private const int _keyIterations = 51200;

        protected byte[] _specifiedSalt;

        /// <summary>
        /// FAES Encrypt/Decrypt Handler. Using a randomly generated salt.
        /// </summary>
        internal Crypt()
        { }

        /// <summary>
        /// FAES Encrypt/Decrypt Handler. Using a user-specified salt.
        /// </summary>
        /// <param name="salt">User-specified Salt</param>
        internal Crypt(byte[] salt)
        {
            _specifiedSalt = salt;
        }

        /// <summary>
        /// Sets the user specified salt.
        /// </summary>
        /// <param name="salt">User-specified salt</param>
        internal void SetUserSalt(byte[] salt)
        {
            _specifiedSalt = salt;
        }

        /// <summary>
        /// Gets the user specified salt.
        /// </summary>
        /// <returns>User-specified salt</returns>
        internal byte[] GetUserSalt()
        {
            return _specifiedSalt;
        }

        /// <summary>
        /// Removes the user specified salt and returns to using a randomly generated one each encryption.
        /// </summary>
        internal void RemoveUserSalt()
        {
            _specifiedSalt = null;
        }

        /// <summary>
        /// Gets if the user specified salt is active.
        /// </summary>
        /// <returns>If the user-specified salt is active</returns>
        internal bool IsUserSaltActive()
        {
            if (_specifiedSalt != null) return true;
            return false;
        }

        /// <summary>
        /// Encrypts the selected file using the given password
        /// </summary>
        /// <param name="metaData">Formatted Metadata used at the start of a file</param>
        /// <param name="inputFilePath">File path for unencrypted file</param>
        /// <param name="outputFilePath">File path for encrypted file</param>
        /// <param name="encryptionPassword">Encryption Password</param>
        /// <param name="percentComplete">Percent completion of the encryption process</param>
        /// <returns>If the encryption was successful</returns>
        internal bool Encrypt(byte[] metaData, string inputFilePath, string outputFilePath, string encryptionPassword, ref decimal percentComplete) // TODO: Diagnose Encryption Progression not updating till finished
        {
            byte[] salt = _specifiedSalt ?? CryptUtils.GenerateRandomSalt();
            byte[] passwordBytes = Encoding.UTF8.GetBytes(encryptionPassword);

            Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(passwordBytes, salt, _keyIterations);
            RijndaelManaged AES = new RijndaelManaged
            {
                KeySize = _keySize,
                BlockSize = _blockSize,
                Padding = _paddingMode,
                Key = key.GetBytes(_keySize / 8),
                IV = key.GetBytes(_blockSize / 8),
                Mode = _cipher
            };

            FileStream outputDataStream = new FileStream(outputFilePath, FileMode.Create);
            outputDataStream.Write(metaData, 0, metaData.Length);
            outputDataStream.Write(salt, 0, salt.Length);

            CryptoStream crypto = new CryptoStream(outputDataStream, AES.CreateEncryptor(), CryptoStreamMode.Write);
            FileStream inputDataStream = new FileStream(inputFilePath, FileMode.Open);

            byte[] buffer = new byte[FileAES_Utilities.GetCryptoStreamBuffer()];
            int read;

            long expectedComplete = metaData.Length + AES.KeySize + AES.BlockSize + inputDataStream.Length;

            Logging.Log("Beginning writing encrypted data...", Severity.DEBUG);
            while ((read = inputDataStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                try
                {
                    percentComplete = Math.Ceiling((decimal)((Convert.ToDouble(outputDataStream.Length) / Convert.ToDouble(expectedComplete)) * 100));
                    if (percentComplete > 100) percentComplete = 100;
                }
                catch
                {
                    // ignored
                }
                crypto.Write(buffer, 0, read);
            }
            Logging.Log("Finished writing encrypted data.", Severity.DEBUG);
            percentComplete = 100;

            inputDataStream.Close();
            crypto.Close();
            outputDataStream.Close();

            return true;
        }

        /// <summary>
        /// Decrypts the selected file using the given password
        /// </summary>
        /// <param name="faesMetaData">Formatted Metadata used at the start of a file</param>
        /// <param name="inputFilePath">File path for encrypted file</param>
        /// <param name="outputFilePath">File path for unencrypted file</param>
        /// <param name="encryptionPassword">Encryption Password</param>
        /// <param name="percentComplete">Percent completion of the encryption process</param>
        /// <returns>If the decryption was successful</returns>
        internal bool Decrypt(MetaData faesMetaData, string inputFilePath, string outputFilePath, string encryptionPassword, ref decimal percentComplete) // TODO: Diagnose Decryption Progression not updating till finished
        {
            byte[] metaData = new byte[faesMetaData.GetLength()];
            byte[] salt = new byte[32];
            byte[] passwordBytes = Encoding.UTF8.GetBytes(encryptionPassword);

            FileStream inputDataStream = new FileStream(inputFilePath, FileMode.Open);

            inputDataStream.Read(metaData, 0, faesMetaData.GetLength());
            inputDataStream.Read(salt, 0, salt.Length);

            Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(passwordBytes, salt, _keyIterations);
            RijndaelManaged AES = new RijndaelManaged
            {
                KeySize = _keySize,
                BlockSize = _blockSize,
                Key = key.GetBytes(_keySize / 8),
                IV = key.GetBytes(_blockSize / 8),
                Padding = _paddingMode,
                Mode = _cipher
            };

            try
            {
                CryptoStream crypto = new CryptoStream(inputDataStream, AES.CreateDecryptor(), CryptoStreamMode.Read);
                FileStream outputDataStream = new FileStream(outputFilePath, FileMode.Create);

                try
                {
                    byte[] buffer = new byte[FileAES_Utilities.GetCryptoStreamBuffer()];
                    long expectedComplete = salt.Length + AES.KeySize + AES.BlockSize + inputDataStream.Length;

                    try
                    {
                        Logging.Log("Beginning writing decrypted data...");
                        int read;
                        while ((read = crypto.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            try
                            {
                                percentComplete = Math.Ceiling((decimal)((Convert.ToDouble(outputDataStream.Length) / Convert.ToDouble(expectedComplete)) * 100));
                                if (percentComplete > 100) percentComplete = 100;
                            }
                            catch
                            {
                                // ignored
                            }

                            outputDataStream.Write(buffer, 0, read);
                        }
                        Logging.Log("Finished writing decrypted data.");
                        percentComplete = 100;
                    }
                    catch
                    {
                        outputDataStream.Close();
                    }

                    crypto.Close();
                    outputDataStream.Close();
                    inputDataStream.Close();

                    bool doesHashMatch = false;
                    switch (faesMetaData.GetHashType())
                    {
                        case Checksums.ChecksumType.SHA1:
                            doesHashMatch = Checksums.CompareHash(faesMetaData.GetOrigHash(), Checksums.GetSHA1(outputFilePath));
                            break;
                        case Checksums.ChecksumType.SHA256:
                            doesHashMatch = Checksums.CompareHash(faesMetaData.GetOrigHash(), Checksums.GetSHA256(outputFilePath));
                            break;
                        case Checksums.ChecksumType.SHA384:
                            doesHashMatch = Checksums.CompareHash(faesMetaData.GetOrigHash(), Checksums.GetSHA384(outputFilePath));
                            break;
                        case Checksums.ChecksumType.SHA512:
                            doesHashMatch = Checksums.CompareHash(faesMetaData.GetOrigHash(), Checksums.GetSHA512(outputFilePath));
                            break;
                        default:
                            Logging.Log($"{faesMetaData.GetHashType()} is an unknown checksum type! Ensure you are using an updated version of FAES!", Severity.WARN);
                            break;
                    }
                    if (!doesHashMatch)
                    {
                        Logging.Log("Invalid Checksum detected! Assuming password is incorrect.");
                        return false;
                    }
                    Logging.Log("Valid Checksum detected!");
                    return true;
                }
                catch
                {
                    crypto.Close();
                    inputDataStream.Close();
                    outputDataStream.Close();

                    return false;
                }
            }
            catch (CryptographicException)
            {
                inputDataStream.Close();

                return false;
            }
        }
    }
}