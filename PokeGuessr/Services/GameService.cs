using PokeGuessr.Enums;
using PokeGuessr.Models;
using PokeGuessr.Exceptions;
using System.Net;
using System.Xml;
using HtmlAgilityPack;
using SixLabors.ImageSharp.Formats.Png;

namespace PokeGuessr.Services
{
    public class GameService
    {
        string filepath = Path.Combine("Data", "pokedex.txt");

        public Pokemon GetPokemon(int nb)
        {
            Pokemon pkmn = new Pokemon();
            string txt = File.ReadAllText(this.filepath);
            string[] lines = txt.Split('\n');
            string line = lines.ElementAt(nb);
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

                pkmn.Dex = nb + 1;
                pkmn.Name = name;
                foreach (var type in types)
                {
                    if (Enum.IsDefined(typeof(Types), type))
                    {
                        if(pkmn.Type1 == null) { pkmn.Type1 = type;}
                        else { pkmn.Type2 = type; }
                    }
                }
                pkmn.Url = GetImgURL(name);
            }
            return pkmn;
        }

        public string[] Setup(string url, int size)
        {
            Image<Rgba32> soluce = ConvertURLtoImg(url);
            Image<Rgba32> shadow = new Image<Rgba32>(soluce.Width, soluce.Height);
            string[] images = new string[7];
            int[][] matrix = new int[soluce.Width][];
            for (int i = 0; i < soluce.Width; i++)
            {
                matrix[i] = new int[soluce.Height];
                for (int j = 0; j < soluce.Height; j++)
                {
                    if (soluce[i, j].A != 0) { shadow[i, j] = Color.Black; }
                }
            }
            images[0] = ConvertImgToUrl(shadow);
            Image<Rgba32> image = new Image<Rgba32>(soluce.Width, soluce.Height);
            for (int k = 1; k < 7; k++)
            {
                int x; int y;
                do {var pos = GetRandomPos(soluce, size); x = pos[0]; y = pos[1]; } 
                while (matrix[x][y] != 0);

                for (int i = x - size / 2; i < x + size / 2; i++)
                {
                    for (int j = y - size / 2; j < y + size / 2; j++)
                    {
                        image[i, j] = soluce[i, j];
                        matrix[i][j] = 1;
                    }
                }
                images[k] = ConvertImgToUrl(image);
            }
            return images;
        }

        public int[] GetRandomPos(Image<Rgba32> image, int size)
        {
            try
            {
                int width = image.Width;
                int height = image.Height;
                if (size > width || size > height) {throw new OutOfTheBoxException();}
                int nbPix = 0; int x; int y; double test;
                do
                {
                    Random rng = new Random();
                    x = rng.Next(size / 2, width - size / 2);
                    y = rng.Next(size / 2, height - size / 2);
                    for (int i = x - size / 2; i < x + size / 2; i++)
                    {
                        for (int j = y - size / 2; j < y + size / 2; j++)
                        {
                            if (image[i, j].A != 0)
                            {
                                nbPix++;
                            }
                        }
                    }
                    test = 100 * (nbPix / Math.Pow(size, 2));
                } while (test < 60);
                int[] result = { x, y };
                return result;

            }
            catch(OutOfTheBoxException)
            {
                Console.Error.WriteLine("Crop size greater than source image");
                return null;
            }
        }

        public string GetImgURL(string name)
        {
            string baseurl = "https://www.pokepedia.fr/Fichier:";
            string url = baseurl + name + ".png";
            string imageUrl = "https://www.pokepedia.fr";
            WebClient client = new WebClient();
            string html = client.DownloadString(url);
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNode imageNode = doc.DocumentNode.Descendants("img").FirstOrDefault(x => x.GetAttributeValue("alt", "").Contains(name));
            imageUrl += imageNode?.GetAttributeValue("src", "");
            return imageUrl;
        }

        public Image<Rgba32> ConvertURLtoImg(string url)
        {
            Image<Rgba32> img = null;
            try
            {
                WebClient client = new WebClient();
                byte[] data = client.DownloadData(url);
                MemoryStream ms = new MemoryStream(data);
                img = Image.Load<Rgba32>(ms);
            }
            catch(Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }
            return img;
        }

        public string ConvertImgToUrl(Image<Rgba32> img)
        {
            Image<Rgba32> image = img;
            MemoryStream stream = new MemoryStream();
            image.Save(stream, new PngEncoder());
            byte[] bytes = stream.ToArray();
            string base64 = Convert.ToBase64String(bytes);
            string dataUrl = $"data:image/png;base64,{base64}";
            return dataUrl;
        }
    }
}
