using System.Collections.Generic;
using System.Text;

namespace PngJpegComparer
{
    /// <summary>
    /// Class that is used to store necessary data for an image.
    /// </summary>
    public class ImageData
    {
        /// <summary>
        /// Absolute path of the image file.
        /// </summary>
        public string File { get; set; }

        /// <summary>
        /// List of sizes of the image in different formats.
        /// </summary>
        public List<long> Sizes { get; set; } = new List<long>();
        
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder(File);
            foreach (long size in Sizes)
            {
                stringBuilder.Append(",");
                stringBuilder.Append(size.ToString());
            }
            return stringBuilder.ToString();
        }

        public override bool Equals(object obj) => obj is ImageData imageData && File == imageData.File;

        public override int GetHashCode() => -825322277 + EqualityComparer<string>.Default.GetHashCode(File);
    }
}
