using MyWebsite.Enums;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWebsite.Models
{
    [Table("genres")]
    public class Genre
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public MovieGenres Name { get; set; }

        [JsonIgnore]
        public List<Movie> Movies { get; set; }

        public Genre()
        {
            Name = new MovieGenres();
            Movies = new List<Movie>();
        }
    }
}
