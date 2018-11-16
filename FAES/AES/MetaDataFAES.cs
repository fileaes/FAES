using System;
using System.Linq;
using System.Text;

namespace FAES.AES
{
    internal class MetaDataFAES
    {
        protected byte[] _passwordHint;
        protected byte[] _encryptionTimestamp;
        protected byte[] _encryptionVersion;
        protected byte[] _compression;

        /// <summary>
        /// Converts various pieces of MetaData into easy-to-manage method calls
        /// </summary>
        /// <param name="passwordHint">Password Hint</param>
        public MetaDataFAES(string passwordHint, string compressionUsed)
        {
            _passwordHint = ConvertStringToBytes(passwordHint);
            _encryptionTimestamp = BitConverter.GetBytes((int)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds);
            _encryptionVersion = ConvertStringToBytes(FileAES_Utilities.GetVersion(), 16);
            _compression = ConvertStringToBytes(compressionUsed, 6);
        }

        /// <summary>
        /// Converts FAESv2 MetaData into easy-to-manage method calls
        /// </summary>
        /// <param name="metaData">Raw FAESv2 MetaData</param>
        public MetaDataFAES(byte[] metaData)
        {
            _passwordHint = metaData.Take(64).ToArray();
            _encryptionTimestamp = metaData.Skip(64).Take(4).ToArray();
            _encryptionVersion = metaData.Skip(68).Take(16).ToArray();
            _compression = metaData.Skip(84).Take(6).ToArray();
        }

        /// <summary>
        /// Gets the Password Hint
        /// </summary>
        /// <returns>Password Hint stored in MetaData</returns>
        public string getPasswordHint()
        {
            if (_passwordHint != null)
            {
                string converted = ConvertBytesToString(_passwordHint).Replace("¬", "");
                if (converted.Contains("�")) converted = converted.Replace("�", "");
                return converted;
            }
            else
                return null;
        }

        /// <summary>
        /// Gets the UNIX timestamp of when the file was encrypted (UTC time)
        /// </summary>
        /// <returns>UNIX timestamp (UTC)</returns>
        public int getEncryptionTimestamp()
        {
            if (_encryptionTimestamp != null)
                return BitConverter.ToInt32(_encryptionTimestamp, 0);
            else
                return -1;
        }

        /// <summary>
        /// Gets the Compression Method used to compress the encrypted file
        /// </summary>
        /// <returns>Compression Mode Type</returns>
        public string getCompressionMode()
        {
            if (_compression != null)
            {
                string converted = ConvertBytesToString(_compression);

                if (!String.IsNullOrEmpty(converted))
                    return converted;
            }
            return "LEGACYZIP";
        }

        /// <summary>
        /// Gets raw metadata
        /// </summary>
        /// <returns>MetaData byte array (256 bytes)</returns>
        public byte[] getMetaData()
        {
            byte[] formedMetaData = new byte[256];
            Buffer.BlockCopy(_passwordHint, 0, formedMetaData, 0, _passwordHint.Length);
            Buffer.BlockCopy(_encryptionTimestamp, 0, formedMetaData, 64, _encryptionTimestamp.Length);
            Buffer.BlockCopy(_encryptionVersion, 0, formedMetaData, 68, _encryptionVersion.Length);
            Buffer.BlockCopy(_compression, 0, formedMetaData, 84, _compression.Length);

            return formedMetaData;
        }

        /// <summary>
        /// Converts a string into a padded/truncated byte array
        /// </summary>
        /// <param name="value">String to convert</param>
        /// <param name="maxLength">Max length of the string</param>
        /// <param name="paddingChar">Character used to pad the string if required</param>
        /// <returns>Byte Array</returns>
        private byte[] ConvertStringToBytes(string value, int maxLength = 64)
        {
            if (value.Length > maxLength)
                return Encoding.UTF8.GetBytes(value.Substring(0, maxLength));
            else
                return Encoding.UTF8.GetBytes(value.PadRight(maxLength, '\0'));
        }

        /// <summary>
        /// Converts a byte array to a string, ensuring all NULL values are trimmed
        /// </summary>
        /// <param name="value">Byte Array</param>
        /// <returns>String</returns>
        private string ConvertBytesToString(byte[] value)
        {
            string conv = Encoding.UTF8.GetString(value).TrimEnd('\0');
            return conv.Replace("¬", "");
        }
    }
}
