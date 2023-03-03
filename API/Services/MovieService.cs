using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;
using MyWebsite.Enums;
using MyWebsite.Models;
using MyWebsite.Repositories;
using System.ComponentModel.DataAnnotations;

namespace MyWebsite.Services
{
    public class MovieService : Controller
    {
        private readonly MovieRepository _movieRepo;
        private readonly GenreRepository _genreRepo;

        public MovieService(MovieRepository movieRepo, GenreRepository genreRepo)
        {
            _movieRepo = movieRepo;
            _genreRepo = genreRepo;
        }

        public List<Movie> GetMovies()
        {
            List<Movie> movies = _movieRepo.FindAll();
            return movies;  
        }

        public List<Movie> GetMoviesSorted(Func<Movie,bool> search)
        {
            List<Movie> movies = _movieRepo.FindAllSorted(search);
            return movies;
        }

        //public List<Movie> GetMoviesOfGenre(MovieGenres genre)
        //{
        //    List<Movie> result = new List<Movie>();
        //    bool test = false;
        //    _movieRepo.FindAll().ForEach(m =>
        //    {
        //        m.Genres.ForEach(g =>
        //        {
        //            if (g.Name == genre) { test = true; }
        //        });
        //        if (test) { result.Add(m); }
        //    });
        //    return result;
        //}

        public Movie GetMovieById(int id)
        {
            Movie movie = _movieRepo.FindById(id);
            return movie;
        }

        public Movie GetMovieByTitle(string name)
        {
            Movie movie = _movieRepo.FindByTitle(name);
            return movie;
        }

        public bool Save(Movie movie)
        {
            if (movie != null)
            {
                _movieRepo.Save(movie);
                return true;
            }
            return false;
        }

        public bool Delete(int id)
        {
            Movie movie = GetMovieById(id);
            if (movie != null)
            {
                _movieRepo.Delete(movie);
                return true;
            }
            return false;
        }

        public bool Update(Movie newmovie)
        {

            Movie oldmovie = GetMovieById(newmovie.Id);
            List<Genre> genres = new List<Genre>();
            newmovie.Genres.ForEach(g =>
            {
                genres.Add(g);
            });

            if (oldmovie != null)
            {
                oldmovie.Title = newmovie.Title;
                oldmovie.Director = newmovie.Director;
                oldmovie.Date = newmovie.Date;
                oldmovie.Commentary = newmovie.Commentary;
                oldmovie.Edited = newmovie.Edited;
                oldmovie.Genres = genres;
                oldmovie.Rating = newmovie.Rating;
                oldmovie.Url = newmovie.Url;
                _movieRepo.Update();
                return true;
            }
            return false;
        }

        public List<string> DisplayAllGenres()
        {
            List<Genre> genres = _genreRepo.FindAll();
            List<string> data = new List<string>();
            genres.ForEach(g =>
            {
                data.Add(g.Name.ToString());
            });
            return data;
        }

        public Genre GetGenreByName(int id)
        {
            return _genreRepo.SearchOne(g => g.Name == (MovieGenres)id);
        }

        public string DisplayGenreById(int id)
        {
            Genre genre = _genreRepo.FindById(id);
            return genre.Name.ToString();
        }
    }
}
