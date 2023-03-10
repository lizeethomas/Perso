namespace MyWebsite.DTOs
{
    public class ImgByteDTO
    {
        public Image<Rgba32> Image { get; set; }
        public byte[] Data { get; set; }
        public int Width { get; set; }
        public int Height { get; set;  }
        public int[,] Pixels { get; set; }
        public int[,] Alphas { get; set; }
    }
}
