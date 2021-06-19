using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace FAES.Tests
{
    [TestClass]
    public class MetaData_Tests
    {
        [TestMethod]
        public void FAESv3_MetaData()
        {
            string filePath = "ExampleFiles/EncryptedFiles/FAESv3.faes";

            string expectedOriginalName = "Example.txt";
            string expectedHint = "Hint";
            string expectedVer = "v1.2.0-RC_1";

            string actualOriginalName = string.Empty;
            string actualHint = string.Empty;
            string actualVer = string.Empty;

            try
            {
                FileAES_Utilities.SetVerboseLogging(true);

                FAES_File faesFile = new FAES_File(filePath);

                actualOriginalName = faesFile.GetOriginalFileName();
                if (expectedOriginalName != actualOriginalName)
                    Assert.Fail("Incorrect original name!");

                actualHint = faesFile.GetPasswordHint();
                if (expectedHint != actualHint)
                    Assert.Fail("Incorrect password hint!");

                actualVer = faesFile.GetEncryptionVersion();
                if (expectedVer != actualVer)
                    Assert.Fail("Incorrect encryption version!");

                // TODO: Add more metadata checks
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
            finally
            {
                Console.WriteLine("\r\n=== Test Values ===\r\n");
                Console.WriteLine("Expected Name: '{0}' | Actual Name: '{1}'", expectedOriginalName, actualOriginalName);
                Console.WriteLine("Expected Hint: '{0}' | Actual Hint: '{1}'", expectedHint, actualHint);
                Console.WriteLine("Expected Ver: '{0}' | Actual Ver: '{1}'", expectedVer, actualVer);
            }
        }

        [TestMethod]
        public void FAESv2_MetaData()
        {
            string filePath = "ExampleFiles/EncryptedFiles/FAESv2.faes";

            string expectedHint = "Hint";
            string expectedVer = "v1.1.0 — v1.1.2";

            string actualHint = string.Empty;
            string actualVer = string.Empty;

            try
            {
                FileAES_Utilities.SetVerboseLogging(true);

                FAES_File faesFile = new FAES_File(filePath);

                actualHint = faesFile.GetPasswordHint();
                if (expectedHint != actualHint)
                    Assert.Fail("Incorrect password hint!");

                actualVer = faesFile.GetEncryptionVersion();
                if (expectedVer != actualVer)
                    Assert.Fail("Incorrect encryption version!");
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
            finally
            {
                Console.WriteLine("\r\n=== Test Values ===\r\n");
                Console.WriteLine("Expected Hint: '{0}' | Actual Hint: '{1}'", expectedHint, actualHint);
                Console.WriteLine("Expected Ver: '{0}' | Actual Ver: '{1}'", expectedVer, actualVer);
            }
        }

        [TestMethod]
        public void FAESv1_MetaData()
        {
            string filePath = "ExampleFiles/EncryptedFiles/FAESv1.faes";

            string expectedVer = "v1.0.0";

            string actualVer = string.Empty;

            try
            {
                FileAES_Utilities.SetVerboseLogging(true);

                FAES_File faesFile = new FAES_File(filePath);

                actualVer = faesFile.GetEncryptionVersion();
                if (expectedVer != actualVer)
                    Assert.Fail("Incorrect encryption version!");
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
            finally
            {
                Console.WriteLine("\r\n=== Test Values ===\r\n");
                Console.WriteLine("Expected Ver: '{0}' | Actual Ver: '{1}'", expectedVer, actualVer);
            }
        }

        [TestMethod]
        public void FAESv0_MetaData()
        {
            string filePath = "ExampleFiles/EncryptedFiles/Legacy.mcrypt";

            string expectedVer = "Pre-v1.0.0";

            string actualVer = string.Empty;

            try
            {
                FileAES_Utilities.SetVerboseLogging(true);

                FAES_File faesFile = new FAES_File(filePath);

                actualVer = faesFile.GetEncryptionVersion();
                if (expectedVer != actualVer)
                    Assert.Fail("Incorrect encryption version!");
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
            finally
            {
                Console.WriteLine("\r\n=== Test Values ===\r\n");
                Console.WriteLine("Expected Ver: '{0}' | Actual Ver: '{1}'", expectedVer, actualVer);
            }
        }
    }
}