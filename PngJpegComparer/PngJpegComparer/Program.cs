using System.Collections.Generic;
using System.IO;

namespace PngJpegComparer
{
    public class Program
    {
        protected Program()
        {

        }

        static void Main(string[] args)
        {
            // RAISE
            RaisePicturesDownloader.Download(Settings.RaiseFile);
            FormatConverter.CompareFormats(
                Settings.GetOutputDirrectory(Settings.RaiseFile), Settings.GetResultsFile(Settings.RaiseFile),
                FileType.TIFF, new List<FileType> { FileType.PNG, FileType.JPEG },
                SearchOption.TopDirectoryOnly
                );

            // LEGO brick images
            FormatConverter.CompareFormats(
                Settings.LegoDirectory, Settings.GetResultsFile(Settings.LegoFile),
                FileType.PNG, new List<FileType> { FileType.JPEG },
                SearchOption.AllDirectories
                );

            // Save 10 examples of RAISE images in png and in jpeg
            FormatConverter.SaveExamples(
                Settings.GetOutputDirrectory(Settings.RaiseFile),
                Settings.GetOutputDirrectory(Settings.RaiseFile) + "_examples",
                10,
                FileType.TIFF, new List<FileType>() { FileType.PNG, FileType.JPEG },
                SearchOption.TopDirectoryOnly
                );

            // Save 10 examples of Lego images in jpeg
            FormatConverter.SaveExamples(
                Settings.GetOutputDirrectory(Settings.LegoDirectory),
                Settings.GetOutputDirrectory(Settings.LegoDirectory) + "_examples",
                10,
                FileType.PNG, new List<FileType>() { FileType.JPEG },
                SearchOption.AllDirectories
                );
        }
    }
}
