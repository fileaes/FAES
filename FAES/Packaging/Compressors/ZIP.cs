using System;
using SharpCompress.Common;
using SharpCompress.Readers;
using SharpCompress.Writers;
using SharpCompress.Writers.Zip;
using System.IO;

namespace FAES.Packaging.Compressors
{
    internal class ZIP : ICompressedFAES
    {
        private readonly SharpCompress.Compressors.Deflate.CompressionLevel _compressLevel;

        public ZIP(CompressionLevel level)
        {
            switch (level)
            {
                case CompressionLevel.None:
                    _compressLevel = SharpCompress.Compressors.Deflate.CompressionLevel.None;
                    break;
                case CompressionLevel.Fastest:
                    _compressLevel = SharpCompress.Compressors.Deflate.CompressionLevel.BestSpeed;
                    break;
                case CompressionLevel.Optimal:
                    _compressLevel = SharpCompress.Compressors.Deflate.CompressionLevel.Level7;
                    break;
                case CompressionLevel.Slowest:
                    _compressLevel = SharpCompress.Compressors.Deflate.CompressionLevel.BestCompression;
                    break;
            }
        }

        public ZIP(int level)
        {
            if (level < 0) level = 0;
            else if (level > 9) level = 9;

            _compressLevel = (SharpCompress.Compressors.Deflate.CompressionLevel)level;
        }

        /// <summary>
        /// Compress (ZIP) an unencrypted FAES File.
        /// </summary>
        /// <param name="unencryptedFile">Unencrypted FAES File</param>
        /// <returns>Path of the unencrypted, ZIP compressed file</returns>
        public string CompressFAESFile(FAES_File unencryptedFile)
        {
            FileAES_IntUtilities.CreateEncryptionFilePath(unencryptedFile, "ZIP", out string tempRawPath, out _, out string tempOutputPath);

            ZipWriterOptions wo = new ZipWriterOptions(CompressionType.Deflate)
            {
                DeflateCompressionLevel = _compressLevel
            };

            using (Stream stream = File.OpenWrite(tempOutputPath))
            using (var writer = new ZipWriter(stream, wo))
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
