using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAES.Packaging
{
    public class Compress : ICompressedFAES
    {
        private CompressionMode _compressionMode;
        private CompressionLevel _compressionLevel;

        public Compress(CompressionMode compressionMode, CompressionLevel compressionLevel = CompressionLevel.Fastest)
        {
            _compressionMode = compressionMode;
            _compressionLevel = compressionLevel;
        }

        public Compress(Optimise optimisedChoice)
        {
            switch (optimisedChoice)
            {
                case Optimise.PreferFasterCompression:
                    _compressionMode = CompressionMode.ZIP;
                    _compressionLevel = CompressionLevel.Fastest;
                    break;
                case Optimise.PreferLowerFilesize:
                    _compressionMode = CompressionMode.ZIP; //Switch to another CompressionMode once added.
                    _compressionLevel = CompressionLevel.Optimal;
                    break;
                case Optimise.PreferBalanced:
                    _compressionMode = CompressionMode.ZIP; //Switch to another CompressionMode once added.
                    _compressionLevel = CompressionLevel.Optimal;
                    break;
                case Optimise.PreferAbsoluteSpeed:
                    _compressionMode = CompressionMode.ZIP;
                    _compressionLevel = CompressionLevel.NoCompression;
                    break;
            }
        }

        public enum CompressionMode
        {
            ZIP
        };

        public enum Optimise
        {
            PreferFasterCompression,
            PreferLowerFilesize,
            PreferBalanced,
            PreferAbsoluteSpeed
        };

        public void CompressFAESFile(FAES_File file, string tempPath, string outputPath)
        {
            switch (_compressionMode)
            {
                case CompressionMode.ZIP:
                    ZIP zip = new ZIP(_compressionLevel);
                    zip.CompressFAESFile(file, tempPath, outputPath);
                    break;
            }
        }

        public void UncompressFAESFile(FAES_File file)
        {
            string fileCompressionMode = FileAES_Utilities.GetCompressionMode(file.getPath());

            switch (fileCompressionMode)
            {
                case "ZIP":
                default:
                    ZIP zip = new ZIP(_compressionLevel);
                    zip.UncompressFAESFile(file);
                    break;
            }
        }

        public CompressionMode GetCompressionMode()
        {
            return _compressionMode;
        }

        public string GetCompressionModeAsString()
        {
            switch (_compressionMode)
            {
                case CompressionMode.ZIP:
                default:
                    return "ZIP";
            }
        }
    }
}
