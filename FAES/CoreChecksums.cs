using System;
using System.IO;
using System.Security.Cryptography;

namespace CoreChecksums
{
    internal class Checksums
    {
        /// <summary>
        /// Converts Hash Byte Array to Hash String
        /// </summary>
        /// <param name="hash">Hash Byte Array</param>
        /// <returns>Hash String</returns>
        public static string convertHashToString(byte[] hash)
        {
            return BitConverter.ToString(hash).Replace("-", "");
        }

        /// <summary>
        /// Compares two hashes
        /// </summary>
        /// <param name="firstHash">First Hash to compare</param>
        /// <param name="secondHash">Second Hash to compare</param>
        /// <returns>If the hashes match</returns>
        public static bool compareHash(byte[] firstHash, byte[] secondHash)
        {
            if (convertHashToString(firstHash) == convertHashToString(secondHash)) return true;
            else return false;
        }

        /// <summary>
        /// Gets the SHA1 Hash of a selected file
        /// </summary>
        /// <param name="inputFile">File to get SHA1 Hash of</param>
        /// <returns>The Files SHA1 Hash</returns>
        public static byte[] getSHA1(string inputFile)
        {
            using (var sha1 = SHA1.Create())
            using (var stream = File.OpenRead(inputFile))
                return sha1.ComputeHash(stream);
        }
    }
}
