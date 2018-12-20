using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAES.Packaging
{
    interface ICompressedFAES
    {
        void CompressFAESFile(FAES_File file, string tempPath, string outputPath);

        void UncompressFAESFile(FAES_File encryptedFile, string unencryptedFile);
    }
}
