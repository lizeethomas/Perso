namespace MyWebsite.Tools
{
    public record TMDB
    {
        public string apiKey;
        public string baseUrl;

        public TMDB()
        {
           apiKey = "857013156f2fa06d7ada9a9d6e51896d";
           baseUrl = "https://api.themoviedb.org/3/search/movie?api_key=" + apiKey + "&query=";
        }
    }
}
