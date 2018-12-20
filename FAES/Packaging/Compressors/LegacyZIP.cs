using System.IO;
using System.IO.Compression;

namespace FAES.Packaging
{
    internal class LegacyZIP : ICompressedFAES
    {
        private SharpCompress.Compressors.Deflate.CompressionLevel _compressLevel;

        public LegacyZIP()
        {

        }

        public void CompressFAESFile(FAES_File file, string tempPath, string outputPath)
        {
            if (file.isFile())
            {
                FileAES_IntUtilities.CreateTempPath(file);

                using (ZipArchive zip = ZipFile.Open(outputPath, ZipArchiveMode.Create))
                {
                    zip.CreateEntryFromFile(file.getPath(), file.getFileName());
                    zip.Dispose();
                }
            }
            else
            {
                string tempFolderName = FileAES_IntUtilities.genRandomTempFolder(file.getFileName().Substring(0, file.getFileName().Length - Path.GetExtension(file.getFileName()).Length));
                FileAES_Utilities._instancedTempFolders.Add(tempFolderName);
                if (Directory.Exists(Path.Combine(tempPath, tempFolderName))) Directory.Delete(Path.Combine(tempPath, tempFolderName), true);
                FileAES_IntUtilities.DirectoryCopy(file.getPath(), Path.Combine(tempPath, tempFolderName, file.getFileName()), true);

                if (Directory.Exists(Path.Combine(tempPath, tempFolderName))) Directory.Delete(Path.Combine(tempPath, tempFolderName), true);
                FileAES_IntUtilities.DirectoryCopy(file.getPath(), Path.Combine(tempPath, tempFolderName, file.getFileName()), true);
                ZipFile.CreateFromDirectory(Path.Combine(tempPath, tempFolderName), outputPath);
            }
        }

        public void UncompressFAESFile(FAES_File file, string unencryptedFile)
        {
            ZipFile.ExtractToDirectory(Path.ChangeExtension(file.getPath(), FileAES_Utilities.ExtentionUFAES), Directory.GetParent(file.getPath()).FullName);
        }
    }
}
