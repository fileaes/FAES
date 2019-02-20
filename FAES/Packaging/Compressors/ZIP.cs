using SharpCompress.Common;
using SharpCompress.Readers;
using SharpCompress.Writers;
using SharpCompress.Writers.Zip;
using System;
using System.IO;

namespace FAES.Packaging
{
    internal class ZIP : ICompressedFAES
    {
        private SharpCompress.Compressors.Deflate.CompressionLevel _compressLevel;

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


        public void CompressFAESFile(FAES_File file, string tempPath, string outputPath)
        {
            string tempFolderName = FileAES_IntUtilities.CreateTempPath(file, tempPath);
            if (Directory.Exists(tempFolderName)) Directory.Delete(tempFolderName, true);

            if (!Directory.Exists(Path.GetDirectoryName(outputPath))) Directory.CreateDirectory(Path.GetDirectoryName(outputPath));

            if (file.isFile())
            {
                Directory.CreateDirectory(tempFolderName);
                File.Copy(file.getPath(), Path.Combine(tempFolderName, file.getFileName()));

                ZipWriterOptions wo = new ZipWriterOptions(CompressionType.Deflate);
                wo.DeflateCompressionLevel = _compressLevel;

                using (Stream stream = File.OpenWrite(outputPath))
                using (var writer = new ZipWriter(stream, wo))
                {
                    writer.WriteAll(tempFolderName, "*", SearchOption.AllDirectories);
                }
            }
            else
            {
                FileAES_IntUtilities.DirectoryCopy(file.getPath(), Path.Combine(tempFolderName, file.getFileName()), true);

                ZipWriterOptions wo = new ZipWriterOptions(CompressionType.Deflate);
                wo.DeflateCompressionLevel = _compressLevel;

                using (Stream stream = File.OpenWrite(outputPath))
                using (var writer = new ZipWriter(stream, wo))
                {
                    writer.WriteAll(tempFolderName, "*", SearchOption.AllDirectories);
                }
            }

            FileAES_IntUtilities.DeleteTempPath(file);
        }

        public void UncompressFAESFile(FAES_File file, string unencryptedFile)
        {
            using (Stream stream = File.OpenRead(Path.Combine(Directory.GetParent(unencryptedFile).FullName, Path.GetFileName(unencryptedFile).Substring(0, Path.GetFileName(unencryptedFile).Length - Path.GetExtension(Path.GetFileName(unencryptedFile)).Length) + FileAES_Utilities.ExtentionUFAES)))
            {
                var reader = ReaderFactory.Open(stream);
                while (reader.MoveToNextEntry())
                {
                    reader.WriteEntryToDirectory(Path.GetFullPath(Directory.GetParent(unencryptedFile).FullName), new ExtractionOptions() { ExtractFullPath = true, Overwrite = true });
                }
            }
        }
    }
}
