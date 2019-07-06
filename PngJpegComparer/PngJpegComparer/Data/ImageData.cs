using System.Collections.Generic;
using System.Text;

namespace PngJpegComparer
{
    public class ImageData
    {
        public string File { get; set; }
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

        public override bool Equals(object obj)
        {
            return obj is ImageData imageData && File == imageData.File;
        }

        public override int GetHashCode()
        {
            return -825322277 + EqualityComparer<string>.Default.GetHashCode(File);
        }
    }
}
