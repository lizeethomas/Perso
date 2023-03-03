using Microsoft.EntityFrameworkCore;
using MyWebsite.Models;
using MyWebsite.Tools;
using System.Linq;

namespace MyWebsite.Repositories
{
    public class MovieRepository : BaseRepository<Movie>
    {
        public MovieRepository(DataDbContext dataContext) : base(dataContext)
        {
        }

        public override List<Movie> FindAll()
        {
            return _dataContext.Movies.Include(m => m.Genres).OrderByDescending(m => m.Edited).ToList();
        }

        public List<Movie> FindAllSorted(Func<Movie, bool> method)
        {
            return _dataContext.Movies.Include(m => m.Genres).OrderBy(method).ToList();
        }

        public override Movie FindById(int id)
        {
            return _dataContext.Movies.Include(g => g.Genres).FirstOrDefault(m => m.Id == id);
        }

        public Movie FindByTitle(string title)
        {
            return _dataContext.Movies.Include(g => g.Genres).FirstOrDefault(m => m.Title == title);
        }

        public override bool Save(Movie element)
        {
            _dataContext.Movies.Add(element);
            return base.Update();
        }

        public override bool Update()
        {
            return base.Update();
        }

        public bool Delete(Movie element)
        {
            _dataContext.Movies.Remove(element);
            return base.Update();
        }

        public override List<Movie> SearchAll(Func<Movie, bool> SearchMethod)
        {
            return _dataContext.Movies.Include(m => m.Genres).Where(SearchMethod).ToList();
        }

        public override Movie SearchOne(Func<Movie, bool> SearchMethod)
        {
            return _dataContext.Movies.Include(m => m.Genres).FirstOrDefault(SearchMethod);
        }
    }
}
