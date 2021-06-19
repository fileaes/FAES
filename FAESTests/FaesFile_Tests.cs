using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace FAES.Tests
{
    [TestClass]
    public class FaesFile_Tests
    {
        [TestMethod]
        public void IsFileCheck()
        {
            string filePath = "TestFile.txt";
            try
            {
                FileAES_Utilities.SetVerboseLogging(true);

                File.WriteAllText(filePath, "Test");
                FAES_File faesFile = new FAES_File(filePath);

                if (!faesFile.IsFile())
                    Assert.Fail("FAES_File incorrectly assumes a file is a folder!");
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
            finally
            {
                if (File.Exists(filePath)) File.Delete(filePath);
            }
        }

        [TestMethod]
        public void IsFolderCheck()
        {
            string filePath = "TestFolder";

            try
            {
                FileAES_Utilities.SetVerboseLogging(true);

                Directory.CreateDirectory(filePath);
                File.WriteAllText(Path.Combine(filePath, "TestFile.txt"), "Test");
                FAES_File faesFile = new FAES_File(filePath);

                if (!faesFile.IsFolder())
                    Assert.Fail("FAES_File incorrectly assumes a file is a folder!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                if (Directory.Exists(filePath)) Directory.Delete(filePath, true);
            }
        }

        [TestMethod]
        public void IsFileEncryptable()
        {
            string filePath = "TestFile.txt";
            try
            {
                FileAES_Utilities.SetVerboseLogging(true);

                File.WriteAllText(filePath, "Test");
                FAES_File faesFile = new FAES_File(filePath);

                if (!faesFile.IsFileEncryptable())
                    Assert.Fail("FAES_File incorrectly assumes file cannot be encrypted!");
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
            finally
            {
                if (File.Exists(filePath)) File.Delete(filePath);
            }
        }

        [TestMethod]
        public void IsFileDecryptable()
        {
            string filePath = "TestFile.faes";
            try
            {
                FileAES_Utilities.SetVerboseLogging(true);

                File.WriteAllText(filePath, "Test");
                FAES_File faesFile = new FAES_File(filePath);

                if (!faesFile.IsFileDecryptable())
                    Assert.Fail("FAES_File incorrectly assumes file cannot be decrypted!");
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
            finally
            {
                if (File.Exists(filePath)) File.Delete(filePath);
            }
        }
    }
}