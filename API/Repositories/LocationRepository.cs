using Microsoft.EntityFrameworkCore;
using MyWebsite.Models;
using MyWebsite.Tools;

namespace MyWebsite.Repositories
{
    public class LocationRepository : BaseRepository<Location>
    {
        public LocationRepository(DataDbContext dataContext) : base(dataContext)
        {
            
        }

        public override List<Location> FindAll()
        {
            return _dataContext.Locations.Include(l => l.Pictures).ToList();
        }

        public override Location FindById(int id)
        {
            return _dataContext.Locations.Include(l => l.Pictures).FirstOrDefault(l => l.Id == id);
        }

        public override bool Save(Location element)
        {
            _dataContext.Locations.Add(element);
            return base.Update();
        }

        public bool Remove(Location element)
        {
            _dataContext.Locations.Remove(element);
            return base.Update();
        }

        public override List<Location> SearchAll(Func<Location, bool> SearchMethod)
        {
            throw new NotImplementedException();
        }

        public override Location SearchOne(Func<Location, bool> SearchMethod)
        {
            throw new NotImplementedException();
        }
    }
}
