using System.Collections.Generic;
using NLog;

namespace Movie_Project
{
    /// <summary>
    /// The <c>MovieTracker</c> Class.
    /// Follows a singleton pattern.
    /// Contains a <c>List</c> of movies.
    /// </summary>
    // The MovieTracker class.
    // Follows a singleton pattern.
    // Contains a List<Movie> of movies.
    internal sealed class MovieTracker
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private static readonly MovieTracker Instance = new MovieTracker();
        private readonly List<Movie> _movies = new List<Movie>();

        /// <summary>
        /// Private constructor.
        /// To get the instance, call <c>GetMovieTrackerInstance()</c>
        /// </summary>
        // Private constructor.
        // To get the instance, call <c>GetMovieTrackerInstance()</c>
        private MovieTracker()
        {

        }

        /// <summary>
        /// Get the instance of <c>MovieTracker</c>.
        /// </summary>
        /// <returns>The <c>MovieTracker</c> instance.</returns>
        // Get the instance of MovieTracker.
        // Returns the MovieTracker instance.
        public static MovieTracker GetMovieTrackerInstance()
        {
            return Instance;
        }

        /// <summary>
        /// Get the <c>List</c> of movies.
        /// </summary>
        /// <returns>The <c>List</c> of movies.</returns>
        // Get the list of movies.
        // Returns the List<Movie>.
        public List<Movie> GetMovieList()
        {
            return _movies;
        }

        /// <summary>
        /// Add a <c>Movie</c> to the <c>List</c>.
        /// </summary>
        /// <param name="movie">The <c>Movie</c> to be added.</param>
        // Add a movie to the list.
        public void AddMovie(Movie movie)
        {
            if (_movies.Contains(movie))
            {
                _logger.Debug("Movie not added. Movie already exists in list.");
                return;
            } else if (movie == null)
            {
                _logger.Debug("Movie not added. Movie argument is null.");
            }
            _movies.Add(movie);
            _logger.Trace("Movie added successfully.");
        }
    }
}