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

        [HttpPost("setup")]
        public IActionResult Setup([FromBody] RequestGameDTO dto)
        {
            ResponseGameDTO responseGameDTO = _gameService.SetUpGame(dto.Url, dto.Size);
            if (responseGameDTO != null)
            {
                return Ok(responseGameDTO);
            }
            return NotFound();
        }

        [HttpPost("hint")]
        public IActionResult Hint([FromBody] NewRequestGameDTO dto)
        {
            ResponseGameDTO responseGameDTO = new ResponseGameDTO();
            responseGameDTO = _gameService.SetUpNewHint(dto.Url, dto.Game, dto.Size);
            if (responseGameDTO != null)
            {
                return Ok(responseGameDTO);
            }
            return NotFound();
        }

        [HttpGet("/shadow/{name}")]
        public IActionResult Shadow(string name)
        {
            ResponseGameDTO responseGameDTO = new ResponseGameDTO();
            var str = _gameService.SetUpShadow(name);
            if (str != null)
            {
                responseGameDTO.Url = str;
                return Ok(responseGameDTO);
            }
            return NotFound();
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
            Image<Rgba32> bitmap = _gameService.GetImgBitmap(name);
            return Ok(bitmap);
        }

    }
}
