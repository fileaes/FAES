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
        /// Allows for the compression/decompression of FAES Files using a preferred Compression Mode
        /// </summary>
        /// <param name="compressionMode">Compression Mode to use during Compression</param>
        /// <param name="compressionLevel">Compression Level to use during Compression (ZIP only)</param>
        public Compress(CompressionMode compressionMode, CompressionLevel compressionLevel = CompressionLevel.Fastest)
        {
            _compressionMode = compressionMode;
            _compressionLevel = compressionLevel;
        }

        /// <summary>
        /// Allows for the compression/decompression of FAES Files using a preferred Compression Mode
        /// </summary>
        /// <param name="compressionMode">Compression Mode to use during Compression</param>
        /// <param name="compressionLevel">Compression Level to use during Compression (ZIP only)</param>
        public Compress(CompressionMode compressionMode, int compressionLevel)
        {
            _compressionMode = compressionMode;
            _compressionLevelRaw = compressionLevel;
        }

        /// <summary>
        /// Allows for the compression/decompression of FAES Files using a preferred Compression Mode
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
                case Optimise.Store:
                    _compressionMode = CompressionMode.ZIP;
                    _compressionLevel = CompressionLevel.None;
                    break;
                case Optimise.Legacy:
                    _compressionMode = CompressionMode.LGYZIP;
                    _compressionLevel = CompressionLevel.None;
                    break;
                case Optimise.Balanced:
                default:
                    _compressionMode = CompressionMode.ZIP;
                    _compressionLevel = CompressionLevel.Optimal;
                    break;
            }
        }

        /// <summary>
        /// Compress an unencrypted FAES File.
        /// </summary>
        /// <param name="unencryptedFile">Unencrypted FAES File</param>
        /// <param name="percentComplete">Percent complete for compression</param>
        /// <returns>Path of the unencrypted, compressed file</returns>
        public string CompressFAESFile(FAES_File unencryptedFile, ref decimal percentComplete) // TODO: Diagnose Compression Progression not updating till finished
        {
            switch (_compressionMode)
            {
                case CompressionMode.GZIP:
                    GZIP gZip = new GZIP();
                    Logging.Log("Compression Mode: GZIP");
                    return gZip.CompressFAESFile(unencryptedFile, ref percentComplete);
                case CompressionMode.LZMA:
                    LZMA lzma = new LZMA();
                    Logging.Log("Compression Mode: LZMA");
                    return lzma.CompressFAESFile(unencryptedFile, ref percentComplete);
                case CompressionMode.TAR:
                    TAR tar = new TAR();
                    Logging.Log("Compression Mode: TAR");
                    return tar.CompressFAESFile(unencryptedFile, ref percentComplete);
                case CompressionMode.LGYZIP:
                    LegacyZIP legacyZIP = new LegacyZIP();
                    Logging.Log("Compression Mode: LEGACYZIP");
                    return legacyZIP.CompressFAESFile(unencryptedFile, ref percentComplete);
                default:
                    {
                        ZIP zip;
                        Logging.Log("Compression Mode: ZIP");

                        if (_compressionLevelRaw < 0)
                        {
                            Logging.Log($"Compression Level: {_compressionLevel}", Severity.DEBUG);
                            zip = new ZIP(_compressionLevel);
                        }
                        else
                        {
                            Logging.Log($"Compression Level: {_compressionLevelRaw}", Severity.DEBUG);
                            zip = new ZIP(_compressionLevelRaw);
                        }
                        return zip.CompressFAESFile(unencryptedFile, ref percentComplete);
                    }
            }
        }

        /// <summary>
        /// Decompress an encrypted FAES File.
        /// </summary>
        /// <param name="encryptedFile">Encrypted FAES File</param>
        /// <param name="percentComplete">Percent complete for decompression</param>
        /// <param name="overridePath">Override the read path</param>
        /// <returns>Path of the encrypted, Decompressed file</returns>
        public string DecompressFAESFile(FAES_File encryptedFile, ref decimal percentComplete, string overridePath = "")
        {
            string fileCompressionMode = FileAES_Utilities.GetCompressionMode(encryptedFile.GetPath());

            Logging.Log($"Compression Mode: {fileCompressionMode}");

            switch (fileCompressionMode)
            {
                case "GZIP":
                    GZIP gZip = new GZIP();
                    return gZip.DecompressFAESFile(encryptedFile, ref percentComplete, overridePath);
                case "LZMA":
                    LZMA lzma = new LZMA();
                    return lzma.DecompressFAESFile(encryptedFile, ref percentComplete, overridePath);
                case "TAR":
                    TAR tar = new TAR();
                    return tar.DecompressFAESFile(encryptedFile, ref percentComplete, overridePath);
                case "ZIP":
                    ZIP zip = new ZIP(_compressionLevel);
                    return zip.DecompressFAESFile(encryptedFile, ref percentComplete, overridePath);
                case "LEGACY":
                case "LEGACYZIP":
                case "LGYZIP":
                    LegacyZIP legacyZip = new LegacyZIP();
                    return legacyZip.DecompressFAESFile(encryptedFile, ref percentComplete, overridePath);
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
