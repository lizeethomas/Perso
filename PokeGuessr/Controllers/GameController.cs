using Microsoft.AspNetCore.Mvc;
using PokeGuessr.DTOs;
using PokeGuessr.Models;
using PokeGuessr.Services;

namespace PokeGuessr.Controllers
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

        [HttpGet("/pokemon/{dex}")]
        public IActionResult GetPokemon(int dex)
        {
            Pokemon pkmn = _gameService.GetPokemon(dex - 1);
            if (pkmn != null)
            {
                return Ok(pkmn);
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult Setup([FromBody] SetupGameDTO dto)
        {
            GameDTO game = new GameDTO();
            string[] images = _gameService.Setup(dto.Url, dto.Size);
            if (images != null)
            {
                game.Urls = images;
                return Ok(game);
            }
            return BadRequest();
            
        }
    }
}
