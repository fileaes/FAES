using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace FAES.AES.Compatibility
{
    internal class LegacyCrypt
    {
        /// <summary>
        /// Decrypts the selected file using the given password
        /// </summary>
        /// <param name="inputFile">Encrypted File</param>
        /// <param name="password">Password to decrypt the file</param>
        /// <param name="percentComplete">Percent of completion</param>
        /// <returns>If the decryption was successful</returns>
        internal bool Decrypt(string inputFile, string password, ref decimal percentComplete)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);
            fsCrypt = DecryptModeHandler(fsCrypt, out byte[] hash, out byte[] salt, out byte[] faesCBCMode, out byte[] faesMetaData, out var cipher);

            const int keySize = 256;
            const int blockSize = 128;
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
                string outputName = Path.ChangeExtension(inputFile, FileAES_Utilities.ExtentionUFAES);

                try
                {
                    FileStream fsOut = new FileStream(outputName, FileMode.Create);
                    File.SetAttributes(outputName, FileAttributes.Hidden);

                    byte[] buffer = new byte[FileAES_Utilities.GetCryptoStreamBuffer()];
                    long expectedComplete = fsCrypt.Length + hash.Length + salt.Length + faesCBCMode.Length + faesMetaData.Length + AES.KeySize + AES.BlockSize;

                    try
                    {
                        int read;
                        Logging.Log("Beginning writing decrypted data...", Severity.DEBUG);
                        while ((read = cs.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            try
                            {
                                percentComplete = Math.Ceiling((decimal)((Convert.ToDouble(fsOut.Length) / Convert.ToDouble(expectedComplete)) * 100));
                                if (percentComplete > 100) percentComplete = 100;
                            }
                            catch
                            {
                                Logging.Log("Percentage completion calculation failed!", Severity.WARN);
                            }

                            fsOut.Write(buffer, 0, read);
                        }
                        Logging.Log("Finished writing decrypted data.", Severity.DEBUG);
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
                        Logging.Log("Invalid Checksum detected! Assuming password is incorrect.", Severity.DEBUG);
                        FileAES_IntUtilities.SafeDeleteFile(outputName);
                        return false;
                    }
                    Logging.Log("Valid Checksum detected!", Severity.DEBUG);
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
        private FileStream DecryptModeHandler(FileStream fsCrypt, out byte[] dHash, out byte[] dSalt, out byte[] dFaesMode, out byte[] dMetaData, out CipherMode cipherMode, bool suppressLog = false)
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
                    if (!suppressLog) Logging.Log("FAESv2 Identifier Detected! Decrypting using FAESv2 Mode.", Severity.DEBUG);
                    dMetaData = metaData;
                    break;

                case "FAESv1-CBC":
                    cipherMode = CipherMode.CBC;
                    if (!suppressLog) Logging.Log("FAESv1 Identifier Detected! Decrypting using FAESv1 Mode.", Severity.DEBUG);
                    fsCrypt.Position = hash.Length + salt.Length + faesCBCMode.Length;
                    break;

                default:
                    cipherMode = CipherMode.CFB;
                    if (!suppressLog) Logging.Log("Version Identifier not found! Decrypting using LegacyCFB Mode.", Severity.DEBUG);
                    fsCrypt.Position = hash.Length + salt.Length;
                    break;
            }
            return fsCrypt;
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
                FileStream fsCrypt = new FileStream(path, FileMode.Open);
                fsCrypt = DecryptModeHandler(fsCrypt, out _, out _, out byte[] faesCBCMode, out _, out _, true);
                fsCrypt.Close();

                switch (Encoding.UTF8.GetString(faesCBCMode))
                {
                    case "FAESv2-CBC":
                    case "FAESv1-CBC":
                        return true;
                }
                return !Encoding.UTF8.GetString(faesCBCMode).Contains("FAES");
            }
            catch
            {
                return false;
            }
        }

        internal MetaDataFAES GetAllMetaData(string filePath)
        {
            try
            {
                FileStream fsCrypt = new FileStream(filePath, FileMode.Open);
                fsCrypt = DecryptModeHandler(fsCrypt, out _, out _, out byte[] faesCBCMode, out byte[] faesMetaData, out _, true);
                fsCrypt.Close();

                switch (Encoding.UTF8.GetString(faesCBCMode))
                {
                    case "FAESv2-CBC":
                        return new MetaDataFAES(faesMetaData);

                    case "FAESv1-CBC":
                        return new MetaDataFAES("FAESv1");

                    default:
                        return new MetaDataFAES("Legacy");
                }
            }
            catch
            {
                throw new Exception("An unexpected error occurred when getting metadata!");
            }
        }
    }
}