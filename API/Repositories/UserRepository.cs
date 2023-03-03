
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyWebsite.Models;
using MyWebsite.Tools;

namespace MyWebsite.Repositories;

public class UserRepository : BaseRepository<User>
{
    public UserRepository(DataDbContext dataContext) : base(dataContext)
    {
    }

    public override List<User> FindAll()
    {
        return _dataContext.Users.Include(u => u.Role).ToList();
    }

    public override User FindById(int id)
    {
        return _dataContext.Users.Include(u => u.Role).FirstOrDefault(u => u.Id == id);
    }

    public User FindByLogin(string login)
    {
        return _dataContext.Users.Include(u => u.Role).FirstOrDefault(u => u.Username == login || u.Email == login);
    }

    public bool Delete(User element)
    {
        _dataContext.Users.Remove(element);
        return Update();
    }

    public override bool Save(User element)
    {
        _dataContext.Users.Add(element);
        return Update();
    }

    public override List<User> SearchAll(Func<User, bool> SearchMethod)
    {
        return _dataContext.Users.Include(u => u.Role).Where(SearchMethod).ToList();
    }

    public override User SearchOne(Func<User, bool> SearchMethod)
    {
        return _dataContext.Users.Include(u => u.Role).FirstOrDefault(SearchMethod);
    }
}

