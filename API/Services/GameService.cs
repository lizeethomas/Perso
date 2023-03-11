
using System.Net;
using MyWebsite.Enums;
using System.Runtime.Intrinsics.Arm;
using System.Xml.Linq;
using MyWebsite.Exceptions;
using static System.Net.Mime.MediaTypeNames;
using HtmlAgilityPack;
using MyWebsite.DTOs;
using System.Runtime.InteropServices;
using static Azure.Core.HttpHeader;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Image = SixLabors.ImageSharp.Image;
using SixLabors.ImageSharp.Formats.Png;

namespace MyWebsite.Services
{
    public class GameService
    {
        private string filepath = "E:\\Bureau\\MyWebsite\\MyWebsite\\API\\Data\\pokedex_names_types.txt";
        //private string filepath = "C:\\Users\\tlizee\\CODE\\MyWebsite\\API\\Data\\pokedex_names_types.txt";


        public GameService() { }

        public Image<Rgba32> GetImgFromURL(string url)
        {
            Image<Rgba32> img = null;
            try
            {
                using (WebClient client = new WebClient())
                {
                    byte[] data = client.DownloadData(url);
                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream(data))
                    {
                        img = Image.Load<Rgba32>(ms);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception here
            }
            return img;
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
                    var image = Image.Load<Rgba32>(ms);
                    imgByteDTO.Width = image.Width;
                    imgByteDTO.Height = image.Height;

                    Image<Rgba32> bitmap = image;
                    imgByteDTO.Image = image;

                    int[,] pixels = new int[bitmap.Width, bitmap.Height];
                    int[,] alphas = new int[bitmap.Width, bitmap.Height];

                    for (int x = 0; x < bitmap.Width; x++)
                    {
                        for (int y = 0; y < bitmap.Height; y++)
                        {
                            Rgba32 pixelColor = image[x,y];
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
            Image<Rgba32> soluce = newImg.Image;

            if (newImg != null)
            {
                Image<Rgba32> mask = new Image<Rgba32>(newImg.Width, newImg.Height);

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
                        if (newImg.Alphas[i, j] != 0)
                        {
                            mask[i, j] = soluce[i, j];
                            game[i][j] = 1;
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
            Image<Rgba32> bitmap = new Image<Rgba32>(soluce.Width, soluce.Height);

            for (int i = 0; i < soluce.Width; i++)
            {
                for (int j = 0; j < soluce.Height; j++)
                {
                    if (game[i][j] != 0)
                    {
                        bitmap[i, j] = soluce.Image[i, j];
                    }       
                }
            }

            int x;
            int y;
            do {
                var pos = GetRandomPos(size, soluce);
                x = pos[0];
                y = pos[1];
            } while (game[x][y] != 0);

            for (int i = x - size / 2; i < x + size / 2; i++)
            {
                for (int j = y - size / 2; j < y + size / 2; j++)
                {
                    bitmap[i, j] = soluce.Image[i, j];
                    game[i][j] = 1;
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
                Image<Rgba32> shadow = new Image<Rgba32>(newImg.Width, newImg.Height);

                for (int i = 0; i < newImg.Width; i++)
                {
                    for (int j = 0; j < newImg.Height; j++)
                    {
                        if (newImg.Alphas[i, j] != 0)
                        {
                            shadow[i,j] = Color.Black;
                        }
                    }
                }
                str = BitmapToUrl(shadow);
            }
            return str;
        }

        public string BitmapToUrl(Image<Rgba32> img)
        {
            Image<Rgba32> image = img;
            MemoryStream stream = new MemoryStream();
            image.Save(stream, new PngEncoder());
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

        public Image<Rgba32> GetImgBitmap(string name)
        {
            Image<Rgba32> bitmap = null;
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