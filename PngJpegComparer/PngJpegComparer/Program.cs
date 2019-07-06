using System.Collections.Generic;
using System.IO;

namespace PngJpegComparer
{
    public class Program
    {
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
        }
    }
}
