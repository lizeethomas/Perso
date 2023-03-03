using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyWebsite.DTOs;
using MyWebsite.Models;
using MyWebsite.Services;
using System.Drawing;

namespace MyWebsite.Controllers
{
    [Route("users")]
    [ApiController]
    public class UserController : Controller
    {
        private UserService _userService;
        private JWTService _jwtService;
        public UserController(UserService userService, JWTService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        [HttpGet]
        public IActionResult DisplayAllUsers()
        {
            List<User> users = _userService.DisplayAll();
            if (users != null)
            {
                return Ok(users);
            }
            return BadRequest();
        }

        [HttpPost]
        public IActionResult NewUser([FromBody] UserDTO userRequest)
        {
            UserDTO userResponse = _userService.CreateUser(userRequest);
            if (userResponse != null)
            {
                return Ok(userResponse);
            }
            return BadRequest();
        }

        [HttpDelete]
        public IActionResult RemoveUser([FromForm] int id)
        {
            if (_userService.DeleteUser(id))
            {
                return Ok(true);
            }
            return BadRequest();

        }

        //[HttpGet("img")]
        //public IActionResult test()
        //{
        //    Img img = new Img();
        //    Bitmap result = img.getImage();
            
        //    return Ok(result);
        //}

        //[HttpPost("/login")]
        //public IActionResult LogIn([FromForm] string login, [FromForm] string password)
        //{
        //    string token = _jwtService.GetJWT(login, password);
        //    if (token != null)
        //    {
        //        return Ok(token);
        //    }
        //    return Ok(null);
        //}
    }
}
