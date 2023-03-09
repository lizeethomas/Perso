using System.Drawing;
using System.Net;
using MyWebsite.Enums;
using System.Runtime.Intrinsics.Arm;
using System.Xml.Linq;
using MyWebsite.Exceptions;
using static System.Net.Mime.MediaTypeNames;
using HtmlAgilityPack;
using System.Drawing.Imaging;
using Image = System.Drawing.Image;
using MyWebsite.DTOs;
using System.Runtime.InteropServices;
using static Azure.Core.HttpHeader;
using System.Threading.Tasks;

namespace MyWebsite.Services
{
    public class GameService
    {
        //private string filepath = "../Data/pokedex_names_types.txt";
        private string filepath = "C:\\Users\\tlizee\\CODE\\MyWebsite\\API\\Data\\pokedex_names_types.txt";


        public GameService() { }

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

        public Bitmap UrlToBitmap(string url)
        {
            WebRequest request = WebRequest.Create(url);
            using (WebResponse response = request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            {
                Bitmap bitmap = new Bitmap(stream);

                // Convertir l'image au format BMP
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    bitmap.Save(memoryStream, ImageFormat.Bmp);
                    return bitmap;
                    // Utiliser l'image BMP ici
                }
            }

        }

        public ImgByteDTO UrlToByte(string url)
        {
            ImgByteDTO imgByteDTO = new ImgByteDTO();
            using (var webClient = new WebClient())
            {
                byte[] imageData = webClient.DownloadData(url);
                imgByteDTO.Data = imageData;

                // Stocker l'image dans un tableau 2D de bytes
                using (var ms = new MemoryStream(imageData))
                {
                    var image = Image.FromStream(ms);
                    imgByteDTO.Width = image.Width;
                    imgByteDTO.Height = image.Height;

                    Bitmap bitmap = new Bitmap(image);
                    int[,] pixels = new int[bitmap.Width, bitmap.Height];
                    int[,] alphas = new int[bitmap.Width, bitmap.Height];

                    for (int x = 0; x < bitmap.Width; x++)
                    {
                        for (int y = 0; y < bitmap.Height; y++)
                        {
                            Color pixelColor = bitmap.GetPixel(x, y);
                            int r = pixelColor.R;
                            int g = pixelColor.G;
                            int b = pixelColor.B;
                            int rgb = (r << 16) + (g << 8) + b;
                            pixels[x, y] = rgb;
                            alphas[x ,y] = pixelColor.A;
                        }
                    }
                    imgByteDTO.Pixels = pixels;
                    imgByteDTO.Alphas = alphas;
                }
            }
            return imgByteDTO;
        }

        public ResponseGameDTO SetUpGame(string url, int size)
        {
            ResponseGameDTO result = new ResponseGameDTO();
            ImgByteDTO newImg = UrlToByte(url);
            var pos = GetRandomPos(size, newImg);
            string str = null;
            

            if (newImg != null)
            {
                Bitmap mask = new Bitmap(newImg.Width, newImg.Height);

                int[][] game = new int[newImg.Width][];
                for (int i = 0; i < newImg.Width; i++)
                {
                    game[i] = new int[newImg.Height];
                }

                int x = pos[0];
                int y = pos[1];
            
                for (int i = x-size/2; i < x+size/2; i++)
                {
                    for (int j = y-size/2; j < y+size/2; j++)
                    {
                        int rgb = newImg.Pixels[i, j];
                        game[i][j] = rgb;
                        int r = (rgb >> 16) & 0xff;
                        int g = (rgb >> 8) & 0xff;
                        int b = rgb & 0xff;
                        Color pixelColor = Color.FromArgb(r, g, b);
                        if (newImg.Alphas[i, j] != 0)
                        {
                            mask.SetPixel(i, j, pixelColor);
                        }
                    }
                }
                result.Url = BitmapToUrl(mask);
                result.Game = game;
            }
            return result;
        }

        public ResponseGameDTO SetUpNewHint(string url, int[][] game, int size)
        {
            ResponseGameDTO result = new ResponseGameDTO();
            ImgByteDTO soluce = UrlToByte(url);
            Bitmap bitmap = new Bitmap(soluce.Width, soluce.Height);

            for (int i = 0; i < soluce.Width; i++)
            {
                for (int j = 0; j < soluce.Height; j++)
                {
                    int rgb = game[i][j];
                    int r = (rgb >> 16) & 0xff;
                    int g = (rgb >> 8) & 0xff;
                    int b = rgb & 0xff;
                    Color pixelColor = Color.FromArgb(r, g, b);
                    if (game[i][j] > 0)
                    {
                        bitmap.SetPixel(i, j, pixelColor);
                    }
                    
                }
            }

            int x;
            int y;
            Range rx;
            Range ry;
            do {
                var pos = GetRandomPos(size, soluce);
                x = pos[0];
                y = pos[1];
            } while (game[x][y] != 0);

            for (int i = x - size / 2; i < x + size / 2; i++)
            {
                for (int j = y - size / 2; j < y + size / 2; j++)
                {
                    int rgb = soluce.Pixels[i, j];
                    game[i][j] = rgb;
                    int r = (rgb >> 16) & 0xff;
                    int g = (rgb >> 8) & 0xff;
                    int b = rgb & 0xff;
                    Color pixelColor = Color.FromArgb(r, g, b);
                    if (soluce.Alphas[i, j] > 0)
                    {
                        bitmap.SetPixel(i, j, pixelColor);
                    }
                }
            }

            result.Url = BitmapToUrl(bitmap);
            result.Game = game;
            return result;
        }

        public string SetUpShadow(string name)
        {
            string str = null;
            string url = GetImgUrl(name);
            ImgByteDTO newImg = UrlToByte(url);
            if (newImg != null)
            {
                Bitmap shadow = new Bitmap(newImg.Width, newImg.Height);
                Graphics gr = Graphics.FromImage(shadow);
                gr.Clear(Color.White);

                for (int i = 0; i < newImg.Width; i++)
                {
                    for (int j = 0; j < newImg.Height; j++)
                    {
                        if (newImg.Alphas[i, j] != 0)
                        {
                            shadow.SetPixel(i, j, Color.Black);
                        }
                    }
                }
                str = BitmapToUrl(shadow);
            }
            return str;
        }

        public string BitmapToUrl(Bitmap img)
        {
            Bitmap image = new Bitmap(img);
            MemoryStream stream = new MemoryStream();
            image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            byte[] bytes = stream.ToArray();
            string base64 = Convert.ToBase64String(bytes);
            string dataUrl = $"data:image/png;base64,{base64}";
            return dataUrl;
        }

        public int[] GetRandomPos(int size, ImgByteDTO dto)
        {
            try
            {
                int width = dto.Width;
                int height = dto.Height;

                if (size > width || size > height)
                {
                    throw new OutOfTheBoxException();
                }

                int nbPixelsEmpty = 0;
                int x;
                int y;
                double test;

                do
                {
                    Random rng = new Random();
                    x = rng.Next(size / 2, width - size / 2);
                    y = rng.Next(size / 2, height - size / 2);

                    for (int i = x - size / 2; i < x + size / 2; i++)
                    {
                        for (int j = y - size / 2; j < y + size / 2; j++)
                        {
                            if (dto.Alphas[i, j] != 0)
                            {
                                nbPixelsEmpty++;
                            }
                        }
                    }
                    test = 100 * (nbPixelsEmpty / Math.Pow(size, 2));
                } while (test < 60);

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


//public Bitmap Crop(int size)
//{
//    string path = this.imagepath;
//    string exit = this.newimagepath;

//    Bitmap oldImg = new Bitmap(path);
//    Bitmap newImg = oldImg;

//    var pos = GetRandomXY(size, path);

//    try
//    {
//        if (pos == null)
//        {
//            throw new PosNotFoundExcpetion();
//        }
//        int x = pos[0];
//        int y = pos[1];

//        Rectangle rectangle = new Rectangle(x - size / 2, y - size / 2, size, size);
//        newImg = oldImg.Clone(rectangle, oldImg.PixelFormat);
//        newImg.Save(exit);
//        return newImg;
//    }
//    catch (PosNotFoundExcpetion)
//    {
//        return null;
//    }
//    finally
//    {
//        oldImg.Dispose();
//        newImg.Dispose();
//    }

//}