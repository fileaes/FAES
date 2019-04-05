using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAES.Packaging
{
    internal interface ICompressedFAES
    {
        string CompressFAESFile(FAES_File file);

        void UncompressFAESFile(FAES_File encryptedFile, string unencryptedFile);
    }
}
