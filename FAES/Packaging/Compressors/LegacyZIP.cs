using System.IO;
using System.IO.Compression;

namespace FAES.Packaging
{
    internal class LegacyZIP : ICompressedFAES
    {
        public LegacyZIP()
        { }

        /// <summary>
        /// Compress (LGYZIP) an unencrypted FAES File.
        /// </summary>
        /// <param name="unencryptedFile">Unencrypted FAES File</param>
        /// <returns>Path of the unencrypted, LGYZIP compressed file</returns>
        public string CompressFAESFile(FAES_File unencryptedFile)
        {
            string tempPath = FileAES_IntUtilities.CreateTempPath(unencryptedFile, "LGYZIP_Compress-" + FileAES_IntUtilities.GetDateTimeString());
            string tempRawPath = Path.Combine(tempPath, "contents");
            string tempOutputPath = Path.Combine(Directory.GetParent(tempPath).FullName, Path.ChangeExtension(unencryptedFile.getFileName(), FileAES_Utilities.ExtentionUFAES));

            if (unencryptedFile.isFile())
            {
                FileAES_IntUtilities.CreateTempPath(unencryptedFile);

                using (ZipArchive zip = ZipFile.Open(tempOutputPath, ZipArchiveMode.Create))
                {
                    zip.CreateEntryFromFile(unencryptedFile.getPath(), unencryptedFile.getFileName());
                    zip.Dispose();
                }
            }
            else
            {
                FileAES_IntUtilities.DirectoryCopy(unencryptedFile.getPath(), tempRawPath, true);

                ZipFile.CreateFromDirectory(tempRawPath, tempOutputPath);
            }

            return tempOutputPath;
        }

        /// <summary>
        /// Uncompress an encrypted FAES File.
        /// </summary>
        /// <param name="encryptedFile">Encrypted FAES File</param>
        /// <returns>Path of the encrypted, uncompressed file</returns>
        public string UncompressFAESFile(FAES_File encryptedFile)
        {
            string path = Path.ChangeExtension(encryptedFile.getPath(), FileAES_Utilities.ExtentionUFAES);
            ZipFile.ExtractToDirectory(path, Directory.GetParent(encryptedFile.getPath()).FullName);

            return path;
        }
    }
}
