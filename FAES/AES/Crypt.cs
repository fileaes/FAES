using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace FAES.AES
{
    internal class Crypt
    {
        protected byte[] _specifiedSalt = null;

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
        internal bool Encrypt(byte[] metaData, string inputFilePath, string outputFilePath, string encryptionPassword, ref decimal percentComplete)
        {
            byte[] salt;
            byte[] passwordBytes = Encoding.UTF8.GetBytes(encryptionPassword);

            if (_specifiedSalt != null) salt = _specifiedSalt;
            else salt = CryptUtils.GenerateRandomSalt();

            RijndaelManaged AES = new RijndaelManaged();
            AES.KeySize = 256;
            AES.BlockSize = 128;
            AES.Padding = PaddingMode.PKCS7;

            var key = new Rfc2898DeriveBytes(passwordBytes, salt, 51200);
            AES.Key = key.GetBytes(AES.KeySize / 8);
            AES.IV = key.GetBytes(AES.BlockSize / 8);

            AES.Mode = CipherMode.CBC;

            FileStream outputDataStream = new FileStream(outputFilePath, FileMode.Create);
            outputDataStream.Write(metaData, 0, metaData.Length);
            outputDataStream.Write(salt, 0, salt.Length);

            CryptoStream crypto = new CryptoStream(outputDataStream, AES.CreateEncryptor(), CryptoStreamMode.Write);
            FileStream inputDataStream = new FileStream(inputFilePath, FileMode.Open);

            byte[] buffer = new byte[FileAES_Utilities.GetCryptoStreamBuffer()];
            int read;

            long expectedComplete = metaData.Length + AES.KeySize + AES.BlockSize;

            Logging.Log(String.Format("Beginning writing encrypted data..."), Severity.DEBUG);
            while ((read = inputDataStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                try
                {
                    percentComplete = Math.Ceiling((decimal)((Convert.ToDouble(outputDataStream.Length) / Convert.ToDouble(expectedComplete)) * 100));
                    if (percentComplete > 100) percentComplete = 100;
                }
                catch { }

                crypto.Write(buffer, 0, read);
            }
            Logging.Log(String.Format("Finished writing encrypted data."), Severity.DEBUG);

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
        internal bool Decrypt(MetaData faesMetaData, string inputFilePath, string outputFilePath, string encryptionPassword, ref decimal percentComplete)
        {
            CipherMode cipher = CipherMode.CBC;
            byte[] metaData = new byte[faesMetaData.GetLength()];
            byte[] salt = new byte[32];
            byte[] passwordBytes = Encoding.UTF8.GetBytes(encryptionPassword);

            FileStream inputDataStream = new FileStream(inputFilePath, FileMode.Open);

            inputDataStream.Read(metaData, 0, faesMetaData.GetLength());
            inputDataStream.Read(salt, 0, salt.Length);

            RijndaelManaged AES = new RijndaelManaged();
            AES.KeySize = 256;
            AES.BlockSize = 128;
            var key = new Rfc2898DeriveBytes(passwordBytes, salt, 51200);
            AES.Key = key.GetBytes(AES.KeySize / 8);
            AES.IV = key.GetBytes(AES.BlockSize / 8);
            AES.Padding = PaddingMode.PKCS7;
            AES.Mode = cipher;

            try
            {
                CryptoStream crypto = new CryptoStream(inputDataStream, AES.CreateDecryptor(), CryptoStreamMode.Read);
                FileStream outputDataStream = new FileStream(outputFilePath, FileMode.Create);

                try
                {
                    int read;
                    byte[] buffer = new byte[FileAES_Utilities.GetCryptoStreamBuffer()];

                    long expectedComplete = salt.Length + AES.KeySize + AES.BlockSize;

                    try
                    {
                        Logging.Log(String.Format("Beginning writing decrypted data..."), Severity.DEBUG);
                        while ((read = crypto.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            try
                            {
                                percentComplete = Math.Ceiling((decimal)((Convert.ToDouble(outputDataStream.Length) / Convert.ToDouble(expectedComplete)) * 100));
                                if (percentComplete > 100) percentComplete = 100;
                            }
                            catch { }

                            outputDataStream.Write(buffer, 0, read);
                        }
                        Logging.Log(String.Format("Finished writing decrypted data."), Severity.DEBUG);
                    }
                    catch
                    {
                        outputDataStream.Close();
                    }

                    crypto.Close();
                    outputDataStream.Close();
                    inputDataStream.Close();
                    /*
                    if (Checksums.ConvertHashToString(hash) != Checksums.ConvertHashToString(Checksums.GetSHA1(outputName)))
                    {
                        Logging.Log(String.Format("Invalid Checksum detected! Assuming password is incorrect."), Severity.DEBUG);
                        FileAES_IntUtilities.SafeDeleteFile(outputName);
                        return false;
                    }
                    Logging.Log(String.Format("Valid Checksum detected!"), Severity.DEBUG);*/
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
