using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using FAES.Packaging;

namespace FAES.Tests
{
    [TestClass]
    public class EncryptAndDecrypt_Tests
    {
        public void File_EncryptDecrypt(bool testCompression, FAES.Packaging.CompressionMode compressionMode, FAES.Packaging.CompressionLevel compressionLevel)
        {
            string encFilePath = "TestFile.txt";
            string originalFileContents = "Hello World!\r\nTest";
            string finalFileContents = string.Empty;
            string password = "password";
            string hint = "Example Hint";
            string decFilePath = Path.ChangeExtension(encFilePath, FileAES_Utilities.ExtentionFAES);

            try
            {
                FileAES_Utilities.SetVerboseLogging(true);

                File.WriteAllText(encFilePath, originalFileContents);

                FAES_File encFile = new FAES_File(encFilePath);

                FileAES_Encrypt encrypt = new FileAES_Encrypt(encFile, password, hint);

                if (testCompression)
                    encrypt.SetCompressionMode(compressionMode, compressionLevel);

                bool encryptSuccess = encrypt.encryptFile();

                if (!encryptSuccess)
                    throw new Exception("Encryption Failed! 'encryptFile' was false.");

                FAES_File decFile = new FAES_File(decFilePath);

                FileAES_Decrypt decrypt = new FileAES_Decrypt(decFile, password);
                bool decryptSuccess = decrypt.decryptFile();

                if (!decryptSuccess)
                    throw new Exception("Decryption Failed! 'decryptFile' was false.");

                finalFileContents = File.ReadAllText(encFilePath).TrimEnd('\n', '\r', ' ');

                if (finalFileContents != originalFileContents)
                    throw new Exception("Final file contents does not match original!");
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            finally
            {
                if (File.Exists(encFilePath)) File.Delete(encFilePath);
                if (File.Exists(decFilePath)) File.Delete(decFilePath);

                Console.WriteLine("\r\n=== Test Values ===\r\n");
                Console.WriteLine("encFilePath: {0}", encFilePath);
                Console.WriteLine("decFilePath: {0}", decFilePath);
                Console.WriteLine("Initial Contents: {0}", originalFileContents.Replace("\r\n", "\\r\\n"));
                Console.WriteLine("Final Contents: {0}", finalFileContents.Replace("\r\n", "\\r\\n"));
            }
        }

        public void Folder_EncryptDecrypt(bool testCompression, FAES.Packaging.CompressionMode compressionMode, FAES.Packaging.CompressionLevel compressionLevel)
        {
            string encFolder = "TestFolder";
            string encFilePath = "TestFile.txt";
            string encPath = Path.Combine(encFolder, encFilePath);
            string originalFileContents = "Hello World!\r\nTest";
            string finalFileContents = string.Empty;
            string password = "password";
            string hint = "Example Hint";

            try
            {
                FileAES_Utilities.SetVerboseLogging(true);

                Directory.CreateDirectory(encFolder);
                File.WriteAllText(encPath, originalFileContents);

                FAES_File encFile = new FAES_File(encPath);

                FileAES_Encrypt encrypt = new FileAES_Encrypt(encFile, password, hint);

                if (testCompression)
                    encrypt.SetCompressionMode(compressionMode, compressionLevel);

                bool encryptSuccess = encrypt.encryptFile();

                if (!encryptSuccess)
                    throw new Exception("Encryption Failed! 'encryptFile' was false.");

                string decFilePath = Path.ChangeExtension(encPath, FileAES_Utilities.ExtentionFAES);
                FAES_File decFile = new FAES_File(decFilePath);

                FileAES_Decrypt decrypt = new FileAES_Decrypt(decFile, password);
                bool decryptSuccess = decrypt.decryptFile();

                if (!decryptSuccess)
                    throw new Exception("Decryption Failed! 'decryptFile' was false.");

                finalFileContents = File.ReadAllText(encPath).TrimEnd('\n', '\r', ' ');

                if (finalFileContents != originalFileContents)
                    throw new Exception("Final file contents does not match original!");
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            finally
            {
                if (Directory.Exists(encFolder)) Directory.Delete(encFolder, true);

                Console.WriteLine("\r\n=== Test Values ===\r\n");
                Console.WriteLine("encFolder: {0}", encFolder);
                Console.WriteLine("encFilePath: {0}", encFilePath);
                Console.WriteLine("encPath: {0}", encPath);
                Console.WriteLine("Initial Contents: {0}", originalFileContents.Replace("\r\n", "\\r\\n"));
                Console.WriteLine("Final Contents: {0}", finalFileContents.Replace("\r\n", "\\r\\n"));
            }
        }

        [TestMethod]
        public void FAESv3_File_Auto() // Letting FAES automatically decide the compression method and level
        {
            try
            {
                File_EncryptDecrypt(false, 0, 0);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public void FAESv3_Folder_Auto() // Letting FAES automatically decide the compression method and level
        {
            try
            {
                Folder_EncryptDecrypt(false, 0, 0);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public void FAESv3_File_LZMA()
        {
            try
            {
                File_EncryptDecrypt(true, CompressionMode.LZMA, CompressionLevel.Fastest);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public void FAESv3_Folder_LZMA()
        {
            try
            {
                Folder_EncryptDecrypt(true, CompressionMode.LZMA, CompressionLevel.Fastest);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public void FAESv3_File_TAR()
        {
            try
            {
                File_EncryptDecrypt(true, CompressionMode.TAR, CompressionLevel.Fastest);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public void FAESv3_Folder_TAR()
        {
            try
            {
                Folder_EncryptDecrypt(true, CompressionMode.TAR, CompressionLevel.Fastest);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public void FAESv3_File_ZIP()
        {
            try
            {
                File_EncryptDecrypt(true, CompressionMode.ZIP, CompressionLevel.Fastest);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public void FAESv3_Folder_ZIP()
        {
            try
            {
                Folder_EncryptDecrypt(true, CompressionMode.ZIP, CompressionLevel.Fastest);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public void FAESv3_File_LGYZIP()
        {
            try
            {
                File_EncryptDecrypt(true, CompressionMode.LGYZIP, CompressionLevel.Fastest);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public void FAESv3_Folder_LGYZIP()
        {
            try
            {
                Folder_EncryptDecrypt(true, CompressionMode.LGYZIP, CompressionLevel.Fastest);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }
    }
}