namespace PngJpegComparer
{
    public class ImageData
    {
        public string File { get; set; }
        public string DestinationTIFF { get; set; }
        
        public int SizeTIFF { get; set; }
        public int SizePNG { get; set; }
        public int SizeJPEG { get; set; }

        public ImageData(string file, string destinationTIFF)
        {
            File = file;
            DestinationTIFF = destinationTIFF;
        }
    }
}
