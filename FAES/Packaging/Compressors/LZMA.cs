using SharpCompress.Common;
using SharpCompress.Readers;
using SharpCompress.Writers;
using System;
using System.IO;

namespace FAES.Packaging.Compressors
{
    internal class LZMA : ICompressedFAES
    {
        /// <summary>
        /// Compress (LZMA) an unencrypted FAES File.
        /// </summary>
        /// <param name="unencryptedFile">Unencrypted FAES File</param>
        /// <returns>Path of the unencrypted, LZMA compressed file</returns>
        public string CompressFAESFile(FAES_File unencryptedFile)
        {
            FileAES_IntUtilities.CreateEncryptionFilePath(unencryptedFile, "LZMA", out string tempRawPath, out _, out string tempOutputPath);

            WriterOptions wo = new WriterOptions(CompressionType.LZMA);

            using (Stream stream = File.OpenWrite(tempOutputPath))
            using (var writer = WriterFactory.Open(stream, ArchiveType.Zip, wo))
            {
                writer.WriteAll(tempRawPath, "*", SearchOption.AllDirectories);
            }

            return tempOutputPath;
        }

        /// <summary>
        /// Decompress an encrypted FAES File.
        /// </summary>
        /// <param name="encryptedFile">Encrypted FAES File</param>
        /// <param name="overridePath">Override the read path</param>
        /// <returns>Path of the encrypted, Decompressed file</returns>
        public string DecompressFAESFile(FAES_File encryptedFile, string overridePath = "")
        {
            string path;
            if (!String.IsNullOrWhiteSpace(overridePath))
                path = overridePath;
            else
                path = Path.ChangeExtension(encryptedFile.GetPath(), FileAES_Utilities.ExtentionUFAES);

            using (Stream stream = File.OpenRead(path))
            {
                var reader = ReaderFactory.Open(stream);
                while (reader.MoveToNextEntry())
                {
                    reader.WriteEntryToDirectory(Directory.GetParent(Path.ChangeExtension(path, Path.GetExtension(encryptedFile.GetOriginalFileName())))?.FullName, new ExtractionOptions() { ExtractFullPath = true, Overwrite = true });
                }
            }
            return path;
        }
    }
}
