using System.Collections.Generic;
using System.Text.RegularExpressions;
using NLog;

namespace Movie_Project
{
    /// <summary>
    /// The <c>MovieFactory</c> Class.
    /// Follows a singleton pattern.
    /// Builds all <c>Movie</c> objects.
    /// </summary>
    // The MovieFactory class.
    // Follows a singleton pattern.
    // Builds all Movie objects.
    internal sealed class MovieFactory
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private static readonly MovieFactory Instance = new MovieFactory();
        private static readonly GenreTracker GenreTrackerInstance = GenreTracker.GetGenreTrackerInstance();
        private const string Rx = ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)";
        private const char GenreSplit = '|';

        /// <summary>
        /// Private constructor.
        /// To access <c>MovieFactory</c>, call <c>GetMovieFactoryInstance()</c>.
        /// </summary>
        // Private constructor.
        // To access MovieFactory, call GetMovieFactoryInstance().
        private MovieFactory()
        {

        }

        /// <summary>
        /// Get the <c>MovieFactory</c> instance.
        /// </summary>
        /// <returns>THe <c>MovieFactory</c> instance</returns>
        // Get the MovieFactory instance.
        public static MovieFactory GetMovieFactoryInstance()
        {
            return Instance;
        }

        public Movie NewMovie(int id, string title, List<string> genres)
        {
            return new Movie(id, title, genres);
        }

        public Movie NewMovie(string movieString)
        {
            var sa = Regex.Split(movieString, Rx);
            var id = int.Parse(sa[0]);
            var title = sa[1];
            var genres = ParseMovieGenres(sa[2]);
            return new Movie(id, title, genres);
        }

        private List<string> ParseMovieGenres(string genreString)
        {
            var list = new List<string>();
            if (genreString != null)
            {
                var sa = genreString.Split(GenreSplit);
                foreach (var s in sa)
                {
                    if (GenreTrackerInstance.GetAllMovieGenres().Contains(s))
                    {
                        list.Add(s);
                    }
                    else
                    {
                        GenreTrackerInstance.AddGenre(s);
                        list.Add(s);
                    }
                }
            }
            list.Sort();

            return list;
        }
    }
}