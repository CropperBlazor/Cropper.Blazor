namespace Cropper.Blazor.Models
{
    public class ImageData
    {
        public decimal Left { get; set; }
        public decimal Top { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public decimal Rotate { get; set; }
        public decimal ScaleX { get; set; }
        public decimal ScaleY { get; set; }
        public decimal NaturalWidth { get; set; }
        public decimal NaturalHeight { get; set; }
        public decimal AspectRatio { get; set; }
    }
}
