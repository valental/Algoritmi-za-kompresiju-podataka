using System.Drawing.Imaging;

namespace PngJpegComparer
{
    public static class Extensions
    {
        public static string GetExtension(this FileType fileType)
        {
            switch (fileType)
            {
                case FileType.TIFF:
                    return "tif";
                case FileType.PNG:
                    return "png";
                case FileType.JPEG:
                    return "jpg";
                default:
                    Logger.Log("Unknown file type: " + fileType);
                    return "Unknown file type";
            }
        }

        public static ImageFormat GetImageFormat(this FileType fileType)
        {
            switch (fileType)
            {
                case FileType.TIFF:
                    return ImageFormat.Tiff;
                case FileType.PNG:
                    return ImageFormat.Png;
                case FileType.JPEG:
                    return ImageFormat.Jpeg;
                default:
                    Logger.Log("Unknown file type: " + fileType);
                    return ImageFormat.Tiff;
            }
        }
    }
}
