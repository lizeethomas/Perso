using Azure.Messaging;
using Microsoft.AspNetCore.Mvc;
using MyWebsite.Services;
using MyWebsite.Models;
using MyWebsite.DTOs;

namespace MyWebsite.Controllers
{
    [Route("home")]
    [ApiController]
    public class HomeController : Controller
    {
        private JWTService _jwtService;
        private UserService _userService;

        public HomeController(JWTService jwtService, UserService userService)
        {
            _jwtService = jwtService;
            _userService = userService;
        }

        [HttpPost("/login")]
        public IActionResult Login([FromBody] LogInfo login)
        {
            User user = _userService.GetUserByLogin(login.Login);

            string token = _jwtService.GetJWT(login.Login, login.Password);
            if (token == null) { token = ""; }

            LoginDTO data = new LoginDTO()
            {
                Token = token,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role.RoleUser,
                Expire = DateTime.Now.AddHours(3),
            };

            return Ok(data);
        }
    }
}
