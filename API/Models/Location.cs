using System.ComponentModel.DataAnnotations.Schema;

namespace MyWebsite.Models
{
    [Table("locations")]
    public class Location
    {
        public int id;

        public string name;
        public float latitude;
        public float longitude;

        public string? country;
        public string? city;
        public int? population;

        public List<Picture>? pictures;

        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("latitude")] 
        public float Latitude { get; set;}

        [Column("longitude")]
        public float Longitude { get; set;}

        [Column("country")]
        public string? Country { get; set; }

        [Column("city")]
        public string? City { get; set; }

        [Column("population")]
        public int? Population { get; set;}

        public List<Picture> Pictures { get; set;}

        public Location()
        {
            Pictures = new List<Picture> { };   
        }

    }
}
