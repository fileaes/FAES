using FAES.AES.Compatability;
using System;
using System.IO;

namespace FAES.AES
{
    public class MetaData
    {
        private bool _usingCompatabilityMode = false;

        private DynamicMetadata _dynamicMetadata;
        private MetaDataFAES _compatabilityMetadata;

        public MetaData(Checksums.ChecksumType checksumHashType, byte[] originalFileHash, string passwordHint, string compressionModeUsed)
        {
            _dynamicMetadata = new DynamicMetadata(checksumHashType, originalFileHash, passwordHint, compressionModeUsed);
        }

        public MetaData(byte[] metaData)
        {
            InitWithBytes(metaData);
        }

        public MetaData(FAES_File faesFile)
        {
            LoadMetaDataFromFile(faesFile.getPath());
        }

        public MetaData(string filePath)
        {
            LoadMetaDataFromFile(filePath);
        }

        private void LoadMetaDataFromFile(string filePath)
        {
            try
            {
                int metaSize;
                byte[] metaSizeBytes = new byte[4];

                FileStream metaSizeStream = new FileStream(filePath, FileMode.Open);
                metaSizeStream.Read(metaSizeBytes, 0, 4);
                metaSizeStream.Close();

                metaSize = BitConverter.ToInt32(metaSizeBytes, 0);
                byte[] faesMetaData = new byte[metaSize];

                FileStream faesMeta = new FileStream(filePath, FileMode.Open);
                faesMeta.Read(faesMetaData, 0, metaSize);
                faesMeta.Close();

                try
                {
                    if (metaSize < 7)
                    {
                        LoadLegacyMetaDataFromFile(filePath);
                    }
                    else
                    {
                        ushort faesIdentifierSize;
                        byte[] faesIdentifierSizeBytes = new byte[2];

                        Array.Copy(faesMetaData, 4, faesIdentifierSizeBytes, 0, 2);
                        faesIdentifierSize = BitConverter.ToUInt16(faesIdentifierSizeBytes, 0);

                        byte[] faesIdentifierBytes = new byte[faesIdentifierSize];

                        Array.Copy(faesMetaData, 6, faesIdentifierBytes, 0, Convert.ToInt32(faesIdentifierSize));

                        if (CryptUtils.ConvertBytesToString(faesIdentifierBytes) != CryptUtils.GetCryptIdentifier())
                        {
                            LoadLegacyMetaDataFromFile(filePath);
                        }
                        else
                        {
                            InitWithBytes(faesMetaData);
                        }
                    }
                }
                catch (IOException)
                {
                    Logging.Log("File is already open in another program!", Severity.ERROR);
                }
                catch (Exception)
                {
                    LoadLegacyMetaDataFromFile(filePath);
                }
            }
            catch (Exception)
            {
                LoadLegacyMetaDataFromFile(filePath);
            }
        }

        private void LoadLegacyMetaDataFromFile(string filePath)
        {
            _usingCompatabilityMode = true;

            try
            {
                byte[] metaSizeBytes = new byte[256];

                FileStream faesFileStream = new FileStream(filePath, FileMode.Open);
                faesFileStream.Read(metaSizeBytes, 0, 256);
                faesFileStream.Close();

                _compatabilityMetadata = new LegacyCrypt().GetAllMetaData(filePath);

                if (_compatabilityMetadata == null) _compatabilityMetadata = new MetaDataFAES(null);
            }
            catch (IOException)
            {
                Logging.Log("File is already open in another program!", Severity.ERROR);
            }
        }

        private void InitWithBytes(byte[] metaData)
        {
            if (!_usingCompatabilityMode)
                _dynamicMetadata = new DynamicMetadata(metaData);
        }

        public bool IsLegacyVersion()
        {
            return _usingCompatabilityMode;
        }

        /// <summary>
        /// Checks the metadata to see if the file can be decrypted
        /// </summary>
        /// <param name="filePath">Path to file (Compatability)</param>
        /// <returns>If the file can be decrypted</returns>
        public bool IsDecryptable(string filePath)
        {
            if (_usingCompatabilityMode)
                return new LegacyCrypt().IsDecryptable(filePath);
            else if (_dynamicMetadata.GetFaesIdentifier() == CryptUtils.GetCryptIdentifier())
                return true;
            else
                return false;
        }

        /// <summary>
        /// Gets the Password Hint
        /// </summary>
        /// <returns>Password Hint stored in MetaData</returns>
        public string GetPasswordHint()
        {
            if (_usingCompatabilityMode)
                return _compatabilityMetadata.GetPasswordHint();
            else
                return _dynamicMetadata.GetPasswordHint();
        }

        /// <summary>
        /// Gets the UNIX timestamp of when the file was encrypted (UTC time)
        /// </summary>
        /// <returns>UNIX timestamp (UTC)</returns>
        public long GetEncryptionTimestamp()
        {
            if (_usingCompatabilityMode)
                return _compatabilityMetadata.GetEncryptionTimestamp();
            else
                return _dynamicMetadata.GetEncryptionTimestamp();
        }

        /// <summary>
        /// Gets the Version of FAES used to encrypt the file
        /// </summary>
        /// <returns>FAES Version</returns>
        public string GetEncryptionVersion()
        {
            if (_usingCompatabilityMode)
                return _compatabilityMetadata.GetEncryptionVersion();
            else
                return _dynamicMetadata.GetEncryptionVersion();
        }

        /// <summary>
        /// Gets the Compression Method used to compress the encrypted file
        /// </summary>
        /// <returns>Compression Mode Type</returns>
        public string GetCompressionMode()
        {
            if (_usingCompatabilityMode)
                return _compatabilityMetadata.GetCompressionMode();
            else
                return _dynamicMetadata.GetCompressionMode();
        }

        /// <summary>
        /// Gets raw metadata
        /// </summary>
        /// <returns>MetaData byte array (256 bytes)</returns>
        public byte[] GetMetaData()
        {
            if (_usingCompatabilityMode)
                return _compatabilityMetadata.GetMetaData();
            else
                return _dynamicMetadata.GetMetaData();
        }

        /// <summary>
        /// Total size of the MetaData
        /// </summary>
        /// <returns>MetaData size (bytes)</returns>
        public int GetLength()
        {
            if (_usingCompatabilityMode) return 256;
            else return _dynamicMetadata.GetLength();
        }
    }
}
