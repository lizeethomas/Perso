using System.Drawing;
using System.Net;
using System.Runtime.Intrinsics.Arm;
using System.Xml.Linq;
using MyWebsite.Exceptions;
using static System.Net.Mime.MediaTypeNames;

namespace MyWebsite.Services
{
    public class GameService
    {
        public GameService() { }

        public Bitmap GetImage(string url)
        {
            string path = "C:\\Users\\tlizee\\CODE\\MyWebsite\\API\\Data\\image.jpg";

            var webRequest = WebRequest.Create(url);
            var webResponse = webRequest.GetResponse();
            var stream = webResponse.GetResponseStream();
            Bitmap bitmap = new Bitmap(stream);
           
            bitmap.Save(path);
            return bitmap;
        }

        public Bitmap Crop(int size)
        {
            string path = "C:\\Users\\tlizee\\CODE\\MyWebsite\\API\\Data\\image.jpg";
            string exit = "C:\\Users\\tlizee\\CODE\\MyWebsite\\API\\Data\\new_image.jpg";

            Bitmap oldImg = new Bitmap(path);
            Bitmap newImg = oldImg;

            var pos = GetRandomXY(size, path);

            try
            {
                if (pos == null)
                {
                    throw new PosNotFoundExcpetion();
                }
                int x = pos[0];
                int y = pos[1];

                Rectangle rectangle = new Rectangle(x - size / 2, y - size / 2, size, size);
                newImg = oldImg.Clone(rectangle, oldImg.PixelFormat);
                newImg.Save(exit);
                return newImg;
            }
            catch(PosNotFoundExcpetion)
            {
                return null;
            }
            finally
            {
                oldImg.Dispose();
                newImg.Dispose();
            }

        }

        public int[] GetRandomXY(int size, string path)
        {
            Bitmap img = new Bitmap(path);
            try
            {
                if (size > img.Width || size > img.Height)
                {
                    throw new OutOfTheBoxException();
                }
                Random rng = new Random();
                int x = rng.Next(size/2, img.Width - size / 2);
                int y = rng.Next(size/2, img.Height - size / 2);

                int[] result = { x, y };
                return result;

            }
            catch (OutOfTheBoxException)
            {
                Console.Error.WriteLine("Crop size greater than source image");
                return null;
            }
            finally
            {
                img.Dispose();
            }
        }

    }
}
