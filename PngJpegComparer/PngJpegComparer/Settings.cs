using System;
using System.IO;

namespace PngJpegComparer
{
    /// <summary>
    /// Static classed with application settings and file helper methods.
    /// </summary>
    public static class Settings
    {
        /// <summary>
        /// Apsolute path of the Resources dirrectory of the application.
        /// </summary>
        private static readonly string projectBasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Resources\");

        #region RAISE
        /// <summary>
        /// Name of the Raise dataset file used.
        /// </summary>
        public const string RaiseFile = "RAISE_1k";
        #endregion

        #region LEGO brick images
        /// <summary>
        /// Absolute path of the folder containing the Lego brick images dataset.
        /// </summary>
        public static readonly string LegoDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), LegoFile);
        /// <summary>
        /// Name of the folder containing the Lego brick images dataset.
        /// </summary>
        public const string LegoFile = "Lego brick images";
        #endregion

        #region Methods
        /// <summary>
        /// Returns the absolute path of the resource file specified.
        /// </summary>
        /// <param name="resourceFile">Name of the resource file.</param>
        /// <returns>The absolute path of the resource file.</returns>
        private static string GetBaseFileName(string resourceFile) => projectBasePath + resourceFile;

        /// <summary>
        /// Returns the absolute path of the resource file with the file extension.
        /// </summary>
        /// <param name="resourceFile">Name of the resource file.</param>
        /// <returns>The absolute path of the resource file with the file extension.</returns>
        public static string GetResourceFile(string resourceFile) => GetBaseFileName(resourceFile) + ".csv";

        /// <summary>
        /// Returns the name of the file where filtered input data will be saved.
        /// </summary>
        /// <param name="resourceFile">Name of the resource file.</param>
        /// <returns>Name of the file where filtered intput data will be saved</returns>
        public static string GetFilteredInputDataFile(string resourceFile) => GetBaseFileName(resourceFile) + "_filtered.csv";

        /// <summary>
        /// Returns the absolute path of the results file based on the name of the resources file.
        /// </summary>
        /// <param name="resourceFile">Name of the resource file.</param>
        /// <returns>The absolute path of the results file based on the name of the resources file.</returns>
        public static string GetResultsFile(string resourceFile) => GetBaseFileName(resourceFile) + "_results.csv";

        /// <summary>
        /// Returns the absolute path of the dirrectory where files specified in the resource file will be saved.
        /// </summary>
        /// <param name="resourceFile">Name of the resource file.</param>
        /// <returns>The absolute path of the dirrectory where files specified in the resource file will be saved.</returns>
        public static string GetOutputDirrectory(string resourceFile)
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), resourceFile);
        }
        #endregion
    }
}
