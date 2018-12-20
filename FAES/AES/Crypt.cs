using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using CoreChecksums;

namespace FAES.AES
{
    public class Crypt
    {
        protected byte[] _specifiedSalt;

        private const string _faesCBCModeIdentifier = "FAESv2-CBC"; //Current FAES Encryption Mode

        /// <summary>
        /// FAES Encrypt/Decrypt Handler
        /// </summary>
        /// <param name="salt">Custom Salt</param>
        public Crypt(byte[] salt = null)
        {
            if (salt != null) _specifiedSalt = salt;
        }

        /// <summary>
        /// Encrypts the selected file using the given password
        /// </summary>
        /// <param name="inputFile">File to encrypt</param>
        /// <param name="password">Password to encrypt the file</param>
        /// <param name="passwordHint">Hint for the password used on the file</param>
        /// <returns></returns>
        public bool Encrypt(string inputFile, string password, string compressionMode, string passwordHint = null)
        {
            if (String.IsNullOrEmpty(passwordHint)) passwordHint = "No Password Hint Set";
            else if (passwordHint.Contains("¬")) throw new Exception("Password hint contains invalid characters.");
            MetaDataFAES fMD = new MetaDataFAES(passwordHint, compressionMode);
            string outputName;
            byte[] hash = Checksums.getSHA1(inputFile);
            byte[] salt;
            byte[] metaData = fMD.getMetaData();

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

            byte[] buffer = new byte[1048576];
            int read;

            while ((read = fsIn.Read(buffer, 0, buffer.Length)) > 0)
            {
                cs.Write(buffer, 0, read);
            }

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
        public bool Decrypt(string inputFile, string password)
        {
            if (FileAES_Utilities.isFileDecryptable(inputFile))
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

                        int read;
                        byte[] buffer = new byte[1048576];

                        try
                        {
                            while ((read = cs.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                fsOut.Write(buffer, 0, read);
                            }
                        }
                        catch
                        {
                            fsOut.Close();
                        }

                        cs.Close();
                        fsOut.Close();
                        fsCrypt.Close();

                        if (Checksums.convertHashToString(hash) != Checksums.convertHashToString(Checksums.getSHA1(outputName)))
                        {
                            FileAES_IntUtilities.SafeDeleteFile(outputName);
                            return false;
                        }

                        return true;
                    }
                    catch
                    {
                        cs.Close();
                        fsCrypt.Close();
                    }
                }
                catch (CryptographicException)
                {
                    fsCrypt.Close();
                }
            }

            return false;
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
        private FileStream DecryptModeHandler(FileStream fsCrypt, ref byte[] dHash, ref byte[] dSalt, ref byte[] dFaesMode, ref byte[] dMetaData, ref CipherMode cipherMode, bool hideWriteLine = false)
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
                    if (!hideWriteLine) Console.WriteLine("FAESv2 Identifier Detected! Decrypting using FAESv2 Mode.");
                    dMetaData = metaData;
                    break;
                case "FAESv1-CBC":
                    cipherMode = CipherMode.CBC;
                    if (!hideWriteLine) Console.WriteLine("FAESv1 Identifier Detected! Decrypting using FAESv1 Mode.");
                    fsCrypt.Position = hash.Length + salt.Length + faesCBCMode.Length;
                    break;
                default:
                    cipherMode = CipherMode.CFB;
                    if (!hideWriteLine) Console.WriteLine("Version Identifier not found! Decrypting using LegacyCFB Mode.");
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
        /// <param name="inputFile">Encrypted File</param>
        /// <param name="PasswordHint">Output Password Hint</param>
        /// <returns>If the inputFile was valid</returns>
        public string GetPasswordHint(FAES_File faesFile)
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
                    return fMD.getPasswordHint();
                }
                return "No Password Hint Set";
            }
            else throw new FormatException("This method only supports encrypted FAES Files!");
        }

        /// <summary>
        /// Gets the UNIX Timestamp (UTC) of when the file was encrypted
        /// </summary>
        /// <param name="inputFile">Encrypted File</param>
        /// <param name="UNIXTimestampUTC">Output UNIX Timestamp</param>
        /// <returns>If the inputFile was valid</returns>
        public int GetEncryptionTimestamp(FAES_File faesFile)
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
                    return fMD.getEncryptionTimestamp();
                }
                return -1;
            }
            else throw new FormatException("This method only supports encrypted FAES Files!");
        }

        /// <summary>
        /// Gets the Compression Mode of the encrypted file
        /// </summary>
        /// <param name="inputFile">Encrypted File</param>
        /// <param name="PasswordHint">Output Password Hint</param>
        /// <returns>If the inputFile was valid</returns>
        public string GetCompressionMode(FAES_File faesFile)
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
                    return fMD.getCompressionMode();
                }
                return "LGYZIP";
            }
            else throw new FormatException("This method only supports encrypted FAES Files!");
        }
    }
}
