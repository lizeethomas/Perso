using Microsoft.AspNetCore.Mvc;
using MyWebsite.DTOs;
using MyWebsite.Enums;
using MyWebsite.Services;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;

namespace MyWebsite.Controllers
{
    [Route("game")]
    [ApiController]
    public class GameController : Controller
    {
        private GameService _gameService;
        public GameController(GameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet]
        public IActionResult SaveImg(string url)
        {
            Bitmap bitmap = _gameService.GetImage(url);
            if (bitmap != null)
            {
                return Ok(bitmap);
            }
            return BadRequest();
        }

        [HttpGet("{size:int}")]
        public IActionResult CropImg(int size)
        {
            Bitmap bitmap = _gameService.Crop(size);
            if (bitmap != null)
            {
                return Ok(bitmap);
            }
            return BadRequest();
        }

        [HttpPost]
        public IActionResult GetCropImg([FromBody] RequestGameDTO dto)
        {
            string str = dto.Url;
            int nb = dto.Size;
            string newurl = _gameService.CropFromURL(nb, str);
            if (newurl != null)
            {
                return Ok(newurl);
            }
            return BadRequest();
        }

        [HttpGet("/pokemon/all")]
        public IActionResult GetAll()
        {
            List<PokemonDTO> tmp = new List<PokemonDTO>();
            for (int i = 1; i <= 1010; i++)
            {
                var data = _gameService.GetPkmnInfo(i - 1);
                PokemonDTO pokemonDTO = new PokemonDTO()
                {
                    Dex = int.Parse(data[0]),
                    Name = data[1],
                    Type1 = data[2],
                    Type2 = data[data.Count - 1],
                };
                tmp.Add(pokemonDTO);
            }

            return Ok(tmp);
        }

        [HttpGet("/pokemon/{dex}")]
        public IActionResult GetOne(int dex)
        {
            var data = _gameService.GetPkmnInfo(dex - 1);
            PokemonDTO pokemonDTO = new PokemonDTO()
            {
                Dex = int.Parse(data[0]),
                Name = data[1],
                Type1 = data[2],
                Type2 = data[data.Count - 1],
            };

            return Ok(pokemonDTO);
        }

        [HttpGet("/image/{name}")]
        public IActionResult GetUrl(string name)
        {
            string url = _gameService.GetImgUrl(name);
            UrlDTO result = new UrlDTO()
            {
                Url = url,
            };
            return Ok(result);
        }

        [HttpGet("/bitmap/{name}")]
        public IActionResult GetBitmap(string name)
        {
            Bitmap bitmap = _gameService.GetImgBitmap(name);
            return Ok(bitmap);
        }

    }
}
