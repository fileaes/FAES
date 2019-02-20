using System.IO;
using System.IO.Compression;

namespace FAES.Packaging
{
    internal class LegacyZIP : ICompressedFAES
    {
        public LegacyZIP()
        {

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

                using (ZipArchive zip = ZipFile.Open(outputPath, ZipArchiveMode.Create))
                {
                    zip.CreateEntryFromFile(Path.Combine(tempFolderName, file.getFileName()), file.getFileName());
                    zip.Dispose();
                }

                /*
                 * Fully Legacy
                FileAES_IntUtilities.CreateTempPath(file);

                using (ZipArchive zip = ZipFile.Open(outputPath, ZipArchiveMode.Create))
                {
                    zip.CreateEntryFromFile(file.getPath(), file.getFileName());
                    zip.Dispose();
                }*/
            }
            else
            {
                FileAES_IntUtilities.DirectoryCopy(file.getPath(), Path.Combine(tempFolderName, file.getFileName()), true);

                if (Directory.Exists(tempFolderName)) Directory.Delete(tempFolderName, true);
                FileAES_IntUtilities.DirectoryCopy(file.getPath(), Path.Combine(tempFolderName, file.getFileName()), true);
                ZipFile.CreateFromDirectory(tempFolderName, outputPath);
            }
        }

        public void UncompressFAESFile(FAES_File file, string unencryptedFile)
        {
            ZipFile.ExtractToDirectory(Path.ChangeExtension(file.getPath(), FileAES_Utilities.ExtentionUFAES), Directory.GetParent(file.getPath()).FullName);
        }
    }
}
