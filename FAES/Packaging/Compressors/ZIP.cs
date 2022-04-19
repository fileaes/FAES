using System;
using System.Collections.Generic;
using SharpCompress.Common;
using SharpCompress.Readers;
using SharpCompress.Writers;
using SharpCompress.Writers.Zip;
using System.IO;
using System.Linq;

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
                case CompressionLevel.Slowest:
                    _compressLevel = SharpCompress.Compressors.Deflate.CompressionLevel.BestCompression;
                    break;
                case CompressionLevel.Optimal:
                default:
                    _compressLevel = SharpCompress.Compressors.Deflate.CompressionLevel.Level7;
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
        /// <param name="percentComplete">Percent complete for compression</param>
        /// <returns>Path of the unencrypted, ZIP compressed file</returns>
        public string CompressFAESFile(FAES_File unencryptedFile, ref decimal percentComplete)
        {
            FileAES_IntUtilities.CreateEncryptionFilePath(unencryptedFile, "ZIP", out string tempRawPath, out _, out string tempOutputPath);

            ZipWriterOptions wo = new ZipWriterOptions(CompressionType.Deflate)
            {
                DeflateCompressionLevel = _compressLevel
            };

            using (Stream stream = File.OpenWrite(tempOutputPath))
            using (var writer = new ZipWriter(stream, wo))
            {
                List<string> filesList = Directory.EnumerateFiles(tempRawPath, "*", SearchOption.AllDirectories).ToList();
                int maxFiles = filesList.Count;

                for (int i = 0; i < filesList.Count; i++)
                {
                    string file = filesList[i];
                    string archivePath = file.Replace(tempRawPath, "");
                    writer.Write(archivePath, file);

                    try
                    {
                        percentComplete = Math.Ceiling((decimal)((Convert.ToDouble(i + 1) / Convert.ToDouble(maxFiles)) * 100));
                        if (percentComplete > 100) percentComplete = 100;
                    }
                    catch (Exception)
                    {
                        percentComplete = 100;
                    }
                }
                percentComplete = 100;
            }
            return tempOutputPath;
        }

        /// <summary>
        /// Decompress an encrypted FAES File.
        /// </summary>
        /// <param name="encryptedFile">Encrypted FAES File</param>
        /// <param name="percentComplete">Percent complete for decompression</param>
        /// <param name="overridePath">Override the read path</param>
        /// <returns>Path of the encrypted, Decompressed file</returns>
        public string DecompressFAESFile(FAES_File encryptedFile, ref decimal percentComplete, string overridePath = "")
        {
            string path;
            path = !String.IsNullOrWhiteSpace(overridePath) ? overridePath : Path.ChangeExtension(encryptedFile.GetPath(), FileAES_Utilities.ExtentionUFAES);

            using (Stream stream = File.OpenRead(path))
            {
                var reader = ReaderFactory.Open(stream);

                while (reader.MoveToNextEntry())
                {
                    try
                    {
                        percentComplete = Math.Ceiling((decimal)((Convert.ToDouble(stream.Position) / Convert.ToDouble(stream.Length)) * 100));
                        if (percentComplete > 100) percentComplete = 100;
                    }
                    catch (Exception)
                    {
                        percentComplete = 100;
                    }

                    reader.WriteEntryToDirectory(Directory.GetParent(Path.ChangeExtension(path, Path.GetExtension(encryptedFile.GetOriginalFileName())))?.FullName, new ExtractionOptions() { ExtractFullPath = true, Overwrite = true });
                }
            }
            return path;
        }
    }
}
