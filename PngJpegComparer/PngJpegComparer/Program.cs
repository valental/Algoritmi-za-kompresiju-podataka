using System.Collections.Generic;
using System.IO;

namespace PngJpegComparer
{
    public class Program
    {
        protected Program()
        {

        }

        /// <summary>
        /// Main method of the console application.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        static void Main(string[] args)
        {
            // RAISE
            RaisePicturesDownloader.Download(Settings.RaiseFile);
            FormatConverter.CompareFormats(
                Settings.GetOutputDirrectory(Settings.RaiseFile), Settings.GetResultsFile(Settings.RaiseFile),
                ImageType.TIFF, new List<ImageType> { ImageType.PNG, ImageType.JPEG },
                SearchOption.TopDirectoryOnly
                );

            // LEGO brick images
            FormatConverter.CompareFormats(
                Settings.LegoDirectory, Settings.GetResultsFile(Settings.LegoFile),
                ImageType.PNG, new List<ImageType> { ImageType.JPEG },
                SearchOption.AllDirectories
                );

            // Save 10 examples of RAISE images in png and in jpeg
            FormatConverter.SaveExamples(
                Settings.GetOutputDirrectory(Settings.RaiseFile),
                Settings.GetOutputDirrectory(Settings.RaiseFile) + "_examples",
                10,
                ImageType.TIFF, new List<ImageType> { ImageType.PNG, ImageType.JPEG },
                SearchOption.TopDirectoryOnly
                );

            // Save 10 examples of Lego images in jpeg
            FormatConverter.SaveExamples(
                Settings.GetOutputDirrectory(Settings.LegoDirectory),
                Settings.GetOutputDirrectory(Settings.LegoDirectory) + "_examples",
                10,
                ImageType.PNG, new List<ImageType> { ImageType.JPEG },
                SearchOption.AllDirectories
                );
        }
    }
}
