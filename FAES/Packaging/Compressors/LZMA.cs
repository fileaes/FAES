using SharpCompress.Common;
using SharpCompress.Readers;
using SharpCompress.Writers;
using SharpCompress.Writers.Zip;
using System;
using System.IO;

namespace FAES.Packaging
{
    internal class LZMA : ICompressedFAES
    {
        public LZMA()
        { }

        /// <summary>
        /// Compress (LZMA) an unencrypted FAES File.
        /// </summary>
        /// <param name="unencryptedFile">Unencrypted FAES File</param>
        /// <returns>Path of the unencrypted, LZMA compressed file</returns>
        public string CompressFAESFile(FAES_File unencryptedFile)
        {
            string tempPath = FileAES_IntUtilities.CreateTempPath(unencryptedFile, "LZMA_Compress-" + FileAES_IntUtilities.GetDateTimeString());
            string tempRawPath = Path.Combine(tempPath, "contents");
            string tempRawFile = Path.Combine(tempRawPath, unencryptedFile.getFileName());
            string tempOutputPath = Path.Combine(Directory.GetParent(tempPath).FullName, Path.ChangeExtension(unencryptedFile.getFileName(), FileAES_Utilities.ExtentionUFAES));

            if (!Directory.Exists(tempRawPath)) Directory.CreateDirectory(tempRawPath);

            if (unencryptedFile.isFile())
                File.Copy(unencryptedFile.getPath(), tempRawFile);
            else
                FileAES_IntUtilities.DirectoryCopy(unencryptedFile.getPath(), tempRawPath, true);

            WriterOptions wo = new WriterOptions(CompressionType.LZMA);

            using (Stream stream = File.OpenWrite(tempOutputPath))
            using (var writer = WriterFactory.Open(stream, ArchiveType.Zip, wo))
            {
                writer.WriteAll(tempRawPath, "*", SearchOption.AllDirectories);
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

            using (Stream stream = File.OpenRead(path))
            {
                var reader = ReaderFactory.Open(stream);
                while (reader.MoveToNextEntry())
                {
                    reader.WriteEntryToDirectory(Path.GetFullPath(Directory.GetParent(Path.ChangeExtension(encryptedFile.getPath(), FileAES_Utilities.ExtentionUFAES)).FullName), new ExtractionOptions() { ExtractFullPath = true, Overwrite = true });
                }
            }
            return path;
        }
    }
}
