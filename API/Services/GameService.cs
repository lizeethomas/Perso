using System.Drawing;
using System.Net;
using MyWebsite.Enums;
using System.Runtime.Intrinsics.Arm;
using System.Xml.Linq;
using MyWebsite.Exceptions;
using static System.Net.Mime.MediaTypeNames;
using HtmlAgilityPack;

namespace MyWebsite.Services
{
    public class GameService
    {
        public GameService() { }

        public Bitmap GetImage(string url)
        {
            string path = "E:\\Bureau\\MyWebsite\\MyWebsite\\API\\Data\\image.jpg";//"C:\\Users\\tlizee\\CODE\\MyWebsite\\API\\Data\\image.jpg";

            var webRequest = WebRequest.Create(url);
            var webResponse = webRequest.GetResponse();
            var stream = webResponse.GetResponseStream();
            Bitmap bitmap = new Bitmap(stream);
           
            bitmap.Save(path);
            return bitmap;
        }

        public Bitmap Crop(int size)
        {
            string path = "E:\\Bureau\\MyWebsite\\MyWebsite\\API\\Data\\image.jpg";
            string exit = "E:\\Bureau\\MyWebsite\\MyWebsite\\API\\Data\\new_image.jpg";

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

        public List<String> GetPkmnInfo(int dex)
        {
            List<string> result = new List<string>();
            string txt = File.ReadAllText("E:\\Bureau\\MyWebsite\\MyWebsite\\API\\Data\\pokedex_names_types.txt");
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
