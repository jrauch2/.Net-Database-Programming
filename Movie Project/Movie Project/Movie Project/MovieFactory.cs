using System.Collections.Generic;
using System.Text.RegularExpressions;

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
        private static readonly MovieFactory Instance = new MovieFactory();
        private static readonly GenreTracker GenreTrackerInstance = GenreTracker.GetGenreTrackerInstance();
        private const char GenreSplit = '|';

        /// <summary>
        /// Private constructor.
        /// </summary>
        // Private constructor.
        private MovieFactory()
        {

        }

        /// <summary>
        /// Get the <c>MovieFactory</c> instance.
        /// </summary>
        /// <returns>The <c>MovieFactory</c> instance</returns>
        // Get the MovieFactory instance.
        public static MovieFactory GetMovieFactoryInstance()
        {
            return Instance;
        }

        /// <summary>
        /// Generate a new movie.
        /// This method is overloaded.
        /// </summary>
        /// <param name="id">an int representing the id of the ticket.</param>
        /// <param name="title">a <c>string</c> of the movie's title</param>
        /// <param name="genres">a <c>List</c> of genres</param>
        /// <returns>The generated movie.</returns>
        // Generate a new movie.
        // This method is overloaded.
        // This takes an int id, string title, and List<string> genres.
        public Movie NewMovie(int id, string title, List<string> genres)
        {
            return new Movie(id, title, genres);
        }

        /// <summary>
        /// Generate a new movie.
        /// This method is overloaded.
        /// </summary>
        /// <param name="movieString">a <c>string</c> of the movie's properties.</param>
        /// <param name="regularExpression">a <c>string</c> of a regular expression for parsing the <c>movieString</c></param>
        /// <returns>The generated movie.</returns>
        // Generate a new movie.
        // This method is overloaded.
        // This takes an string of the movie's properties and a regular expression string.
        public Movie NewMovie(string movieString, string regularExpression)
        {
            var sa = Regex.Split(movieString, regularExpression);
            var id = int.Parse(sa[0]);
            var title = sa[1];
            var genres = ParseMovieGenres(sa[2], GenreSplit);
            return new Movie(id, title, genres);
        }

        /// <summary>
        /// Parse the string of genres, delimited by the <c>char</c> argument.
        /// </summary>
        /// <param name="genreString">The string to be parsed</param>
        /// <param name="genreSplit">The <c>char</c> to split the string on.</param>
        /// <returns>A <c>List</c> of genres.</returns>
        // Parse the string of genres, delimited by the char argument.
        public static List<string> ParseMovieGenres(string genreString, char genreSplit)
        {
            var list = new List<string>();
            if (genreString != null)
            {
                var sa = genreString.Split(genreSplit);
                foreach (var g in sa)
                {
                    var t = g.Trim();
                    if (!GenreTrackerInstance.Contains(t.FirstCharToUpper()))
                    GenreTrackerInstance.AddGenre(t.FirstCharToUpper());
                    list.Add(t);
                }
            }
            list.Sort();

            return list;
        }
    }
}