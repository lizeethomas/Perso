using Microsoft.IdentityModel.Tokens;
using MyWebsite.Models;
using MyWebsite.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyWebsite.Services
{
    public class JWTService
    {
        private UserRepository _userRepository;

        public JWTService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public string GetJWT(string log, string password)
        {
            User user = _userRepository.SearchOne(u => (u.Email == log || u.Username == log) && u.Password == password);
            if (user != null)
            {
                //Créer le token 
                JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor()
                {
                    Expires = DateTime.Now.AddHours(3),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes("So kouzin being a clef de securite is my way of life")), SecurityAlgorithms.HmacSha256),
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Role, user.Role.RoleUser),
                        new Claim(ClaimTypes.Name, user.Username)
                    }),
                    Issuer = "MyWebsite",
                    Audience = "MyWebsite"

                };
                SecurityToken securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);

                return jwtSecurityTokenHandler.WriteToken(securityToken);
            }
            return null;
        }
    }
}
