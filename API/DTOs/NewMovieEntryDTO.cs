using MyWebsite.Models;

namespace MyWebsite.DTOs
{
    public class NewMovieEntryDTO
    {
        public string Title { get; set; }
        public string Director { get; set; }
        public int Date { get; set; }
        public int Rating { get; set; }
        public List<int> Genres { get; set; }
        public string? Commentary { get; set; }
        public string? Url { get; set; }

    }
}
