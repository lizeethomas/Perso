using Microsoft.AspNetCore.Mvc;
using MyWebsite.DTOs;
using MyWebsite.Models;
using MyWebsite.Services;

namespace MyWebsite.Controllers
{
    [Route("map")]
    public class LocationController : Controller
    {
        private LocationService _locationService;

        public LocationController(LocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpGet]
        public IActionResult GetAllLocations()
        {
            List<Location> list = _locationService.GetLocations();

            if (list != null)
            {
                return Ok(list);
            }
            return BadRequest();
        }

        [HttpPost]
        public IActionResult AddNewLocation([FromBody] LocationDTO location)
        {
            Location newLocation = new Location()
            {
                Name = location.Name,
                Latitude = location.Latitude,
                Longitude = location.Longitude
            };
            if (location.City != null) { newLocation.City = location.City; }
            if (location.Country != null) { newLocation.Country = location.Country; }

            if (_locationService.Save(newLocation))
            {
                return Ok(newLocation);
            }
            return BadRequest();
        }

        [HttpPut("picture")]
        public IActionResult AddPictureToLocation([FromBody] NewPictureDTO newPictureDTO)
        {
            Location location = _locationService.GetLocationById(newPictureDTO.Id);
            if (location != null)
            {
                Picture picture = new Picture()
                {
                    Title = newPictureDTO.Title,
                    PictureUrl = newPictureDTO.Url,
                    Description = newPictureDTO.Description,
                };
                location.Pictures.Add(picture);
                if (_locationService.Update(location)) {
                    return Ok();
                }
            }
            return NotFound();
        }

        [HttpDelete("{id}")] 
        public IActionResult DeleteLocation(int id)
        {
            Location location = _locationService.GetLocationById(id);
            if (location != null)
            {
                _locationService.Delete(location);
                return Ok();
            }
            return NotFound();
        }
    }
}
