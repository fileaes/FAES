namespace FAES.Packaging
{
    internal interface ICompressedFAES
    {
        /// <summary>
        /// Compress an unencrypted FAES File.
        /// </summary>
        /// <param name="unencryptedFile">Unencrypted FAES File</param>
        /// <returns>Path of the unencrypted, compressed file</returns>
        string CompressFAESFile(FAES_File unencryptedFile);

        /// <summary>
        /// Uncompress an encrypted FAES File.
        /// </summary>
        /// <param name="encryptedFile">Encrypted FAES File</param>
        /// <returns>Path of the encrypted, uncompressed file</returns>
        string UncompressFAESFile(FAES_File encryptedFile);
    }
}
