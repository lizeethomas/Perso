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
                game.Shadow = images[0];
                game.Start = images[1];
                game.Hint1 = images[2];
                game.Hint2 = images[3];
                game.Hint3 = images[4];
                game.Hint4 = images[5];
                game.Hint5 = images[6];
                return Ok(game);
            }
            return BadRequest();
        }
    }
}
