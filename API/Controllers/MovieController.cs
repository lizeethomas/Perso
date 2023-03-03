using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyWebsite.DTOs;
using MyWebsite.Enums;
using MyWebsite.Models;
using MyWebsite.Services;
using MyWebsite.Tools;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace MyWebsite.Controllers
{
    [Route("movies")]
    [ApiController]
    public class MovieController : Controller
    {
        private MovieService _movieService;

        public MovieController(MovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        public IActionResult DisplayAllMovies()
        {

            List<Movie> movies = _movieService.GetMovies();
            List<NewMovieExitDTO> moviesDTO  = new List<NewMovieExitDTO>();
            movies.ForEach(m =>
            {
                List<MovieGenres> movieGenres = new List<MovieGenres>();
                m.Genres.ForEach(g =>
                {
                    movieGenres.Add(g.Name);
                });
                NewMovieExitDTO tmpMovie = new NewMovieExitDTO()
                {
                    Id = m.Id,
                    Title = m.Title,
                    Director = m.Director,
                    Date = m.Date,
                    Rating = m.Rating,
                    Commentary = m.Commentary,
                    Url = m.Url,
                    Genres = movieGenres,
                    Edited = m.Edited,
                };
                moviesDTO.Add(tmpMovie);
            });

            if (moviesDTO != null)
            {
                return Ok(moviesDTO);
            }
            return BadRequest();

        }

        //[HttpGet("of/{genre}")]
        //public IActionResult GetAllByGenre(MovieGenres genre) 
        //{
        //    List<Movie> movies = _movieService.GetMovies();
        //    Genre g = _movieService.GetGenreByName(genre.K);
        //    movies.ForEach(m =>
        //    {
        //        if (m.Genres.Any(g => g.Name == genre)) { result.Add(m); }
        //    });
        //    if (result != null)
        //    {
        //        return Ok(result);
        //    }
        //    return NotFound();
        //}

        [HttpGet("{id}")]
        public IActionResult DisplayMovie(int id)
        {

            Movie movie = _movieService.GetMovieById(id);
            List<MovieGenres> movieGenres = new List<MovieGenres>();
            movie.Genres.ForEach(g =>
            {
                movieGenres.Add(g.Name);
            });
            NewMovieExitDTO movieDTO = new NewMovieExitDTO()
            {
                Id = movie.Id,
                Title= movie.Title,
                Director = movie.Director,
                Date = movie.Date,
                Rating = movie.Rating,
                Commentary = movie.Commentary,
                Url = movie.Url,
                Genres = movieGenres,
                Edited = movie.Edited,
            };

            if (movieDTO != null)
            {
                return Ok(movieDTO);
            }
            return BadRequest();

        }

        [HttpPut]
        [Authorize("admin")]
        public IActionResult EditMovie([FromBody] NewMovieEntryDTO movie)
        {
            List<int> newEntryGenres = movie.Genres;
            var listOfGenres = new List<Genre>();
            newEntryGenres.ForEach(g =>
            {
                Genre genre = _movieService.GetGenreByName(g);
                listOfGenres.Add(genre);
            });

            Movie tmp = _movieService.GetMovieByTitle(movie.Title);

            Movie modifiedEntry = new Movie()
            {
                Id = tmp.Id,
                Title = movie.Title,
                Director = movie.Director,
                Date = movie.Date,
                Rating = movie.Rating,
                Commentary = movie.Commentary,
                Url = movie.Url,
                Genres = listOfGenres,
                Edited = DateTime.Now,
            };

            if (_movieService.Update(modifiedEntry))
            {
                return Ok(true);
            }
            return NotFound();
        }

        [HttpDelete("{id}")]
        [Authorize("admin")]
        public IActionResult DeleteMovie(int id)
        {
            if (_movieService.Delete(id))
            {
                return Ok();
            }
            return BadRequest(407);
        }

        [HttpPost]
        [Authorize("admin")]
        public IActionResult AddMovie([FromBody] NewMovieEntryDTO movie)
        {
            List<int> newEntryGenres = movie.Genres;
            var listOfGenres = new List<Genre>();
            newEntryGenres.ForEach(g =>
            {
                Genre genre = _movieService.GetGenreByName(g);
                listOfGenres.Add(genre);
            });

            Movie newEntry = new Movie()
            {
                Title = movie.Title,
                Director = movie.Director,
                Date = movie.Date,
                Rating = movie.Rating,
                Commentary = movie.Commentary,
                Url = movie.Url,
                Genres= listOfGenres,
                Edited = DateTime.Now,
            };

            if (_movieService.Save(newEntry))
            {
                List<MovieGenres> movieGenres= new List<MovieGenres>();
                listOfGenres.ForEach(g =>
                {
                    movieGenres.Add(g.Name);
                });
                NewMovieExitDTO newMovie = new NewMovieExitDTO()
                {
                    Title = movie.Title,
                    Director = movie.Director,
                    Date = movie.Date,
                    Rating = movie.Rating,
                    Commentary = movie.Commentary,
                    Url = movie.Url,
                    Genres = movieGenres,
                };
                return(Ok(newMovie));   
            }
            return BadRequest();
        }

        [HttpGet("/genres/")]
        public IActionResult DisplayAllGenres()
        {
            List<string> genres = _movieService.DisplayAllGenres();
            if (genres != null)
            {
                return Ok(genres);
            }
            return BadRequest();
        }

        [HttpGet("/genres/{id}")]
        public IActionResult DisplayGenre(int id)
        {
            string genre = _movieService.DisplayGenreById(id);
            if (genre != null)
            {
                return Ok(genre);
            }
            return NotFound();
        }
    }
}
