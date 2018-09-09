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

        /// <summary>
        /// Takes a <c>List</c> of <c>Movie</c> objects and adds them to the <c>StoredMovies</c> list.
        /// Will replace matching IDs in <c>StoredMovies</c> before appending new <c>Movie</c> objects.
        /// </summary>
        /// <param name="movies"></param>
        // Takes a List<Movie> and adds it to StoredMovies.
        // Will replace matching IDs in StoredMovies before appending new Movie objects.
        protected static void UpdateStoredTickets(List<Movie> movies)
        {
            for (var i = 0; i < movies.Count; i++)
            {
                for (var j = 0; j < StoredMovies.Count; j++)
                {
                    if (StoredMovies[j].GetId() != movies[i].GetId()) continue;
                    StoredMovies[j] = movies[i];
                    movies.RemoveAt(i);
                }
            }
            movies.Sort((t1, t2) => t1.GetId().CompareTo(t2.GetId()));
            foreach (var movie in movies)
            {
                StoredMovies.Add(movie);
            }
        }
    }
}