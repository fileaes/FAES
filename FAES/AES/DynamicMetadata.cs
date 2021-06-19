using System;

namespace FAES.AES
{
    internal class DynamicMetadata
    {
        protected int _totalMetadataSize;
        protected byte[] _metaData, _faesIdentifier, _hashType, _originalFileHash, _encryptionTimestamp, _passwordHint, _compressionMode, _encryptionVersion, _originalFileName;
        protected byte[] _unsupportedMetadata = null;
            
        /// <summary>
        /// Converts variables into easy-to-manage method calls to create a byte array for metadata
        /// </summary>
        /// <param name="checksumHashType">Type of checksum used to generate the file hash</param>
        /// <param name="originalFileHash">File hash of the original file (checksum generated using previous hash type)</param>
        /// <param name="passwordHint">A password hint for the password used to encrypt the file</param>
        /// <param name="compressionModeUsed">Compression mode used to compress the file</param>
        /// <param name="originalFileName">The name of the original file</param>
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
            _totalMetadataSize = _metaData.Length; // Get the size of the entire metadata
            int offset = 4; // Skip ahead 4 bytes (the length of the totalMetadataSize chunk)

            _faesIdentifier = LoadDynamicMetadataChunk(ref offset, ref _totalMetadataSize); // This should always exist. Get the current FAES Identifier (e.g. FAESv3)

            try
            {
                _hashType = LoadDynamicMetadataChunk(ref offset, ref _totalMetadataSize); // Hash types used to hash the original .ufaes file (e.g. SHA256)
                _originalFileHash = LoadDynamicMetadataChunk(ref offset, ref _totalMetadataSize); // Hash of the original .ufaes file
                _encryptionTimestamp = LoadDynamicMetadataChunk(ref offset, ref _totalMetadataSize); // Unix timestamp of encryption time
                _passwordHint = LoadDynamicMetadataChunk(ref offset, ref _totalMetadataSize); // Password Hint
                _encryptionVersion = LoadDynamicMetadataChunk(ref offset, ref _totalMetadataSize); // FAES version used to encrypt the file
                _compressionMode = LoadDynamicMetadataChunk(ref offset, ref _totalMetadataSize); // Compression mode used when compressing the original file
                _originalFileName = LoadDynamicMetadataChunk(ref offset, ref _totalMetadataSize); // Original filename of the unencrypted file
            }
            catch (Exception e)
            {
                string msg = "MetaData (FAESv3) was shorter than expected! This probably means you are decrypting an older file; If so, this isn't a problem. If not, something is wrong.";

                if (FileAES_Utilities.GetVerboseLogging())
                    Logging.Log(String.Format("{0} | {1}", msg, e), Severity.WARN);
                else
                    Logging.Log(msg, Severity.WARN);
            }

            // The file contains unsupported metadata, this is likely from using an older version of FAES on a file encrypted with a newer version
            if (offset != _totalMetadataSize)
            {
                Logging.Log("Unsupported metadata detected! You are likely decrypting a file from a newer FAES version. This shouldn't cause any issues, although this newer metadata will not be readable!", Severity.WARN);
                Array.Copy(_metaData, offset, _unsupportedMetadata, 0, _totalMetadataSize - offset);
            }
        }

        /// <summary>
        /// Gets the data at the current offset
        /// </summary>
        /// <param name="offset">Offset from the beginning of the metadata</param>
        /// <param name="totalSize">Total metadata size</param>
        /// <returns>Data at the current offset</returns>
        private byte[] LoadDynamicMetadataChunk(ref int offset, ref int totalSize)
        {
            if (offset < totalSize)
            {
                int origOffset = offset; // Saves the starting offset
                byte[] chunkSizeBytes = new byte[2]; // chunkSize's are always 2 bytes (ushort)

                Array.Copy(_metaData, offset, chunkSizeBytes, 0, 2); // Gets the size of the next chunk (the data chunk) from the chunkSize
                ushort chunkSize = BitConverter.ToUInt16(chunkSizeBytes, 0); // Size of the data chunk
                byte[] metaDataChunk = new byte[chunkSize];
                offset += 2; // Increase offset to be past the chunkSize chunk

                Array.Copy(_metaData, offset, metaDataChunk, 0, chunkSize); // Gets the data (for the size of the data chunk) from the metadata
                offset += chunkSize; // Increase offset to be past the data chunk

                Logging.Log(String.Format("MetaData | Size: {0}, Converted: {2}, InitialOffset: {3}, FinalOffset: {4}, Raw: {1}", chunkSize, BitConverter.ToString(metaDataChunk), CryptUtils.ConvertBytesToString(metaDataChunk), origOffset, offset), Severity.DEBUG);

                return metaDataChunk; // Return the data from the data chunk
            }
            else
            {
                throw new IndexOutOfRangeException("Metadata cannot be found at this offset!"); // Something is fucked, this shouldn't happen. Corrupted file?
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
            try
            {
                MetaDataBlockCopy(_faesIdentifier, ref formedMetaData, ref offset);
                MetaDataBlockCopy(_hashType, ref formedMetaData, ref offset);
                MetaDataBlockCopy(_originalFileHash, ref formedMetaData, ref offset);
                MetaDataBlockCopy(_encryptionTimestamp, ref formedMetaData, ref offset);
                MetaDataBlockCopy(_passwordHint, ref formedMetaData, ref offset);
                MetaDataBlockCopy(_encryptionVersion, ref formedMetaData, ref offset);
                MetaDataBlockCopy(_compressionMode, ref formedMetaData, ref offset);
                MetaDataBlockCopy(_originalFileName, ref formedMetaData, ref offset);
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred when creating the FAESv3 metadata: " + e);
            }

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
            ushort chunkSize = Convert.ToUInt16(src.Length); // Size of the data chunk (to store within the chunkSize)
            int origOffset = offset;

            Array.Copy(BitConverter.GetBytes(chunkSize), 0, dst, offset, 2); // Store the data chunk size (size of the data within the data chunk)
            offset += 2; // Increase offset to be past the chunkSize chunk
            Array.Copy(src, 0, dst, offset, src.Length); // Store the data chunk data
            offset += src.Length; // Increase offset to be past the data chunk

            Logging.Log(String.Format("MetaData | Size: {0}, Converted: {2}, InitialOffset: {3}, FinalOffset: {4}, Raw: {1}", chunkSize, BitConverter.ToString(src), CryptUtils.ConvertBytesToString(src), origOffset, offset), Severity.DEBUG);
        }

        /// <summary>
        /// Gets the Checksum Hash Type used to hash the original file
        /// </summary>
        /// <returns>The original hash type</returns>
        public Checksums.ChecksumType GetHashType()
        {
            return CryptUtils.ConvertBytesToChecksumType(_hashType);
        }

        /// <summary>
        /// Gets the original file hash
        /// </summary>
        /// <returns>Original file hash</returns>
        public byte[] GetOrigHash()
        {
            return _originalFileHash;
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
                //Removes the old padding character used in older FAES versions, as well as any newlines or special chars. I wish I wasn't stupid and actually zero-padded + checked for special characters in older v1.1.x versions...
                return CryptUtils.ConvertBytesToString(_passwordHint).TrimEnd('\n', '\r', '¬', '�'); 
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
                return ver;
            }
            return "Unknown version!";
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
