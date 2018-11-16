using System;

namespace FAES.Packaging
{
    public class Compress : ICompressedFAES
    {
        private CompressionMode _compressionMode;
        private CompressionLevel _compressionLevel;

        /// <summary>
        /// Allows for the compression/decompression of FAES Files using a prefered Compression Mode
        /// </summary>
        /// <param name="compressionMode">Compression Mode to use during Compression</param>
        /// <param name="compressionLevel">Compression Level to use during Compression</param>
        public Compress(CompressionMode compressionMode, CompressionLevel compressionLevel = CompressionLevel.Fastest)
        {
            _compressionMode = compressionMode;
            _compressionLevel = compressionLevel;
        }

        /// <summary>
        /// Allows for the compression/decompression of FAES Files using a prefered Compression Mode
        /// </summary>
        /// <param name="optimisedChoice">A predefined Compression Mode and Level to use during Compression</param>
        public Compress(Optimise optimisedChoice)
        {
            switch (optimisedChoice)
            {
                case Optimise.Faster_Compression:
                    _compressionMode = CompressionMode.ZIP;
                    _compressionLevel = CompressionLevel.Fastest;
                    break;
                case Optimise.Lower_Filesize:
                    _compressionMode = CompressionMode.ZIP;
                    _compressionLevel = CompressionLevel.Slowest;
                    break;
                case Optimise.Balanced:
                    _compressionMode = CompressionMode.ZIP;
                    _compressionLevel = CompressionLevel.Optimal;
                    break;
                case Optimise.Store:
                    _compressionMode = CompressionMode.ZIP;
                    _compressionLevel = CompressionLevel.None;
                    break;
            }
        }

        /// <summary>
        /// Compresses a FAES File
        /// </summary>
        /// <param name="file">FAES File to compress</param>
        /// <param name="tempPath">Temporary File Path</param>
        /// <param name="outputPath">Final Output Path</param>
        public void CompressFAESFile(FAES_File file, string tempPath, string outputPath)
        {
            switch (_compressionMode)
            {
                case CompressionMode.LZMA:
                    LZMA lzma = new LZMA();
                    lzma.CompressFAESFile(file, tempPath, outputPath);
                    break;
                case CompressionMode.TAR:
                    TAR tar = new TAR();
                    tar.CompressFAESFile(file, tempPath, outputPath);
                    break;
                case CompressionMode.ZIP:
                    ZIP zip = new ZIP(_compressionLevel);
                    zip.CompressFAESFile(file, tempPath, outputPath);
                    break;
            }
        }

        /// <summary>
        /// Uncompresses a FAES File
        /// </summary>
        /// <param name="file">FAES File to uncompress</param>
        public void UncompressFAESFile(FAES_File file)
        {
            string fileCompressionMode = FileAES_Utilities.GetCompressionMode(file.getPath());

            switch (fileCompressionMode)
            {
                case "LZMA":
                    LZMA lzma = new LZMA();
                    lzma.UncompressFAESFile(file);
                    break;
                case "TAR":
                    TAR tar = new TAR();
                    tar.UncompressFAESFile(file);
                    break;
                case "ZIP":
                    ZIP zip = new ZIP(_compressionLevel);
                    zip.UncompressFAESFile(file);
                    break;
                case "LEGACYZIP":
                    LegacyZIP legacyZip = new LegacyZIP();
                    legacyZip.UncompressFAESFile(file);
                    break;
                default:
                    throw new NotSupportedException("FAES File was compressed using an unsupported file format.");
            }
        }

        /// <summary>
        /// Gets the current Compression Mode
        /// </summary>
        /// <returns>Compression Mode</returns>
        public CompressionMode GetCompressionMode()
        {
            return _compressionMode;
        }

        /// <summary>
        /// Gets the current Compression Mode as a string
        /// </summary>
        /// <returns>Compression Mode</returns>
        public string GetCompressionModeAsString()
        {
            return Enum.GetName(typeof(CompressionMode), _compressionMode);
        }

        /// <summary>
        /// Gets the current Compression Level
        /// </summary>
        /// <returns>Compression Level</returns>
        public CompressionLevel GetCompressionLevel()
        {
            return _compressionLevel;
        }

        /// <summary>
        /// Gets the current Compression Level as a string
        /// </summary>
        /// <returns>Compression Level</returns>
        public string GetCompressionLevelAsString()
        {
            return Enum.GetName(typeof(CompressionLevel), _compressionLevel);
        }
    }
}
