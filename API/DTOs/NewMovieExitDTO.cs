using MyWebsite.Enums;

namespace MyWebsite.DTOs
{
    public class NewMovieExitDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Director { get; set; }
        public int Date { get; set; }
        public int Rating { get; set; }
        public List<MovieGenres> Genres { get; set; }
        public string? Commentary { get; set; }
        public string? Url { get; set; }
        public DateTime Edited { get; set; }
    }
}
