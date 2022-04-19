using System;
using System.IO;
using System.Security.Cryptography;

namespace FAES
{
    public class Checksums
    {
        /// <summary>
        /// Converts Hash Byte Array to Hash String
        /// </summary>
        /// <param name="hash">Hash Byte Array</param>
        /// <returns>Hash String</returns>
        public static string ConvertHashToString(byte[] hash)
        {
            return BitConverter.ToString(hash).Replace("-", "");
        }

        /// <summary>
        /// Compares two hashes
        /// </summary>
        /// <param name="firstHash">First Hash to compare</param>
        /// <param name="secondHash">Second Hash to compare</param>
        /// <returns>If the hashes match</returns>
        public static bool CompareHash(byte[] firstHash, byte[] secondHash)
        {
            return CompareHash(ConvertHashToString(firstHash), ConvertHashToString(secondHash));
        }

        /// <summary>
        /// Compares two hashes
        /// </summary>
        /// <param name="firstHash">First Hash to compare</param>
        /// <param name="secondHash">Second Hash to compare</param>
        /// <returns>If the hashes match</returns>
        public static bool CompareHash(string firstHash, string secondHash)
        {
            return (firstHash == secondHash);
        }

        /// <summary>
        /// Gets the SHA1 Hash of a selected file
        /// </summary>
        /// <param name="inputFile">File to get SHA1 Hash of</param>
        /// <returns>The Files SHA1 Hash</returns>
        public static byte[] GetSHA1(string inputFile)
        {
            using (var sha1 = SHA1.Create())
            using (var stream = File.OpenRead(inputFile))
                return sha1.ComputeHash(stream);
        }

        /// <summary>
        /// Gets the SHA256 Hash of a selected file
        /// </summary>
        /// <param name="inputFile">File to get SHA256 Hash of</param>
        /// <returns>The Files SHA256 Hash</returns>
        public static byte[] GetSHA256(string inputFile)
        {
            using (var sha256 = SHA256.Create())
            using (var stream = File.OpenRead(inputFile))
                return sha256.ComputeHash(stream);
        }

        /// <summary>
        /// Gets the SHA384 Hash of a selected file
        /// </summary>
        /// <param name="inputFile">File to get SHA384 Hash of</param>
        /// <returns>The Files SHA256 Hash</returns>
        public static byte[] GetSHA384(string inputFile)
        {
            using (var sha384 = SHA384.Create())
            using (var stream = File.OpenRead(inputFile))
                return sha384.ComputeHash(stream);
        }

        /// <summary>
        /// Gets the SHA512 Hash of a selected file
        /// </summary>
        /// <param name="inputFile">File to get SHA512 Hash of</param>
        /// <returns>The Files SHA512 Hash</returns>
        public static byte[] GetSHA512(string inputFile)
        {
            using (var sha512 = SHA512.Create())
            using (var stream = File.OpenRead(inputFile))
                return sha512.ComputeHash(stream);
        }

        public enum ChecksumType
        {
            NULL,
            SHA1,
            SHA256,
            SHA384,
            SHA512
        }
    }
}
