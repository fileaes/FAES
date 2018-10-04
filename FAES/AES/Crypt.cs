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

        private const string _faesCBCModeIdentifier = "FAESv2-CBC";

        public Crypt(byte[] salt = null)
        {
            if (salt != null) _specifiedSalt = salt;
        }

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

        public bool Encrypt(string inputFile, string password, string passwordHint = null)
        {
            if (String.IsNullOrEmpty(passwordHint)) passwordHint = "No Password Hint Set";
            MetaDataFAES fMD = new MetaDataFAES(passwordHint);
            string outputName;
            byte[] hash = Checksums.getSHA1(inputFile);
            byte[] salt = GenerateRandomSalt();
            byte[] metaData = fMD.getMetaData();

            if (inputFile.Contains(".faeszip")) outputName = inputFile.Replace(".faeszip", "");
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
                    if (!hideWriteLine) Console.WriteLine("Version Identifier not found! Decrypting using Legacy Mode.");
                    fsCrypt.Position = hash.Length + salt.Length;
                    break;
            }

            return fsCrypt;
        }

        public bool Decrypt(string inputFile, string password)
        {
            if (Path.GetExtension(inputFile) == ".faes" || Path.GetExtension(inputFile) == ".mcrypt")
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

                    if (Path.GetExtension(inputFile) == ".faes")
                        outputName = inputFile.Replace(".faes", ".faeszip");
                    else
                        outputName = inputFile.Replace(".mcrypt", ".faeszip");

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
                            File.Delete(outputName);
                            return false;
                        }
                        else return true;
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

        public bool GetPasswordHint(string inputFile, ref string PasswordHint)
        {
            if (Path.GetExtension(inputFile) == ".faes" || Path.GetExtension(inputFile) == ".mcrypt")
            {
                CipherMode cipher = CipherMode.CBC;
                byte[] hash = new byte[20];
                byte[] salt = new byte[32];
                byte[] faesCBCMode = new byte[10];
                byte[] faesMetaData = new byte[256];

                FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);
                fsCrypt = DecryptModeHandler(fsCrypt, ref hash, ref salt, ref faesCBCMode, ref faesMetaData, ref cipher, true);
                fsCrypt.Close();

                if (Encoding.UTF8.GetString(faesCBCMode) == "FAESv2-CBC")
                {
                    MetaDataFAES fMD = new MetaDataFAES(faesMetaData);
                    PasswordHint = fMD.getPasswordHint();
                }
                else PasswordHint = "No Password Hint Set";

                return true;
            }
            return false;
        }
    }
}
