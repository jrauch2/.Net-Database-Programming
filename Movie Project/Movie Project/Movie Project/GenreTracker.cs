using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;

namespace Movie_Project
{
    /// <summary>
    /// The GenreTracker Class.
    /// Follows a singleton pattern.
    /// Contains a <c>List</c> of genres.
    /// </summary>
    // The GenreTracker class.
    // Follows a singleton pattern.
    // Contains a List<string> of genres.
    internal sealed class GenreTracker
    {
        private static readonly GenreTracker Instance = new GenreTracker();
        private readonly List<string> _movieGenres = new List<string>();

        /// <summary>
        /// Private constructor.
        /// To get the instance, call <c>GetGenreTrackerInstance()</c>
        /// </summary>
        // Private constructor.
        // To get the instance, call <c>GetGenreTrackerInstance()</c>
        private GenreTracker()
        {

        }

        /// <summary>
        /// Get the instance of <c>GenreTracker</c>.
        /// </summary>
        /// <returns>The <c>GenreTracker</c> instance.</returns>
        // 
        public static GenreTracker GetGenreTrackerInstance()
        {
            return Instance;
        }

        public List<string> GetAllMovieGenres()
        {
            return _movieGenres;
        }

        public void AddGenre(string genre)
        {
            if (!_movieGenres.Contains(genre.FirstCharToUpper()))
            {
                _movieGenres.Add(genre.FirstCharToUpper());
            }
        }
    }
}