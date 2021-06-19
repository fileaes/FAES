using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace FAES.Tests
{
    [TestClass]
    public class Decrypt_Tests
    {
        public void File_Decrypt(string exampleFileName, string subFolder)
        {
            string folderName = Path.Combine("ExampleFiles", subFolder);
            string originalFileContents = "ExampleString";
            string finalFileContents = string.Empty;
            string password = "password";
            string filePath = Path.Combine(folderName, exampleFileName);
            string exportPath = string.Empty;

            try
            {
                FileAES_Utilities.SetVerboseLogging(true);

                FAES_File decFile = new FAES_File(filePath);
                exportPath = Path.Combine(folderName, "Example.txt");

                FileAES_Decrypt decrypt = new FileAES_Decrypt(decFile, password, false);
                bool decryptSuccess = decrypt.DecryptFile();

                if (!decryptSuccess)
                    throw new Exception("Decryption Failed! 'decryptFile' was false.");

                finalFileContents = File.ReadAllText(exportPath).TrimEnd('\n', '\r', ' ');

                if (finalFileContents != originalFileContents)
                    throw new Exception("Final file contents does not match original!");
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            finally
            {
                if (!String.IsNullOrWhiteSpace(exportPath) && File.Exists(exportPath)) File.Delete(exportPath);

                Console.WriteLine("\r\n=== Test Values ===\r\n");
                Console.WriteLine("filePath: {0}", filePath);
                Console.WriteLine("exportPath: {0}", exportPath);
                Console.WriteLine("Initial Contents: {0}", originalFileContents.Replace("\r\n", "\\r\\n"));
                Console.WriteLine("Final Contents: {0}", finalFileContents.Replace("\r\n", "\\r\\n"));
            }
        }

        public void Folder_Decrypt(string exampleFileName, string subFolder)
        {
            string folderName = Path.Combine("ExampleFiles", subFolder);
            string originalFileContents = "ExampleString";
            string finalFileContents = string.Empty;
            string password = "password";
            string filePath = Path.Combine(folderName, exampleFileName);
            string exportPath = Path.Combine(folderName, "Example", "Example.txt");

            try
            {
                FAES_File decFile = new FAES_File(filePath);

                FileAES_Decrypt decrypt = new FileAES_Decrypt(decFile, password, false);
                bool decryptSuccess = decrypt.DecryptFile();

                if (!decryptSuccess)
                    throw new Exception("Decryption Failed! 'decryptFile' was false.");

                finalFileContents = File.ReadAllText(exportPath).TrimEnd('\n', '\r', ' ');

                if (finalFileContents != originalFileContents)
                    throw new Exception("Final file contents does not match original!");
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            finally
            {
                if (!String.IsNullOrWhiteSpace(exportPath) && File.Exists(exportPath)) File.Delete(exportPath);

                Console.WriteLine("\r\n=== Test Values ===\r\n");
                Console.WriteLine("filePath: {0}", filePath);
                Console.WriteLine("exportPath: {0}", exportPath);
                Console.WriteLine("Initial Contents: {0}", originalFileContents.Replace("\r\n", "\\r\\n"));
                Console.WriteLine("Final Contents: {0}", finalFileContents.Replace("\r\n", "\\r\\n"));
            }
        }

        [TestMethod]
        public void FAESv3_File_Decrypt()
        {
            try
            {
                File_Decrypt("FAESv3.faes", "EncryptedFiles");
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public void FAESv2_File_Decrypt()
        {
            try
            {
                File_Decrypt("FAESv2.faes", "EncryptedFiles");
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public void FAESv1_File_Decrypt()
        {
            try
            {
                File_Decrypt("FAESv1.faes", "EncryptedFiles");
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public void FAESv0_File_Decrypt()
        {
            try
            {
                File_Decrypt("Legacy.mcrypt", "EncryptedFiles");
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public void FAESv3_Folder_Decrypt()
        {
            try
            {
                Folder_Decrypt("FAESv3.faes", "EncryptedFolders");
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public void FAESv2_Folder_Decrypt()
        {
            try
            {
                Folder_Decrypt("FAESv2.faes", "EncryptedFolders");
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public void FAESv1_Folder_Decrypt()
        {
            try
            {
                Folder_Decrypt("FAESv1.faes", "EncryptedFolders");
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public void FAESv0_Folder_Decrypt()
        {
            try
            {
                Folder_Decrypt("Legacy.mcrypt", "EncryptedFolders");
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }
    }
}