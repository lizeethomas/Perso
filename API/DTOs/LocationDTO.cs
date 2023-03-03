namespace MyWebsite.DTOs
{
    public class LocationDTO
    {
        public string Name { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }

        public string? Country { get; set; }
        public string? City { get; set; }
    }
}
