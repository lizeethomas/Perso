using MyWebsite.Models;
using MyWebsite.Repositories;

namespace MyWebsite.Services
{
    public class LocationService
    {
        private readonly LocationRepository _locationRepo;

        public LocationService(LocationRepository locationRepo)
        {
            _locationRepo = locationRepo;
        }

        public List<Location> GetLocations()
        {
            List<Location> locations = _locationRepo.FindAll();
            return locations;
        }

        public Location GetLocationById(int id)
        {
            Location location = _locationRepo.FindById(id);
            return location;
        }

        public bool Update(Location modifiedlocation)
        {
            Location oldLocation = GetLocationById(modifiedlocation.Id);
            List<Picture> pictures = new List<Picture>();
            modifiedlocation.Pictures.ForEach(p =>
            {
                pictures.Add(p);
            });
            if (oldLocation != null)
            {
                oldLocation.Name = modifiedlocation.Name;
                oldLocation.City = modifiedlocation.City;
                oldLocation.Pictures = pictures;
                oldLocation.Latitude = modifiedlocation.Latitude;
                oldLocation.Longitude = modifiedlocation.Longitude;
                oldLocation.Country = modifiedlocation.Country;
                if (_locationRepo.Update())
                {
                    return true;
                };
            }
            return false;
        }

        public bool Save(Location location)
        {
            if (location != null)
            {
                _locationRepo.Save(location);
                return true;
            }
            return false;
        }

        public bool Delete(Location location)
        {
            if (location != null)
            {
                _locationRepo.Remove(location);
                return true;
            }
            return false;
        }
    }
}
