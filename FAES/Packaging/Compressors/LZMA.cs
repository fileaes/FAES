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

        public void CompressFAESFile(FAES_File file, string tempPath, string outputPath)
        {
            if (file.isFile())
            {
                FileAES_IntUtilities.CreateTempPath(file);

                WriterOptions wo = new WriterOptions(CompressionType.LZMA);

                using (Stream stream = File.OpenWrite(outputPath))
                using (var writer = WriterFactory.Open(stream, ArchiveType.Zip, wo))
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

                WriterOptions wo = new WriterOptions(CompressionType.LZMA);

                using (Stream stream = File.OpenWrite(outputPath))
                using (var writer = WriterFactory.Open(stream, ArchiveType.Zip, wo))
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
