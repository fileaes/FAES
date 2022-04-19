using System;
using System.Collections.Generic;
using System.Linq;

namespace FAES.Packaging
{
    /// <summary>
    /// Compression Modes
    /// </summary>
    public enum CompressionMode
    {
        ZIP,
        TAR,
        LZMA,
        GZIP,
        LGYZIP
    };

    /// <summary>
    /// Compression Levels
    /// </summary>
    public enum CompressionLevel
    {
        None,
        Fastest,
        Optimal,
        Slowest,
    }

    /// <summary>
    /// Predefined Optimise Compression Autoselects
    /// </summary>
    public enum Optimise
    {
        Balanced,
        Faster_Compression,
        Lower_Filesize,
        Store,
        Legacy
    };

    public class CompressionUtils
    {
        /// <summary>
        /// Gets a List of all Compression Modes
        /// </summary>
        /// <returns>Compression Modes</returns>
        public static List<CompressionMode> GetAllCompressionModes()
        {
            return Enum.GetValues(typeof(CompressionMode)).Cast<CompressionMode>().ToList();
        }

        /// <summary>
        /// Gets a List of all Compression Mode Names
        /// </summary>
        /// <returns>Compression Modes</returns>
        public static List<string> GetAllCompressionModesAsStrings()
        {
            return Enum.GetNames(typeof(CompressionMode)).ToList();
        }

        /// <summary>
        /// Gets a List of all Compression Levels
        /// </summary>
        /// <returns>Compression Levels</returns>
        public static List<CompressionLevel> GetAllCompressionLevels()
        {
            return Enum.GetValues(typeof(CompressionLevel)).Cast<CompressionLevel>().ToList();
        }

        /// <summary>
        /// Gets a List of all Compression Level Names
        /// </summary>
        /// <returns>Compression Levels</returns>
        public static List<string> GetAllCompressionLevelsAsStrings()
        {
            return Enum.GetNames(typeof(CompressionLevel)).ToList();
        }

        /// <summary>
        /// Gets a List of all Optimise Modes
        /// </summary>
        /// <returns>Optimise Modes</returns>
        public static List<Optimise> GetAllOptimiseModes()
        {
            return Enum.GetValues(typeof(Optimise)).Cast<Optimise>().ToList();
        }

        /// <summary>
        /// Gets a List of all Optimise Mode Names
        /// </summary>
        /// <returns>Optimise Modes</returns>
        public static List<string> GetAllOptimiseModesAsStrings()
        {
            return Enum.GetNames(typeof(Optimise)).ToList();
        }
    }
}
