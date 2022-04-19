using System;
using System.IO;
using System.IO.Compression;

namespace FAES.Packaging.Compressors
{
    internal class LegacyZIP : ICompressedFAES
    {
        /// <summary>
        /// Compress (LGYZIP) an unencrypted FAES File.
        /// </summary>
        /// <param name="unencryptedFile">Unencrypted FAES File</param>
        /// <param name="percentComplete">Percent complete for compression</param>
        /// <returns>Path of the unencrypted, LGYZIP compressed file</returns>
        public string CompressFAESFile(FAES_File unencryptedFile, ref decimal percentComplete)
        {
            FileAES_IntUtilities.CreateEncryptionFilePath(unencryptedFile, "LGYZIP", out string tempRawPath, out _, out string tempOutputPath);

            Logging.Log("LGYZIP does not support percent complete reporting!", Severity.WARN);

            percentComplete = 99;
            if (unencryptedFile.IsFile())
            {
                using (ZipArchive zip = ZipFile.Open(tempOutputPath, ZipArchiveMode.Create))
                {
                    zip.CreateEntryFromFile(unencryptedFile.GetPath(), unencryptedFile.GetFileName());
                    zip.Dispose();
                }
            }
            else
            {
                FileAES_IntUtilities.DirectoryCopy(unencryptedFile.GetPath(), tempRawPath);

                ZipFile.CreateFromDirectory(tempRawPath, tempOutputPath);
            }

            return tempOutputPath;
        }

        /// <summary>
        /// Decompress an encrypted FAES File.
        /// </summary>
        /// <param name="encryptedFile">Encrypted FAES File</param>
        /// <param name="overridePath">Override the read path</param>
        /// <returns>Path of the encrypted, Decompressed file</returns>
        public string DecompressFAESFile(FAES_File encryptedFile, ref decimal percentComplete, string overridePath = "")
        {
            Logging.Log("LGYZIP does not support percent complete reporting!", Severity.WARN);

            string path;
            path = !String.IsNullOrWhiteSpace(overridePath) ? overridePath : Path.ChangeExtension(encryptedFile.GetPath(), FileAES_Utilities.ExtentionUFAES);

            percentComplete = 99;
            ZipFile.ExtractToDirectory(path, Directory.GetParent(path)?.FullName);

            return path;
        }
    }
}
