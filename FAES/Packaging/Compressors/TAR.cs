using SharpCompress.Common;
using SharpCompress.Readers;
using SharpCompress.Writers;
using SharpCompress.Writers.Tar;
using System;
using System.IO;


namespace FAES.Packaging
{
    internal class TAR : ICompressedFAES
    {
        public TAR()
        {
        }

        public void CompressFAESFile(FAES_File file, string tempPath, string outputPath)
        {
            if (file.isFile())
            {
                string tempFolderName = FileAES_IntUtilities.genRandomTempFolder(file.getFileName().Substring(0, file.getFileName().Length - Path.GetExtension(file.getFileName()).Length));
                FileAES_Utilities._instancedTempFolders.Add(tempFolderName);
                if (Directory.Exists(Path.Combine(tempPath, tempFolderName))) Directory.Delete(Path.Combine(tempPath, tempFolderName), true);

                Directory.CreateDirectory(Path.Combine(tempPath, tempFolderName));
                File.Copy(file.getPath(), Path.Combine(tempPath, tempFolderName, file.getFileName()));

                TarWriterOptions wo = new TarWriterOptions(CompressionType.BZip2, true);

                using (Stream stream = File.OpenWrite(outputPath))
                using (var writer = new TarWriter(stream, wo))
                {
                    writer.WriteAll(Path.Combine(tempPath, tempFolderName), "*", SearchOption.AllDirectories);
                }
            }
            else
            {
                string tempFolderName = FileAES_IntUtilities.genRandomTempFolder(file.getFileName().Substring(0, file.getFileName().Length - Path.GetExtension(file.getFileName()).Length));
                FileAES_Utilities._instancedTempFolders.Add(tempFolderName);
                if (Directory.Exists(Path.Combine(tempPath, tempFolderName))) Directory.Delete(Path.Combine(tempPath, tempFolderName), true);
                FileAES_IntUtilities.DirectoryCopy(file.getPath(), Path.Combine(tempPath, tempFolderName, file.getFileName()), true);

                TarWriterOptions wo = new TarWriterOptions(CompressionType.BZip2, true);

                using (Stream stream = File.OpenWrite(outputPath))
                using (var writer = new TarWriter(stream, wo))
                {
                    writer.WriteAll(Path.Combine(tempPath, tempFolderName), "*", SearchOption.AllDirectories);
                }
            }
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
