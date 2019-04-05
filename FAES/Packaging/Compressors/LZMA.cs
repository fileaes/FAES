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
        {

        }

        public string CompressFAESFile(FAES_File file)
        {
            string tempPath = FileAES_IntUtilities.CreateTempPath(file, "LZMA_Compress-" + FileAES_IntUtilities.GetDateTimeString());
            string tempRawPath = Path.Combine(tempPath, "contents");
            string tempRawFile = Path.Combine(tempRawPath, file.getFileName());
            string tempOutputPath = Path.Combine(Directory.GetParent(tempPath).FullName, Path.ChangeExtension(file.getFileName(), FileAES_Utilities.ExtentionUFAES));

            if (!Directory.Exists(tempRawPath)) Directory.CreateDirectory(tempRawPath);

            if (file.isFile())
                File.Copy(file.getPath(), tempRawFile);
            else
                FileAES_IntUtilities.DirectoryCopy(file.getPath(), tempRawPath, true);

            WriterOptions wo = new WriterOptions(CompressionType.LZMA);

            using (Stream stream = File.OpenWrite(tempOutputPath))
            using (var writer = WriterFactory.Open(stream, ArchiveType.Zip, wo))
            {
                writer.WriteAll(tempRawPath, "*", SearchOption.AllDirectories);
            }

            return tempOutputPath;
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
