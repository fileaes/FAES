namespace FAES.Utilities
{
    internal class TempPath
    {
        protected FAES_File _faesFile;
        protected string _tempPath;

        /// <summary>
        /// Creates a TempPath
        /// </summary>
        /// <param name="faesFile">FAES File to link to the TempPath</param>
        /// <param name="tempPath">Actual path to link to the TempPath</param>
        internal TempPath(FAES_File faesFile, string tempPath)
        {
            _faesFile = faesFile;
            _tempPath = tempPath;
        }

        /// <summary>
        /// Gets the FAES File linked to the TempPath
        /// </summary>
        /// <returns>Linked FAES File</returns>
        internal FAES_File GetFaesFile()
        {
            return _faesFile;
        }

        /// <summary>
        /// Gets the actual path linked to the TempPath
        /// </summary>
        /// <returns>Temp path</returns>
        internal string GetTempPath()
        {
            return _tempPath;
        }
    }
}
