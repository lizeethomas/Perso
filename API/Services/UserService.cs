using MyWebsite.DTOs;
using MyWebsite.Models;
using MyWebsite.Repositories;

namespace MyWebsite.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepo;
        private readonly RoleRepository _roleRepo;

        public UserService(UserRepository userRepo, RoleRepository roleRepo)
        {
            _userRepo = userRepo;
            _roleRepo = roleRepo;   
        }

        public List<User> DisplayAll()
        {
            List<User> users = _userRepo.FindAll();
            return users;
        }

        public UserDTO CreateUser(UserDTO userRequest)
        {
            UserDTO userResponse = new UserDTO();
            User user = new User()
            {
                Email = userRequest.Email,
                Password = userRequest.Password,
                Username = userRequest.Username,
            };
            Role role = _roleRepo.FindById(1);
            user.Role = role;

            if (_userRepo.Save(user))
            {
                userResponse.Email = userRequest.Email;
                userResponse.Password = userRequest.Password;
                userResponse.Username = userRequest.Username;

                return userResponse;
            }
            return null;
        }

        public User GetUserByLogin(string login)
        {
            if (_userRepo.FindByLogin(login) != null)
            {
                return _userRepo.FindByLogin(login);
            }
            return null;
        }

        public bool DeleteUser(int id) 
        { 
            User user = _userRepo.FindById(id);
            if (_userRepo.Delete(user))
            {
                return true;
            }
            return false;
        }
    }
}
