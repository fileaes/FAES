using System;
using System.IO;
using System.Security.Cryptography;

namespace CoreChecksums
{
    public class Checksums
    {
        public static string convertHashToString(byte[] hash)
        {
            return BitConverter.ToString(hash).Replace("-", "");
        }

        public static bool compareHash(byte[] firstHash, byte[] secondHash)
        {
            if (convertHashToString(firstHash) == convertHashToString(secondHash)) return true;
            else return false;
        }

        public static byte[] getMD5(string inputFile)
        {
            using (var md5 = MD5.Create())
            using (var stream = File.OpenRead(inputFile))
                return md5.ComputeHash(stream);
        }

        public static byte[] getSHA1(string inputFile)
        {
            using (var sha1 = SHA1.Create())
            using (var stream = File.OpenRead(inputFile))
                return sha1.ComputeHash(stream);
        }

        public static byte[] getSHA256(string inputFile)
        {
            using (var sha256 = SHA256.Create())
            using (var stream = File.OpenRead(inputFile))
                return sha256.ComputeHash(stream);
        }

        public static byte[] getSHA512(string inputFile)
        {
            using (var sha512 = SHA512.Create())
            using (var stream = File.OpenRead(inputFile))
                return sha512.ComputeHash(stream);
        }
    }
}
