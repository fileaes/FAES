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
                FileAES_IntUtilities.CreateTempPath(file);

                TarWriterOptions wo = new TarWriterOptions(CompressionType.BZip2, true);

                using (Stream stream = File.OpenWrite(outputPath))
                using (var writer = new TarWriter(stream, wo))
                {
                    writer.Write(file.getPath(), new FileInfo(file.getPath()));
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

        public void UncompressFAESFile(FAES_File file)
        {
            using (Stream stream = File.OpenRead(Path.Combine(Directory.GetParent(file.getPath()).FullName, file.getFileName().Substring(0, file.getFileName().Length - Path.GetExtension(file.getFileName()).Length) + FileAES_Utilities.ExtentionUFAES)))
            {
                var reader = ReaderFactory.Open(stream);
                while (reader.MoveToNextEntry())
                {
                    if (!reader.Entry.IsDirectory)
                    {
                        Console.WriteLine(reader.Entry.Key);
                        reader.WriteEntryToDirectory(Path.GetFullPath(Directory.GetParent(file.getPath()).FullName), new ExtractionOptions() { ExtractFullPath = false, Overwrite = true });
                    }
                }
            }
        }
    }
}
