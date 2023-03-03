using Microsoft.AspNetCore.Mvc;
using MyWebsite.Models;
using MyWebsite.Services;

namespace MyWebsite.Controllers
{
    [Route("cv")]
    [ApiController]
    public class CVController : Controller
    {
        private CVService _cvService;
        public CVController(CVService cvService)
        {
            _cvService = cvService;
        }

        [HttpGet]
        public IActionResult DisplayAll()
        {
            List<CV> result = _cvService.GetCVs();
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest();
        }
    }
}
