using Microsoft.AspNetCore.Mvc;
using MyWebsite.Services;
using System.Drawing;

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

        [HttpGet("{size}")]
        public IActionResult CropImg(int size)
        {
            Bitmap bitmap = _gameService.Crop(size);
            if (bitmap != null)
            {
                return Ok(bitmap);
            }
            return BadRequest();
        }
    }
}
