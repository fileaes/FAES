using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using CoreChecksums;

namespace SecureAES
{
    public class SecureAES
    {
        protected byte[] _specifiedSalt;

        private const string _faesCBCModeIdentifier = "FAESv2-CBC";

        public SecureAES(byte[] salt = null)
        {
            if (salt != null) _specifiedSalt = salt;
        }

        public string CreateRandomPassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890-=_+[]{}:;@'~#,<.>/?`¬!$%^&*()";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        public static byte[] GenerateRandomSalt()
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

        public bool AES_Encrypt(string inputFile, string password)
        {
            string outputName;
            byte[] hash = Checksums.getSHA1(inputFile);
            byte[] salt = GenerateRandomSalt();

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

        public bool AES_Decrypt(string inputFile, string password, bool isValidationTool = false)
        {
            if (Path.GetExtension(inputFile) == ".faes")
            {
                string outputName;
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = new byte[20];
                byte[] salt = new byte[32];
                byte[] faesCBCMode = new byte[10];

                FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);
                fsCrypt.Read(hash, 0, hash.Length);
                fsCrypt.Read(salt, 0, salt.Length);
                fsCrypt.Read(faesCBCMode, 0, faesCBCMode.Length);

                RijndaelManaged AES = new RijndaelManaged();
                AES.KeySize = 256;
                AES.BlockSize = 128;
                var key = new Rfc2898DeriveBytes(passwordBytes, salt, 50000);
                AES.Key = key.GetBytes(AES.KeySize / 8);
                AES.IV = key.GetBytes(AES.BlockSize / 8);
                AES.Padding = PaddingMode.PKCS7;

                if (Encoding.UTF8.GetString(faesCBCMode) == _faesCBCModeIdentifier)
                {
                    AES.Mode = CipherMode.CBC;
                    Console.WriteLine("FileAESv2 Identifier Detected! Decrypting using FAESv2 Mode.");
                }
                else
                {
                    AES.Mode = CipherMode.CFB;
                    Console.WriteLine("Version Identifier not found! Decrypting using FAESv1 Mode.");
                    fsCrypt.Seek(-(faesCBCMode.Length), SeekOrigin.Current);
                }
                
                try
                {
                    CryptoStream cs = new CryptoStream(fsCrypt, AES.CreateDecryptor(), CryptoStreamMode.Read);

                    outputName = inputFile.Replace(".faes", ".faeszip");
                    if (isValidationTool) outputName += ".faesvalidation";

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
    }
}
