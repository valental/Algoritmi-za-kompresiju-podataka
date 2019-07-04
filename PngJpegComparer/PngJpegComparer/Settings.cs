using System;
using System.IO;

namespace PngJpegComparer
{
    public static class Settings
    {
        private static readonly string projectBasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Resources\");
        private const string resourceFile = "RAISE_1k";
        private static readonly string baseFileName = projectBasePath + resourceFile;
        public static string ResourceFile => baseFileName + ".csv";
        public static string FilteredInputDataFile => baseFileName + "_filtered.csv";
        public static string ResultsFile => baseFileName + "_results.csv";
        public static string OutputDirrectory => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), resourceFile);
        public const string ImageDownloadFormat = ".tif";

        public const string ImageQuality = "Lossless Compressed RAW (14-bit)";
    }
}
