using System;
using System.Security.Cryptography;
using System.Text;

namespace FAES.AES
{
    internal class CryptUtils
    {
        private const string _faesCryptIdentifier = "FAESv3";

        /// <summary>
        /// Gets the current FAES file format
        /// </summary>
        /// <returns></returns>
        public static string GetCryptIdentifier()
        {
            return _faesCryptIdentifier;
        }

        /// <summary>
        /// Generates a Random Salt
        /// </summary>
        /// <returns>A Random Salt (byte[32])</returns>
        public static byte[] GenerateRandomSalt()
        {
            byte[] data = new byte[32];

            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                for (int i = 0; i < 10; i++)
                {
                    rng.GetBytes(data);
                }
            }
            return data;
        }

        /// <summary>
        /// Converts a string into a padded/truncated byte array
        /// </summary>
        /// <param name="value">String to convert</param>
        /// <param name="limitLength">Limit the length of the password</param>
        /// <param name="maxLength">Max length of the string</param>
        /// <returns>Byte Array</returns>
        public static byte[] ConvertStringToBytes(string value, bool limitLength = false, int maxLength = 64)
        {
            if (limitLength)
            {
                if (value.Length > maxLength)
                    return Encoding.UTF8.GetBytes(value.Substring(0, maxLength));
                else
                    return Encoding.UTF8.GetBytes(value.PadRight(maxLength, '\0'));
            }
            else return Encoding.UTF8.GetBytes(value);
        }

        /// <summary>
        /// Converts a byte array to a string, ensuring all NULL values are trimmed
        /// </summary>
        /// <param name="value">Byte Array</param>
        /// <returns>String</returns>
        public static string ConvertBytesToString(byte[] value)
        {
            if (value != null)
                return Encoding.UTF8.GetString(value).TrimEnd('\0', '\n', '\r');
            else return "";
        }

        /// <summary>
        /// Converts a ChecksumType to a byte array.
        /// </summary>
        /// <param name="checksumHashType">Checksum Type</param>
        /// <returns>Byte Array</returns>
        public static byte[] ConvertChecksumTypeToBytes(Checksums.ChecksumType checksumHashType)
        {
            switch (checksumHashType)
            {
                case Checksums.ChecksumType.SHA1:
                    return BitConverter.GetBytes(1);

                case Checksums.ChecksumType.SHA256:
                    return BitConverter.GetBytes(2);

                case Checksums.ChecksumType.SHA512:
                    return BitConverter.GetBytes(3);

                case Checksums.ChecksumType.SHA384:
                    return BitConverter.GetBytes(4);

                default:
                    return BitConverter.GetBytes(0);
            }
        }

        /// <summary>
        /// Converts a byte array to a ChecksumType.
        /// </summary>
        /// <param name="value">Byte Array</param>
        /// <returns>ChecksumType</returns>
        public static Checksums.ChecksumType ConvertBytesToChecksumType(byte[] value)
        {
            switch (BitConverter.ToUInt16(value, 0))
            {
                case 1:
                    return Checksums.ChecksumType.SHA1;

                case 2:
                    return Checksums.ChecksumType.SHA256;

                case 3:
                    return Checksums.ChecksumType.SHA512;

                case 4:
                    return Checksums.ChecksumType.SHA384;

                default:
                    return Checksums.ChecksumType.NULL;
            }
        }
    }
}