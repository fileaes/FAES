using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace FAES.AES
{
    internal class Crypt
    {
        protected byte[] _specifiedSalt = null;

        private const string _faesCBCModeIdentifier = "FAESv2-CBC"; //Current FAES Encryption Mode

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
        /// <param name="inputFile">File to encrypt</param>
        /// <param name="password">Password to encrypt the file</param>
        /// <param name="passwordHint">Hint for the password used on the file</param>
        /// <returns></returns>
        internal bool Encrypt(string inputFile, string password, string compressionMode, ref decimal percentComplete, string passwordHint = null)
        {
            if (String.IsNullOrEmpty(passwordHint)) passwordHint = "No Password Hint Set";

            MetaDataFAES fMD = new MetaDataFAES(passwordHint, compressionMode);
            string outputName;
            byte[] hash = Checksums.GetSHA1(inputFile);
            byte[] salt;
            byte[] metaData = fMD.GetMetaData();

            if (_specifiedSalt != null) salt = _specifiedSalt;
            else salt = GenerateRandomSalt();

            if (inputFile.Contains(FileAES_Utilities.ExtentionUFAES)) outputName = inputFile.Replace(FileAES_Utilities.ExtentionUFAES, "");
            else outputName = inputFile;

            outputName = Path.ChangeExtension(outputName, "faes");

            FileStream fsCrypt = new FileStream(outputName, FileMode.Create);

            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] faesCBCMode = Encoding.UTF8.GetBytes(_faesCBCModeIdentifier);

            RijndaelManaged AES = new RijndaelManaged();
            AES.KeySize = 256;
            AES.BlockSize = 128;
            AES.Padding = PaddingMode.PKCS7;

            var key = new Rfc2898DeriveBytes(passwordBytes, salt, 50000);
            AES.Key = key.GetBytes(AES.KeySize / 8);
            AES.IV = key.GetBytes(AES.BlockSize / 8);

            AES.Mode = CipherMode.CBC;

            fsCrypt.Write(hash, 0, hash.Length);
            fsCrypt.Write(salt, 0, salt.Length);
            fsCrypt.Write(faesCBCMode, 0, faesCBCMode.Length);
            fsCrypt.Write(metaData, 0, metaData.Length);

            CryptoStream cs = new CryptoStream(fsCrypt, AES.CreateEncryptor(), CryptoStreamMode.Write);

            FileStream fsIn = new FileStream(inputFile, FileMode.Open);

            byte[] buffer = new byte[FileAES_Utilities.GetCryptoStreamBuffer()];
            int read;

            long expectedComplete = fsIn.Length + hash.Length + salt.Length + faesCBCMode.Length + metaData.Length + AES.KeySize + AES.BlockSize;

            Logging.Log(String.Format("Beginning writing encrypted data..."), Severity.DEBUG);
            while ((read = fsIn.Read(buffer, 0, buffer.Length)) > 0)
            {
                try
                {
                    percentComplete = Math.Ceiling((decimal)((Convert.ToDouble(fsCrypt.Length) / Convert.ToDouble(expectedComplete)) * 100));
                    if (percentComplete > 100) percentComplete = 100;
                }
                catch { }

                cs.Write(buffer, 0, read);
            }
            Logging.Log(String.Format("Finished writing encrypted data."), Severity.DEBUG);

            fsIn.Close();
            cs.Close();
            fsCrypt.Close();

            return true;
        }

        /// <summary>
        /// Decrypts the selected file using the given password
        /// </summary>
        /// <param name="inputFile">Encrypted File</param>
        /// <param name="password">Password to decrypt the file</param>
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

            RijndaelManaged AES = new RijndaelManaged();
            AES.KeySize = 256;
            AES.BlockSize = 128;
            var key = new Rfc2898DeriveBytes(passwordBytes, salt, 50000);
            AES.Key = key.GetBytes(AES.KeySize / 8);
            AES.IV = key.GetBytes(AES.BlockSize / 8);
            AES.Padding = PaddingMode.PKCS7;
            AES.Mode = cipher;

            try
            {
                CryptoStream cs = new CryptoStream(fsCrypt, AES.CreateDecryptor(), CryptoStreamMode.Read);

                outputName = Path.ChangeExtension(inputFile, FileAES_Utilities.ExtentionUFAES);

                try
                {
                    FileStream fsOut = new FileStream(outputName, FileMode.Create);
                    File.SetAttributes(outputName, FileAttributes.Hidden);

                    int read;
                    byte[] buffer = new byte[FileAES_Utilities.GetCryptoStreamBuffer()];

                    long expectedComplete = fsCrypt.Length + hash.Length + salt.Length + faesCBCMode.Length + faesMetaData.Length + AES.KeySize + AES.BlockSize;

                    try
                    {
                        Logging.Log(String.Format("Beginning writing decrypted data..."), Severity.DEBUG);
                        while ((read = cs.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            try
                            {
                                percentComplete = Math.Ceiling((decimal)((Convert.ToDouble(fsOut.Length) / Convert.ToDouble(expectedComplete)) * 100));
                                if (percentComplete > 100) percentComplete = 100;
                            }
                            catch { }

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
        /// Handles which Decryption Mode should be used when decrypting the file. Ensures compatability with previous FAES versions.
        /// </summary>
        /// <param name="fsCrypt">Encrypted Files FileStream</param>
        /// <param name="dHash">Output of original file hash</param>
        /// <param name="dSalt">Output of password salt</param>
        /// <param name="dFaesMode">Output of the FAES Decryption Mode used on the file</param>
        /// <param name="dMetaData">Output of the FAES MetaData contained in the file</param>
        /// <param name="cipherMode">Output of the Cipher Mode used on the file</param>
        /// <param name="hideWriteLine">Disables pushing the FAES Encryption Mode to console</param>
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
        /// Generates a Random Salt
        /// </summary>
        /// <returns>A Random Salt (byte[32])</returns>
        private static byte[] GenerateRandomSalt()
        {
            byte[] data = new byte[32];

            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                for (int i = 0; i < 10; i++)
                {
                    rng.GetBytes(data);
                }
            }
            return data;
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
    }
}
