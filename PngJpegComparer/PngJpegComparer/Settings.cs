using System;
using System.IO;

namespace PngJpegComparer
{
    public static class Settings
    {
        private static readonly string projectBasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Resources\");

        #region RAISE
        public const string RaiseFile = "RAISE_1k";
        #endregion

        #region LEGO brick images
        public static readonly string LegoDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), LegoFile);
        public const string LegoFile = "Lego brick images";
        #endregion

        #region Methods
        private static string GetBaseFileName(string resourceFile) => projectBasePath + resourceFile;

        public static string GetResourceFile(string resourceFile) => GetBaseFileName(resourceFile) + ".csv";

        public static string GetFilteredInputDataFile(string resourceFile) => GetBaseFileName(resourceFile) + "_filtered.csv";

        public static string GetResultsFile(string resourceFile) => GetBaseFileName(resourceFile) + "_results.csv";

        public static string GetOutputDirrectory(string resourceFile)
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), resourceFile);
        }
        #endregion
    }
}
