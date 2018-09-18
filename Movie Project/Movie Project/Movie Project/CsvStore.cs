using System;
using System.Collections.Generic;
using System.Linq;

namespace Movie_Project
{
    /// <summary>
    /// The abstract <c>CsvStore</c> class.
    /// Contains a <c>List</c> of stored <c>Movies</c>.
    /// </summary>
    // The abstract CsvStore class.
    // Contains a List<Movie>.
    internal abstract class CsvStore
    {
        protected static List<Movie> StoredMovies = new List<Movie>();

        //Get the highest used ID.
        public int GetMaxId()
        {
            if (StoredMovies.Any())
            {
                var maxId = StoredMovies.Max(movie => movie.GetId());
                return maxId;
            }
            else
            {
                return 0;
            }

        }

        // Get the movie with matching ID.
        public bool FindMovieById(int id, out Movie movie)
        {
            movie = StoredMovies.Find(m => m.GetId() == id);
            return movie != null;
        }

        public bool FindMovieByTitle(string title, out Movie movie)
        {
            if (title != null)
            {
                if (title == "") throw new ArgumentException("Argument string cannot be empty");
            }
            else
                throw new ArgumentNullException();

            foreach (var storedMovie in StoredMovies)
            {
                if (!title.ToUpper().Equals(storedMovie.GetTitle().ToUpper())) continue;
                movie = storedMovie;
                return true;
            }

            movie = null;
            return false;
        }

        public Movie[] GetAllMovies()
        {
            Movie[] movies = new Movie[StoredMovies.Count()];
            StoredMovies.CopyTo(movies);
            return movies;
        }
    }
}