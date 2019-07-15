using System.Drawing.Imaging;

namespace PngJpegComparer
{
    /// <summary>
    /// Class containg extension methods for ImageType enum.
    /// </summary>
    public static class ImageTypeExtensions
    {
        /// <summary>
        /// Extension method that returns the file type extension used by Windows OS for the specified image format.
        /// </summary>
        /// <param name="imageType">Enum value specifying the image format.</param>
        /// <returns>File type extension used by the Windows OS for the specified image format.</returns>
        public static string GetExtension(this ImageType imageType)
        {
            switch (imageType)
            {
                case ImageType.TIFF:
                    return "tif";
                case ImageType.PNG:
                    return "png";
                case ImageType.JPEG:
                    return "jpg";
                default:
                    Logger.Log("Unknown image format: " + imageType);
                    return "Unknown image format";
            }
        }

        /// <summary>
        /// Extension method that returns the ImageFormat of the specified image format.
        /// </summary>
        /// <param name="imageType">Enum value specifying the image format.</param>
        /// <returns>The ImageFormat of the specified image format.</returns>
        public static ImageFormat GetImageFormat(this ImageType imageType)
        {
            switch (imageType)
            {
                case ImageType.TIFF:
                    return ImageFormat.Tiff;
                case ImageType.PNG:
                    return ImageFormat.Png;
                case ImageType.JPEG:
                    return ImageFormat.Jpeg;
                default:
                    Logger.Log("Unknown file type: " + imageType);
                    return ImageFormat.Tiff;
            }
        }
    }
}
