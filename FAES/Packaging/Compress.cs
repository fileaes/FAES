using System;

namespace FAES.Packaging
{
    internal class Compress : ICompressedFAES
    {
        private CompressionMode _compressionMode;
        private CompressionLevel _compressionLevel;
        private int _compressionLevelRaw = -1;

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
        /// <param name="compressionMode">Compression Mode to use during Compression</param>
        /// <param name="compressionLevel">Compression Level to use during Compression</param>
        public Compress(CompressionMode compressionMode, int compressionLevel)
        {
            _compressionMode = compressionMode;
            _compressionLevelRaw = compressionLevel;
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
                case Optimise.Legacy:
                    _compressionMode = CompressionMode.LGYZIP;
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
        public string CompressFAESFile(FAES_File file)
        {
            switch (_compressionMode)
            {
                case CompressionMode.LZMA:
                    LZMA lzma = new LZMA();
                    return lzma.CompressFAESFile(file);
                case CompressionMode.TAR:
                    TAR tar = new TAR();
                    return tar.CompressFAESFile(file);
                case CompressionMode.LGYZIP:
                    LegacyZIP legacyZIP = new LegacyZIP();
                    return legacyZIP.CompressFAESFile(file);
                case CompressionMode.ZIP:
                default:
                    {
                        ZIP zip;
                        if (_compressionLevelRaw < 0)
                            zip = new ZIP(_compressionLevel);
                        else
                            zip = new ZIP(_compressionLevelRaw);

                        return zip.CompressFAESFile(file);
                    }
            }
        }

        /// <summary>
        /// Uncompresses a FAES File
        /// </summary>
        /// <param name="file">FAES File to uncompress</param>
        public void UncompressFAESFile(FAES_File encryptedFile, string uFaesFile)
        {
            string fileCompressionMode = FileAES_Utilities.GetCompressionMode(encryptedFile.getPath());

            switch (fileCompressionMode)
            {
                case "LZMA":
                    LZMA lzma = new LZMA();
                    lzma.UncompressFAESFile(encryptedFile, uFaesFile);
                    break;
                case "TAR":
                    TAR tar = new TAR();
                    tar.UncompressFAESFile(encryptedFile, uFaesFile);
                    break;
                case "ZIP":
                    ZIP zip = new ZIP(_compressionLevel);
                    zip.UncompressFAESFile(encryptedFile, uFaesFile);
                    break;
                case "LEGACY":
                case "LEGACYZIP":
                case "LGYZIP":
                    LegacyZIP legacyZip = new LegacyZIP();
                    legacyZip.UncompressFAESFile(encryptedFile, uFaesFile);
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
