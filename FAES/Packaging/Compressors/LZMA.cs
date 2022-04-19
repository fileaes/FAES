using SharpCompress.Common;
using SharpCompress.Readers;
using SharpCompress.Writers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FAES.Packaging.Compressors
{
    internal class LZMA : ICompressedFAES
    {
        /// <summary>
        /// Compress (LZMA) an unencrypted FAES File.
        /// </summary>
        /// <param name="unencryptedFile">Unencrypted FAES File</param>
        /// <param name="percentComplete">Percent complete for compression</param>
        /// <returns>Path of the unencrypted, LZMA compressed file</returns>
        public string CompressFAESFile(FAES_File unencryptedFile, ref decimal percentComplete)
        {
            FileAES_IntUtilities.CreateEncryptionFilePath(unencryptedFile, "LZMA", out string tempRawPath, out _, out string tempOutputPath);

            WriterOptions wo = new WriterOptions(CompressionType.LZMA);

            using (Stream stream = File.OpenWrite(tempOutputPath))
            using (var writer = WriterFactory.Open(stream, ArchiveType.Zip, wo))
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
            if (!String.IsNullOrWhiteSpace(overridePath))
                path = overridePath;
            else
                path = Path.ChangeExtension(encryptedFile.GetPath(), FileAES_Utilities.ExtentionUFAES);

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
