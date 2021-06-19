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
        /// Decompress an encrypted FAES File.
        /// </summary>
        /// <param name="encryptedFile">Encrypted FAES File</param>
        /// <param name="overridePath">Override the read path</param>
        /// <returns>Path of the encrypted, decompressed file</returns>
        string DecompressFAESFile(FAES_File encryptedFile, string overridePath = "");
    }
}
