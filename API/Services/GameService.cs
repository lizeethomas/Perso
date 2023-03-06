using System.Drawing;
using System.Net;
using MyWebsite.Enums;
using System.Runtime.Intrinsics.Arm;
using System.Xml.Linq;
using MyWebsite.Exceptions;
using static System.Net.Mime.MediaTypeNames;
using HtmlAgilityPack;
using System.Drawing.Imaging;

namespace MyWebsite.Services
{
    public class GameService
    {
        //private string filepath = "E:\\Bureau\\MyWebsite\\MyWebsite\\API\\Data\\pokedex_names_types.txt";
        //private string imagepath = "E:\\Bureau\\MyWebsite\\MyWebsite\\API\\Data\\image.jpg";
        //private string newimagepath = "E:\\Bureau\\MyWebsite\\MyWebsite\\API\\Data\\new_image.jpg";

        private string filepath = "C:\\Users\\tlizee\\CODE\\MyWebsite\\API\\Data\\pokedex_names_types.txt";
        private string imagepath = "C:\\Users\\tlizee\\CODE\\MyWebsite\\API\\Data\\image.jpg";
        private string newimagepath = "C:\\Users\\tlizee\\CODE\\MyWebsite\\API\\Data\\new_image.jpg";


        public GameService() { }

        public Bitmap GetImage(string url)
        {
            string path = this.imagepath;

            var webRequest = WebRequest.Create(url);
            var webResponse = webRequest.GetResponse();
            var stream = webResponse.GetResponseStream();
            Bitmap bitmap = new Bitmap(stream);
           
            bitmap.Save(path);
            return bitmap;
        }

        public Bitmap GetImgFromURL(string url)
        {
            Bitmap bitmap = null;
            try
            {
                using (WebClient client = new WebClient())
                {
                    byte[] data = client.DownloadData(url);
                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream(data))
                    {
                        bitmap = new Bitmap(ms);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception here
            }
            return bitmap;
        }

        public Bitmap Crop(int size)
        {
            string path = this.imagepath;
            string exit = this.newimagepath;

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


        public string CropFromURL(int size, string url)
        {

            Bitmap oldImg = GetImgFromURL(url);
            Bitmap newImg = oldImg;

            var pos = GetRandomPos(size, oldImg);

            try
            {
                if (pos == null)
                {
                    throw new PosNotFoundExcpetion();
                }
                int x = pos[0];
                int y = pos[1];

                Rectangle rectangle = new Rectangle(x - size / 2, y - size / 2, size, size);
                PixelFormat pixelFormat = oldImg.PixelFormat;
                newImg = oldImg.Clone(rectangle, PixelFormat.Format32bppArgb);

                MemoryStream stream = new MemoryStream();
                newImg.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                byte[] bytes = stream.ToArray();
                string base64 = Convert.ToBase64String(bytes);
                string dataUrl = $"data:image/png;base64,{base64}";
                return dataUrl;
            }
            catch (PosNotFoundExcpetion)
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

        public int[] GetRandomPos(int size, Bitmap bitmap)
        {
            try
            {
                if (size > bitmap.Width || size > bitmap.Height)
                {
                    throw new OutOfTheBoxException();
                }
                Random rng = new Random();
                int x = rng.Next(size / 2, bitmap.Width - size / 2);
                int y = rng.Next(size / 2, bitmap.Height - size / 2);

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
                bitmap.Dispose();
            }
        }

        public string GetImgUrl(string name)
        {
            string baseurl = "https://www.pokepedia.fr/Fichier:";
            string url = baseurl + name + ".png";
            string imageUrl = "https://www.pokepedia.fr";
            // Instancier un objet WebClient pour télécharger le contenu de la page
            using (var client = new WebClient())
            {
                string html = client.DownloadString(url);

                // Instancier un objet HtmlDocument pour analyser le code HTML de la page
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);

                // Trouver l'élément img ayant l'attribut alt recherché
                HtmlNode imageNode = doc.DocumentNode.Descendants("img").FirstOrDefault(x => x.GetAttributeValue("alt", "").Contains(name));

                // Récupérer l'URL de l'image à partir de l'attribut src de l'élément img
                imageUrl += imageNode?.GetAttributeValue("src", "");

                // Télécharger l'image à partir de l'URL
                //if (!string.IsNullOrEmpty(imageUrl))
                //{
                //    client.DownloadFile(imageUrl, "nom de fichier.png");
                //}
            }

            return imageUrl;
        }

        public Bitmap GetImgBitmap(string name)
        {
            Bitmap bitmap = null;
            string baseurl = "https://www.pokepedia.fr/Fichier:";
            string url = baseurl + name + ".png";
            string imageUrl = "https://www.pokepedia.fr";
            // Instancier un objet WebClient pour télécharger le contenu de la page
            using (var client = new WebClient())
            {
                string html = client.DownloadString(url);

                // Instancier un objet HtmlDocument pour analyser le code HTML de la page
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);

                // Trouver l'élément img ayant l'attribut alt recherché
                HtmlNode imageNode = doc.DocumentNode.Descendants("img").FirstOrDefault(x => x.GetAttributeValue("alt", "").Contains(name));

                // Récupérer l'URL de l'image à partir de l'attribut src de l'élément img
                imageUrl += imageNode?.GetAttributeValue("src", "");

                // Télécharger l'image à partir de l'URL
                //if (!string.IsNullOrEmpty(imageUrl))
                //{
                //    client.DownloadFile(imageUrl, "nom de fichier.png");
                //}
                bitmap = GetImgFromURL(imageUrl);

            }

            return bitmap;
        }

        public List<String> GetPkmnInfo(int dex)
        {
            List<string> result = new List<string>();
            string txt = File.ReadAllText(this.filepath);
            string[] lines = txt.Split('\n');
            string line = lines.ElementAt(dex);
            if (line != null)
            {
                string[] tmp = line.Split("=");
                var types = tmp[1].Substring(0, tmp[1].Length - 2)
                    .Replace("{", String.Empty)
                    .Replace("}", String.Empty)
                    .Replace("\r", String.Empty)
                    .Replace("\"", String.Empty)
                    .Split(",");
                var name = tmp[0].Substring(1, tmp[0].Length - 2);
                
                result.Add((dex+1).ToString());
                result.Add(name);
                foreach (var type in types) {
                    if (Enum.IsDefined(typeof(Types), type))
                    {
                        result.Add(type);
                    }
                }
                return result;
            }
            return result;
        }

    }
}
