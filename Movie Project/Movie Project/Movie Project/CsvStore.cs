using System.Collections.Generic;

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
    }
}