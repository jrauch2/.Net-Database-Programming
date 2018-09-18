using System;
using System.Collections.Generic;
using NLog;

namespace Movie_Project
{
    /// <summary>
    /// The <c>GenreTracker</c> Class.
    /// Follows a singleton pattern.
    /// Contains a <c>List</c> of genres.
    /// </summary>
    // The GenreTracker class.
    // Follows a singleton pattern.
    // Contains a List<string> of genres.
    internal sealed class GenreTracker
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
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
        // Get the instance of GenreTracker.
        // Returns the instance of GenreTracker.
        public static GenreTracker GetGenreTrackerInstance()
        {
            return Instance;
        }

        /// <summary>
        /// Get the list of genres.
        /// </summary>
        /// <returns>The <c>List</c> of genres</returns>
        // Get the list of genres.
        // Returns List<string> of genres.
//        public List<string> GetAllMovieGenres()
//        {
//            _movieGenres.Sort();
//            return _movieGenres;
//        }

        public bool Contains(string s)
        {
            return _movieGenres.Contains(s);
        }

        /// <summary>
        /// Add a genre to the <c>List</c>.
        /// </summary>
        /// <param name="genre">The genre to be added.</param>
        // Add a genre to the List<string>.
        public void AddGenre(string genre)
        {
            try
            {
                if (_movieGenres.Contains(genre))
                {
                    _logger.Debug("Genre not added. Genre already exists in list.");
                    return;
                }
                else if (genre == null)
                {
                    _logger.Error("Genre not added. Argument is null.");
                    return;
                }

                _movieGenres.Add(genre);
                _logger.Trace("Genre added successfully.");
            }
            catch (ArgumentNullException ane)
            {
                _logger.Error(ane.Source + "\n" + ane.Message);
            }
            catch (ArgumentException ae)
            {
                _logger.Error(ae.Source + " argument cannot be empty.");
            }
            catch (Exception e)
            {
                _logger.Error(e.Source + "\n" + e.Message);
            }
        }

        public string ToFormattedString()
        {
            return _movieGenres.ToFormattedString();
        }

        public string[] GetGenres()
        {
            var genres = new string[_movieGenres.Count];
            _movieGenres.Sort();
            _movieGenres.CopyTo(genres);
            return genres;
        }
    }
}