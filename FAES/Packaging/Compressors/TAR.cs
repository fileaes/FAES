using SharpCompress.Common;
using SharpCompress.Readers;
using SharpCompress.Writers;
using SharpCompress.Writers.Tar;
using System;
using System.IO;


namespace FAES.Packaging.Compressors
{
    internal class TAR : ICompressedFAES
    {
        /// <summary>
        /// Compress (TAR/BZip2) an unencrypted FAES File.
        /// </summary>
        /// <param name="unencryptedFile">Unencrypted FAES File</param>
        /// <returns>Path of the unencrypted, TAR/BZip2 compressed file</returns>
        public string CompressFAESFile(FAES_File unencryptedFile)
        {
            FileAES_IntUtilities.CreateEncryptionFilePath(unencryptedFile, "TAR", out string tempRawPath, out string tempRawFile, out string tempOutputPath);

            TarWriterOptions wo = new TarWriterOptions(CompressionType.BZip2, true);

            using (Stream stream = File.OpenWrite(tempOutputPath))
            using (var writer = new TarWriter(stream, wo))
            {
                writer.WriteAll(tempRawPath, "*", SearchOption.AllDirectories);
            }
            return tempOutputPath;
        }

        /// <summary>
        /// Decompress an encrypted FAES File.
        /// </summary>
        /// <param name="encryptedFile">Encrypted FAES File</param>
        /// <returns>Path of the encrypted, Decompressed file</returns>
        public string DecompressFAESFile(FAES_File encryptedFile, string overridePath = "")
        {
            string path;
            if (!String.IsNullOrWhiteSpace(overridePath))
            {
                path = overridePath;
            }
            else
            {
                path = Path.ChangeExtension(encryptedFile.getPath(), FileAES_Utilities.ExtentionUFAES);
            }

            using (Stream stream = File.OpenRead(path))
            {
                var reader = ReaderFactory.Open(stream);
                while (reader.MoveToNextEntry())
                {
                    reader.WriteEntryToDirectory(Directory.GetParent(Path.ChangeExtension(path, Path.GetExtension(encryptedFile.GetOriginalFileName()))).FullName, new ExtractionOptions() { ExtractFullPath = true, Overwrite = true });
                }
            }
            return path;
        }
    }
}
