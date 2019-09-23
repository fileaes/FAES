using System;

namespace FAES.AES
{
    internal class DynamicMetadata
    {
        protected int _totalMetadataSize;
        protected byte[] _metaData, _faesIdentifier, _hashType, _originalFileHash, _encryptionTimestamp, _passwordHint, _compressionMode, _encryptionVersion, _originalFileName;
        protected byte[] _unsupportedMetadata = null;
            
        /// <summary>
        /// Converts FAESv3 MetaData into easy-to-manage method calls
        /// </summary>
        /// <param name="metaData">Raw FAESv3 MetaData</param>
        public DynamicMetadata(Checksums.ChecksumType checksumHashType, byte[] originalFileHash, string passwordHint, string compressionModeUsed, string originalFileName)
        {
            _faesIdentifier = CryptUtils.ConvertStringToBytes(CryptUtils.GetCryptIdentifier());
            _hashType = CryptUtils.ConvertChecksumTypeToBytes(checksumHashType);
            _originalFileHash = originalFileHash;
            _encryptionTimestamp = BitConverter.GetBytes((long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds);
            _passwordHint = CryptUtils.ConvertStringToBytes(passwordHint.TrimEnd('\n', '\r'));
            _encryptionVersion = CryptUtils.ConvertStringToBytes(FileAES_Utilities.GetVersion());
            _compressionMode = CryptUtils.ConvertStringToBytes(compressionModeUsed);
            _originalFileName = CryptUtils.ConvertStringToBytes(originalFileName);

            _totalMetadataSize = (4 + 2 + _faesIdentifier.Length + 2 + _hashType.Length + 2 + _originalFileHash.Length + 2 + _encryptionTimestamp.Length + 2 + _passwordHint.Length + 2 + _encryptionVersion.Length + 2 + _compressionMode.Length + 2 + _originalFileName.Length);
        }

        /// <summary>
        /// Converts FAESv3 MetaData into easy-to-manage method calls
        /// </summary>
        /// <param name="metaData">Raw FAESv3 MetaData</param>
        public DynamicMetadata(byte[] metaData)
        {
            _metaData = metaData;
            _totalMetadataSize = Convert.ToUInt16(_metaData.Length);
            int offset = 4;

            _faesIdentifier = loadDynamicMetadataChunk(ref offset);

            try
            {
                _hashType = loadDynamicMetadataChunk(ref offset);
                _originalFileHash = loadDynamicMetadataChunk(ref offset);
                _encryptionTimestamp = loadDynamicMetadataChunk(ref offset);
                _passwordHint = loadDynamicMetadataChunk(ref offset);
                _encryptionVersion = loadDynamicMetadataChunk(ref offset);
                _compressionMode = loadDynamicMetadataChunk(ref offset);
                _originalFileName = loadDynamicMetadataChunk(ref offset);
            }
            catch
            { }

            if (offset != _totalMetadataSize)
            {
                Array.Copy(_metaData, offset, _unsupportedMetadata, 0, _totalMetadataSize - offset);
            }
        }

        /// <summary>
        /// Gets the data at the current offset
        /// </summary>
        /// <param name="offset">Offset from the beginning of the metadata</param>
        /// <returns>Data at the current offset</returns>
        private byte[] loadDynamicMetadataChunk(ref int offset)
        {
            ushort chunkSize;
            byte[] chunkSizeBytes = new byte[2];

            byte[] metaDataChunk;

            try
            {
                Array.Copy(_metaData, offset, chunkSizeBytes, 0, 2);
                chunkSize = BitConverter.ToUInt16(chunkSizeBytes, 0);
                metaDataChunk = new byte[chunkSize];
                offset += 2;

                Array.Copy(_metaData, offset, metaDataChunk, 0, chunkSize);
                offset += chunkSize;

                return metaDataChunk;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gets raw metadata
        /// </summary>
        /// <returns>MetaData byte array</returns>
        public byte[] GetMetaData()
        {
            byte[] formedMetaData = new byte[_totalMetadataSize];
            byte[] totalSizeBytes = BitConverter.GetBytes(_totalMetadataSize);
            int offset = 0;

            // Total Size
            Array.Copy(totalSizeBytes, 0, formedMetaData, offset, totalSizeBytes.Length);
            offset += totalSizeBytes.Length;

            // Metadata Chunks
            MetaDataBlockCopy(_faesIdentifier, ref formedMetaData, ref offset);
            MetaDataBlockCopy(_hashType, ref formedMetaData, ref offset);
            MetaDataBlockCopy(_originalFileHash, ref formedMetaData, ref offset);
            MetaDataBlockCopy(_encryptionTimestamp, ref formedMetaData, ref offset);
            MetaDataBlockCopy(_passwordHint, ref formedMetaData, ref offset);
            MetaDataBlockCopy(_encryptionVersion, ref formedMetaData, ref offset);
            MetaDataBlockCopy(_compressionMode, ref formedMetaData, ref offset);
            MetaDataBlockCopy(_originalFileName, ref formedMetaData, ref offset);

            return formedMetaData;
        }

        /// <summary>
        /// Automatically adds a Metadata Chunk byte array to a FAESv3 Metadata structure
        /// </summary>
        /// <param name="src">Metadata Chunk</param>
        /// <param name="dst">FAESv3 Metadata structure</param>
        /// <param name="offset">Current offset</param>
        private void MetaDataBlockCopy(byte[] src, ref byte[] dst, ref int offset)
        {
            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(src.Length)), 0, dst, offset, 2);
            offset += 2;
            Array.Copy(src, 0, dst, offset, src.Length);
            offset += src.Length;
        }

        /// <summary>
        /// Gets the Original Filename
        /// </summary>
        /// <returns>Gets the Original Filename stored in MetaData</returns>
        public string GetOriginalFileName()
        {
            return CryptUtils.ConvertBytesToString(_originalFileName);
        }

        /// <summary>
        /// Gets the FAES Identifier
        /// </summary>
        /// <returns>Gets the FAES Identifier stored in MetaData</returns>
        public string GetFaesIdentifier()
        {
            return CryptUtils.ConvertBytesToString(_faesIdentifier);
        }

        /// <summary>
        /// Gets the Password Hint
        /// </summary>
        /// <returns>Password Hint stored in MetaData</returns>
        public string GetPasswordHint()
        {
            if (_passwordHint != null)
                return CryptUtils.ConvertBytesToString(_passwordHint).TrimEnd('\n', '\r', '¬', '�'); //Removes the old padding character used in older FAES versions, as well as any newlines or special chars
            else
                return "No Password Hint Set";
        }

        /// <summary>
        /// Gets the UNIX timestamp of when the file was encrypted (UTC time)
        /// </summary>
        /// <returns>UNIX timestamp (UTC)</returns>
        public long GetEncryptionTimestamp()
        {
            if (_encryptionTimestamp != null)
                return BitConverter.ToInt64(_encryptionTimestamp, 0);
            else
                return -1;
        }

        /// <summary>
        /// Gets the Version of FAES used to encrypt the file
        /// </summary>
        /// <returns>FAES Version</returns>
        public string GetEncryptionVersion()
        {
            if (_encryptionVersion != null)
            {
                string ver = CryptUtils.ConvertBytesToString(_encryptionVersion);

                if (ver.Contains("DEV"))
                    return ver.Split('_')[0];
                else
                    return ver;
            }
            else return "Unknown version!";
        }

        /// <summary>
        /// Gets the Compression Method used to compress the encrypted file
        /// </summary>
        /// <returns>Compression Mode Type</returns>
        public string GetCompressionMode()
        {
            if (_compressionMode != null)
            {
                string converted = CryptUtils.ConvertBytesToString(_compressionMode);

                if (!String.IsNullOrEmpty(converted))
                    return converted;
            }
            return "LGYZIP";
        }

        /// <summary>
        /// Total size of the MetaData
        /// </summary>
        /// <returns>MetaData size (bytes)</returns>
        public int GetLength()
        {
            return _totalMetadataSize;
        }
    }
}
