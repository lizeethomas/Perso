using Microsoft.EntityFrameworkCore;
using MyWebsite.Enums;
using MyWebsite.Models;
using MyWebsite.Tools;
using System.Linq;

namespace MyWebsite.Repositories
{
    public class GenreRepository : BaseRepository<Genre>
    {
        public GenreRepository(DataDbContext dataContext) : base(dataContext)
        {
        }

        public override List<Genre> FindAll()
        {
            return _dataContext.Genres.Include(m => m.Movies).ToList();
        }

        public override Genre FindById(int id)
        {
            return _dataContext.Genres.Include(m => m.Movies).FirstOrDefault(g => g.Id == id);
        }

        public override bool Save(Genre element)
        {
            throw new NotImplementedException();
        }

        public override List<Genre> SearchAll(Func<Genre, bool> SearchMethod)
        {
            throw new NotImplementedException();
        }

        public override Genre SearchOne(Func<Genre, bool> SearchMethod)
        {
            return _dataContext.Genres.Include(m => m.Movies).FirstOrDefault(SearchMethod);
        }
    }
}
