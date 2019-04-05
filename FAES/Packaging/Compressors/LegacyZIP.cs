using System.IO;
using System.IO.Compression;

namespace FAES.Packaging
{
    internal class LegacyZIP : ICompressedFAES
    {
        public LegacyZIP()
        {

        }

        
        public string CompressFAESFile(FAES_File file)
        {
            string tempPath = FileAES_IntUtilities.CreateTempPath(file, "ZIP_Compress-" + FileAES_IntUtilities.GetDateTimeString());
            string tempRawPath = Path.Combine(tempPath, "contents");
            string tempOutputPath = Path.Combine(Directory.GetParent(tempPath).FullName, Path.ChangeExtension(file.getFileName(), FileAES_Utilities.ExtentionUFAES));

            if (file.isFile())
            {
                FileAES_IntUtilities.CreateTempPath(file);

                using (ZipArchive zip = ZipFile.Open(tempOutputPath, ZipArchiveMode.Create))
                {
                    zip.CreateEntryFromFile(file.getPath(), file.getFileName());
                    zip.Dispose();
                }
            }
            else
            {
                FileAES_IntUtilities.DirectoryCopy(file.getPath(), tempRawPath, true);

                ZipFile.CreateFromDirectory(tempRawPath, tempOutputPath);
            }

            return tempOutputPath;
        }

        public void UncompressFAESFile(FAES_File file, string unencryptedFile)
        {
            ZipFile.ExtractToDirectory(Path.ChangeExtension(file.getPath(), FileAES_Utilities.ExtentionUFAES), Directory.GetParent(file.getPath()).FullName);
        }
    }
}
