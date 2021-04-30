using System;
using FAES.Packaging.Compressors;

namespace FAES.Packaging
{
    internal class Compress : ICompressedFAES
    {
        private readonly CompressionMode _compressionMode;
        private readonly CompressionLevel _compressionLevel;
        private readonly int _compressionLevelRaw = -1;

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
        /// Compress an unencrypted FAES File.
        /// </summary>
        /// <param name="unencryptedFile">Unencrypted FAES File</param>
        /// <returns>Path of the unencrypted, compressed file</returns>
        public string CompressFAESFile(FAES_File unencryptedFile)
        {
            switch (_compressionMode)
            {
                case CompressionMode.LZMA:
                    LZMA lzma = new LZMA();
                    Logging.Log(String.Format("Compression Mode: LZMA"), Severity.DEBUG);
                    return lzma.CompressFAESFile(unencryptedFile);
                case CompressionMode.TAR:
                    TAR tar = new TAR();
                    Logging.Log(String.Format("Compression Mode: LZMA"), Severity.DEBUG);
                    return tar.CompressFAESFile(unencryptedFile);
                case CompressionMode.LGYZIP:
                    LegacyZIP legacyZIP = new LegacyZIP();
                    Logging.Log(String.Format("Compression Mode: LEGACYZIP"), Severity.DEBUG);
                    return legacyZIP.CompressFAESFile(unencryptedFile);
                default:
                    {
                        ZIP zip;
                        Logging.Log(String.Format("Compression Mode: ZIP"), Severity.DEBUG);

                        if (_compressionLevelRaw < 0)
                        {
                            Logging.Log(String.Format("Compression Level: {0}", _compressionLevel), Severity.DEBUG);
                            zip = new ZIP(_compressionLevel);
                        }
                        else
                        {
                            Logging.Log(String.Format("Compression Level: {0}", _compressionLevelRaw), Severity.DEBUG);
                            zip = new ZIP(_compressionLevelRaw);
                        }
                        return zip.CompressFAESFile(unencryptedFile);
                    }
            }
        }

        /// <summary>
        /// Decompress an encrypted FAES File.
        /// </summary>
        /// <param name="encryptedFile">Encrypted FAES File</param>
        /// <returns>Path of the encrypted, Decompressed file</returns>
        public string DecompressFAESFile(FAES_File encryptedFile, string overridePath = "")
        {
            string fileCompressionMode = FileAES_Utilities.GetCompressionMode(encryptedFile.getPath());

            Logging.Log(String.Format("Compression Mode: {0}", fileCompressionMode), Severity.DEBUG);

            switch (fileCompressionMode)
            {
                case "LZMA":
                    LZMA lzma = new LZMA();
                    return lzma.DecompressFAESFile(encryptedFile, overridePath);
                case "TAR":
                    TAR tar = new TAR();
                    return tar.DecompressFAESFile(encryptedFile, overridePath);
                case "ZIP":
                    ZIP zip = new ZIP(_compressionLevel);
                    return zip.DecompressFAESFile(encryptedFile, overridePath);
                case "LEGACY":
                case "LEGACYZIP":
                case "LGYZIP":
                    LegacyZIP legacyZip = new LegacyZIP();
                    return legacyZip.DecompressFAESFile(encryptedFile, overridePath);
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
