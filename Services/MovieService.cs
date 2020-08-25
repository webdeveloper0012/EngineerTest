using EngineerTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EngineerTest.Services
{
    public class MovieService
    {
        private readonly ApplicationDbContext _context;
        /// <summary>
        /// Create movie object to work with database
        /// </summary>
        /// <param name="context">Database connection</param>
        public MovieService(ApplicationDbContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }
        /// <summary>
        /// Save movie in database
        /// </summary>
        /// <param name="movieDbMovie">Movie information</param>
        /// <param name="isTopRated">True if movie is top rated, false if up coming </param>
        public void Save(MovieDbMovie movieDbMovie, bool isTopRated)
        {
            Movie movie = new Movie();
            movie.IsTopRated = isTopRated;
            movie.OriginalLanguage = movieDbMovie.original_language;
            movie.Overview = movieDbMovie.overview;
            movie.Popularity = movieDbMovie.popularity;
            movie.PosterPath = movieDbMovie.poster_path;
            movie.ReleaseDate = DateTime.Now;
            movie.SysDatetime = DateTime.Now;
            movie.Title = movieDbMovie.title;
            movie.VoteAverage = movieDbMovie.vote_average;
            _context.Movies.Add(movie);
            _context.SaveChanges();
        }

        /// <summary>
        /// Delete movie from database
        /// </summary>
        /// <param name="isTopRated">True if movie is top rated, false if up coming</param>
        public void DeleteAll(bool isTopRated)
        {
            var movies = _context.Movies.Where(m => m.IsTopRated == isTopRated);
            _context.Movies.RemoveRange(movies);
            _context.SaveChanges();
        }

        /// <summary>
        /// Get list of movie
        /// </summary>
        /// <returns>List of movie</returns>
        public List<Movie> GetAll()
        {
            var movies = _context.Movies.ToList();
            return movies;
        }
        /// <summary>
        /// Get movie list from API and save in database
        /// </summary>
        /// <param name="isTopRated">True if movie is top rated, false if up coming</param>
        public void PopulateMovies(bool isTopRated)
        {
            TheMovieDb movieDb = new TheMovieDb();
            MovieDbResult movieDbResult = movieDb.FetchMovies(isTopRated);
            if(movieDbResult != null && movieDbResult.results != null && movieDbResult.results.Length > 0)
            {
                this.DeleteAll(isTopRated);
                foreach (var movie in movieDbResult.results)
                {
                    this.Save(movie, isTopRated);
                }
            }
        }

    }
}
