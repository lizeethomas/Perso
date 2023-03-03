using MyWebsite.Enums;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace MyWebsite.Models
{
    [Table("movies")]
    public class Movie
    {
        private int id;
        private string title;
        private string director;
        private int date;
        private int rating;
        private Genre[] genres;
        private string commentary;
        private string url;

        [Column("id")]
        public int Id { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("director")]
        public string Director { get; set; }

        [Column("date")]
        public int Date { get; set; }

        [Column("rating")]
        public int Rating { get; set; }

        [Column("genres")]
        [JsonIgnore]
        public List<Genre> Genres { get; set; }

        [Column("commentary")]
        public string? Commentary { get; set; }

        [Column("edited")]
        public DateTime Edited { get; set; }

        [Column("url")]
        public string? Url { get; set; }

        public Movie()
        {
            Genres = new List<Genre>();
            //Edited = DateTime.Now;
        }

    }
}
