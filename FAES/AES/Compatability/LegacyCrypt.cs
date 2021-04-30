using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace FAES.AES.Compatibility
{
    internal class LegacyCrypt
    {
        //private const string _faesCBCModeIdentifier = "FAESv2-CBC"; //Current FAES Encryption Mode

        /// <summary>
        /// Legacy FAES Decrypt Handler.
        /// </summary>
        internal LegacyCrypt()
        { }

        /// <summary>
        /// Decrypts the selected file using the given password
        /// </summary>
        /// <param name="inputFile">Encrypted File</param>
        /// <param name="password">Password to decrypt the file</param>
        /// <param name="percentComplete">Percent of completion</param>
        /// <returns>If the decryption was successful</returns>
        internal bool Decrypt(string inputFile, string password, ref decimal percentComplete)
        {
            string outputName;
            CipherMode cipher = CipherMode.CBC;
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] hash = new byte[20];
            byte[] salt = new byte[32];
            byte[] faesCBCMode = new byte[10];
            byte[] faesMetaData = new byte[256];

            FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);

            fsCrypt = DecryptModeHandler(fsCrypt, ref hash, ref salt, ref faesCBCMode, ref faesMetaData, ref cipher);

            int keySize = 256;
            int blockSize = 128;
            Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(passwordBytes, salt, 50000);
            RijndaelManaged AES = new RijndaelManaged
            {
                KeySize = keySize,
                BlockSize = blockSize,
                Key = key.GetBytes(keySize / 8),
                IV = key.GetBytes(blockSize / 8),
                Padding = PaddingMode.PKCS7,
                Mode = cipher
            };
            
            try
            {
                CryptoStream cs = new CryptoStream(fsCrypt, AES.CreateDecryptor(), CryptoStreamMode.Read);
                outputName = Path.ChangeExtension(inputFile, FileAES_Utilities.ExtentionUFAES);

                try
                {
                    FileStream fsOut = new FileStream(outputName, FileMode.Create);
                    File.SetAttributes(outputName, FileAttributes.Hidden);

                    byte[] buffer = new byte[FileAES_Utilities.GetCryptoStreamBuffer()];
                    long expectedComplete = fsCrypt.Length + hash.Length + salt.Length + faesCBCMode.Length + faesMetaData.Length + AES.KeySize + AES.BlockSize;

                    try
                    {
                        int read;
                        Logging.Log(String.Format("Beginning writing decrypted data..."), Severity.DEBUG);
                        while ((read = cs.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            try
                            {
                                percentComplete = Math.Ceiling((decimal)((Convert.ToDouble(fsOut.Length) / Convert.ToDouble(expectedComplete)) * 100));
                                if (percentComplete > 100) percentComplete = 100;
                            }
                            catch
                            {
                                Logging.Log(String.Format("Percentage completion calculation failed!"), Severity.WARN);
                            }

                            fsOut.Write(buffer, 0, read);
                        }
                        Logging.Log(String.Format("Finished writing decrypted data."), Severity.DEBUG);
                    }
                    catch
                    {
                        fsOut.Close();
                    }

                    cs.Close();
                    fsOut.Close();
                    fsCrypt.Close();

                    if (Checksums.ConvertHashToString(hash) != Checksums.ConvertHashToString(Checksums.GetSHA1(outputName)))
                    {
                        Logging.Log(String.Format("Invalid Checksum detected! Assuming password is incorrect."), Severity.DEBUG);
                        FileAES_IntUtilities.SafeDeleteFile(outputName);
                        return false;
                    }
                    Logging.Log(String.Format("Valid Checksum detected!"), Severity.DEBUG);
                    return true;
                }
                catch
                {
                    cs.Close();
                    fsCrypt.Close();

                    return false;
                }
            }
            catch (CryptographicException)
            {
                fsCrypt.Close();

                return false;
            }
        }

        /// <summary>
        /// Handles which Decryption Mode should be used when decrypting the file. Ensures Compatibility with previous FAES versions.
        /// </summary>
        /// <param name="fsCrypt">Encrypted Files FileStream</param>
        /// <param name="dHash">Output of original file hash</param>
        /// <param name="dSalt">Output of password salt</param>
        /// <param name="dFaesMode">Output of the FAES Decryption Mode used on the file</param>
        /// <param name="dMetaData">Output of the FAES MetaData contained in the file</param>
        /// <param name="cipherMode">Output of the Cipher Mode used on the file</param>
        /// <param name="suppressLog">Disables pushing the FAES Encryption Mode to console</param>
        /// <returns></returns>
        private FileStream DecryptModeHandler(FileStream fsCrypt, ref byte[] dHash, ref byte[] dSalt, ref byte[] dFaesMode, ref byte[] dMetaData, ref CipherMode cipherMode, bool suppressLog = false)
        {
            byte[] hash = new byte[20];
            byte[] salt = new byte[32];
            byte[] faesCBCMode = new byte[10];
            byte[] metaData = new byte[256];

            fsCrypt.Read(hash, 0, hash.Length);
            fsCrypt.Read(salt, 0, salt.Length);
            fsCrypt.Read(faesCBCMode, 0, faesCBCMode.Length);
            fsCrypt.Read(metaData, 0, metaData.Length);

            dHash = hash;
            dSalt = salt;
            dFaesMode = faesCBCMode;
            dMetaData = metaData;

            switch (Encoding.UTF8.GetString(faesCBCMode))
            {
                case "FAESv2-CBC":
                    cipherMode = CipherMode.CBC;
                    if (!suppressLog) Logging.Log(String.Format("FAESv2 Identifier Detected! Decrypting using FAESv2 Mode."), Severity.DEBUG);
                    dMetaData = metaData;
                    break;
                case "FAESv1-CBC":
                    cipherMode = CipherMode.CBC;
                    if (!suppressLog) Logging.Log(String.Format("FAESv1 Identifier Detected! Decrypting using FAESv1 Mode."), Severity.DEBUG);
                    fsCrypt.Position = hash.Length + salt.Length + faesCBCMode.Length;
                    break;
                default:
                    cipherMode = CipherMode.CFB;
                    if (!suppressLog) Logging.Log(String.Format("Version Identifier not found! Decrypting using LegacyCFB Mode."), Severity.DEBUG);
                    fsCrypt.Position = hash.Length + salt.Length;
                    break;
            }
            return fsCrypt;
        }

        /// <summary>
        /// Gets the Password Hint of the encrypted file
        /// </summary>
        /// <param name="faesFile">Encrypted File</param>
        /// <returns>The password hint</returns>
        internal string GetPasswordHint(FAES_File faesFile)
        {
            if (faesFile.isFileDecryptable())
            {
                CipherMode cipher = CipherMode.CBC;
                byte[] hash = new byte[20];
                byte[] salt = new byte[32];
                byte[] faesCBCMode = new byte[10];
                byte[] faesMetaData = new byte[256];

                FileStream fsCrypt = new FileStream(faesFile.getPath(), FileMode.Open);
                fsCrypt = DecryptModeHandler(fsCrypt, ref hash, ref salt, ref faesCBCMode, ref faesMetaData, ref cipher, true);
                fsCrypt.Close();

                if (Encoding.UTF8.GetString(faesCBCMode) == "FAESv2-CBC")
                {
                    MetaDataFAES fMD = new MetaDataFAES(faesMetaData);
                    return fMD.GetPasswordHint();
                }
                return "No Password Hint Set";
            }
            else throw new FormatException("This method only supports encrypted FAES Files!");
        }

        /// <summary>
        /// Gets the UNIX Timestamp (UTC) of when the file was encrypted
        /// </summary>
        /// <param name="faesFile">Encrypted File</param>
        /// <returns>Timestamp of when the file was encrypted</returns>
        internal int GetEncryptionTimestamp(FAES_File faesFile)
        {
            if (faesFile.isFileDecryptable())
            {
                CipherMode cipher = CipherMode.CBC;
                byte[] hash = new byte[20];
                byte[] salt = new byte[32];
                byte[] faesCBCMode = new byte[10];
                byte[] faesMetaData = new byte[256];

                FileStream fsCrypt = new FileStream(faesFile.getPath(), FileMode.Open);
                fsCrypt = DecryptModeHandler(fsCrypt, ref hash, ref salt, ref faesCBCMode, ref faesMetaData, ref cipher, true);
                fsCrypt.Close();

                if (Encoding.UTF8.GetString(faesCBCMode) == "FAESv2-CBC")
                {
                    MetaDataFAES fMD = new MetaDataFAES(faesMetaData);
                    return fMD.GetEncryptionTimestamp();
                }
                return -1;
            }
            else throw new FormatException("This method only supports encrypted FAES Files!");
        }

        /// <summary>
        /// Gets the FAES version used to encrypt the file
        /// </summary>
        /// <param name="faesFile">Encrypted File</param>
        /// <returns>FAES Version used to encrypt file</returns>
        internal string GetEncryptionVersion(FAES_File faesFile)
        {
            if (faesFile.isFileDecryptable())
            {
                CipherMode cipher = CipherMode.CBC;
                byte[] hash = new byte[20];
                byte[] salt = new byte[32];
                byte[] faesCBCMode = new byte[10];
                byte[] faesMetaData = new byte[256];

                FileStream fsCrypt = new FileStream(faesFile.getPath(), FileMode.Open);
                fsCrypt = DecryptModeHandler(fsCrypt, ref hash, ref salt, ref faesCBCMode, ref faesMetaData, ref cipher, true);
                fsCrypt.Close();

                if (Encoding.UTF8.GetString(faesCBCMode) == "FAESv2-CBC")
                    return new MetaDataFAES(faesMetaData).GetEncryptionVersion();
                else if (Encoding.UTF8.GetString(faesCBCMode) == "FAESv1-CBC")
                    return "v1.0.0";
                else if (Encoding.UTF8.GetString(faesCBCMode).Contains("FAES"))
                    return "Post-" + FileAES_Utilities.GetVersion();
                else
                    return "Pre-v1.0.0";
            }
            else throw new FormatException("This method only supports encrypted FAES Files!");
        }

        /// <summary>
        /// Gets if the currently selected FAES file is decryptable
        /// </summary>
        /// <param name="faesFile">Encrypted File</param>
        /// <returns>If the currently selected FAES File can be decrypted</returns>
        internal bool IsDecryptable(FAES_File faesFile)
        {
            return IsDecryptable(faesFile.getPath());
        }

        /// <summary>
        /// Gets if the currently selected file is decryptable
        /// </summary>
        /// <param name="path">Encrypted File</param>
        /// <returns>If the currently selected file can be decrypted</returns>
        internal bool IsDecryptable(string path)
        {
            try
            {
                CipherMode cipher = CipherMode.CBC;
                byte[] hash = new byte[20];
                byte[] salt = new byte[32];
                byte[] faesCBCMode = new byte[10];
                byte[] faesMetaData = new byte[256];

                FileStream fsCrypt = new FileStream(path, FileMode.Open);
                fsCrypt = DecryptModeHandler(fsCrypt, ref hash, ref salt, ref faesCBCMode, ref faesMetaData, ref cipher, true);
                fsCrypt.Close();

                if (Encoding.UTF8.GetString(faesCBCMode) == "FAESv2-CBC")
                    return true;
                else if (Encoding.UTF8.GetString(faesCBCMode) == "FAESv1-CBC")
                    return true;
                else if (Encoding.UTF8.GetString(faesCBCMode).Contains("FAES"))
                    return false;
                else
                    return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the Compression Mode of the encrypted file
        /// </summary>
        /// <param name="faesFile">Encrypted File</param>
        /// <returns>Output Password Hint</returns>
        internal string GetCompressionMode(FAES_File faesFile)
        {
            if (faesFile.isFileDecryptable())
            {
                CipherMode cipher = CipherMode.CBC;
                byte[] hash = new byte[20];
                byte[] salt = new byte[32];
                byte[] faesCBCMode = new byte[10];
                byte[] faesMetaData = new byte[256];

                FileStream fsCrypt = new FileStream(faesFile.getPath(), FileMode.Open);
                fsCrypt = DecryptModeHandler(fsCrypt, ref hash, ref salt, ref faesCBCMode, ref faesMetaData, ref cipher, true);
                fsCrypt.Close();

                if (Encoding.UTF8.GetString(faesCBCMode) == "FAESv2-CBC")
                {
                    MetaDataFAES fMD = new MetaDataFAES(faesMetaData);
                    return fMD.GetCompressionMode();
                }
                return "LGYZIP";
            }
            else throw new FormatException("This method only supports encrypted FAES Files!");
        }

        internal MetaDataFAES GetAllMetaData(FAES_File faesFile)
        {
            try
            {
                CipherMode cipher = CipherMode.CBC;
                byte[] hash = new byte[20];
                byte[] salt = new byte[32];
                byte[] faesCBCMode = new byte[10];
                byte[] faesMetaData = new byte[256];

                FileStream fsCrypt = new FileStream(faesFile.getPath(), FileMode.Open);
                fsCrypt = DecryptModeHandler(fsCrypt, ref hash, ref salt, ref faesCBCMode, ref faesMetaData, ref cipher, true);
                fsCrypt.Close();

                if (Encoding.UTF8.GetString(faesCBCMode) == "FAESv2-CBC")
                {
                    return new MetaDataFAES(faesMetaData);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        internal MetaDataFAES GetAllMetaData(string filePath)
        {
            try
            {
                CipherMode cipher = CipherMode.CBC;
                byte[] hash = new byte[20];
                byte[] salt = new byte[32];
                byte[] faesCBCMode = new byte[10];
                byte[] faesMetaData = new byte[256];

                FileStream fsCrypt = new FileStream(filePath, FileMode.Open);
                fsCrypt = DecryptModeHandler(fsCrypt, ref hash, ref salt, ref faesCBCMode, ref faesMetaData, ref cipher, true);
                fsCrypt.Close();

                if (Encoding.UTF8.GetString(faesCBCMode) == "FAESv2-CBC")
                {
                    return new MetaDataFAES(faesMetaData);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
