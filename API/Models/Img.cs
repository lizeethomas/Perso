using System.Drawing;

namespace MyWebsite.Models
{
    public class Img
    {
        public Bitmap getImage()
        {
            string path = "C:\\Users\\tlizee\\CODE\\MyWebsite\\API\\Data\\profondo_rosso.jpg";
            Bitmap img = new Bitmap(path);

            int height = img.Height;
            int width = img.Width;

            int xsize = width/2;
            int ysize = height/2;

            Bitmap newimg = new Bitmap(xsize, ysize);
            using (Graphics g = Graphics.FromImage(newimg))
            {
                g.DrawImage(img, new Rectangle(0,0, xsize, ysize), new Rectangle(0, 0, xsize, ysize), GraphicsUnit.Pixel);
            }

            img.Dispose();
            newimg.Dispose();

            return newimg;
        }
        

    }
}