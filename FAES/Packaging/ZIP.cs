using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAES.Packaging
{
    internal class ZIP : ICompressedFAES
    {
        private CompressionLevel _compressionLevel;

        public ZIP(CompressionLevel compressionLevel)
        {
            _compressionLevel = compressionLevel;
        }

        public void CompressFAESFile(FAES_File file, string tempPath, string outputPath)
        {
            if (file.isFile())
            {
                FileAES_IntUtilities.CreateTempPath(file);

                using (ZipArchive zip = ZipFile.Open(outputPath, ZipArchiveMode.Create))
                {
                    zip.CreateEntryFromFile(file.getPath(), file.getFileName(), _compressionLevel);
                    zip.Dispose();
                }
            }
            else
            {
                string tempFolderName = FileAES_IntUtilities.genRandomTempFolder(file.getFileName().Substring(0, file.getFileName().Length - Path.GetExtension(file.getFileName()).Length));
                FileAES_Utilities._instancedTempFolders.Add(tempFolderName);
                if (Directory.Exists(Path.Combine(tempPath, tempFolderName))) Directory.Delete(Path.Combine(tempPath, tempFolderName), true);
                FileAES_IntUtilities.DirectoryCopy(file.getPath(), Path.Combine(tempPath, tempFolderName, file.getFileName()), true);
                ZipFile.CreateFromDirectory(Path.Combine(tempPath, tempFolderName), outputPath, _compressionLevel, false);
            }
        }

        public void UncompressFAESFile(FAES_File file)
        {
            ZipFile.ExtractToDirectory(Path.Combine(Directory.GetParent(file.getPath()).FullName, file.getFileName().Substring(0, file.getFileName().Length - Path.GetExtension(file.getFileName()).Length) + FileAES_IntUtilities.CompressedPreEncFiletype), Directory.GetParent(file.getPath()).FullName);
        }
    }
}
