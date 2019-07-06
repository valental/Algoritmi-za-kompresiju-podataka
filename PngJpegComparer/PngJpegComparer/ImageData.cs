using System.Collections.Generic;

namespace PngJpegComparer
{
    public class ImageData
    {
        public string File { get; set; }
        
        public long SizeTiff { get; set; }
        public long SizePng { get; set; }
        public long SizeJpeg { get; set; }

        public string ToStringWithoutTiff()
        {
            return File + "," + SizePng + "," + SizeJpeg;
        }

        public override string ToString()
        {
            return File + "," + SizeTiff + "," + SizePng + "," + SizeJpeg;
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
