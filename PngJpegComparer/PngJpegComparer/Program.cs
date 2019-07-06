namespace PngJpegComparer
{
    public class Program
    {
        static void Main(string[] args)
        {
            // RAISE
            RaisePicturesDownloader.Download(Settings.RaiseFile);
            FormatConverter.CompareFormatsTiffToPngAndJpeg(Settings.GetOutputDirrectory(Settings.RaiseFile), Settings.GetResultsFile(resultsFile));

            // LEGO brick images
            FormatConverter.CompareFormatsPngToJpegRecursively(Settings.LegoDirectory, Settings.GetResultsFile(Settings.LegoFile));
        }
    }
}
