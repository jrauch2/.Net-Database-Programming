namespace Movie_Project
{/// <summary>
    /// The <c>IInput</c> interface.
    /// provides signatures for methods to query stored movies.
    /// </summary>
    internal interface IInput
    {
        /// <summary>
        /// Gets the highest ID stored.
        /// Used to generate new movies without reusing IDs.
        /// </summary>
        /// <returns>
        /// The highest ID as an <c>int</c>.
        /// </returns>
        int GetMaxId();

        /// <summary>
        /// Get a <c>Movie</c> by ID.
        /// </summary>
        /// <param name="id">The id of the desired <c>Movie</c> as an <c>int</c>.</param>
        /// <param name="movie"></param>
        /// <returns>The desired <c>Movie</c>, if found.</returns>
        bool FindMovieById(int id, out Movie movie);

        /// <summary>
        /// Get a <c>Movie</c> by title.
        /// </summary>
        /// <param name="title">The title of the desired <c>Movie</c></param>
        /// <param name="movie">The <c>Movie</c> wanted</param>
        /// <returns><c>true</c> if movie is found, <c>false</c> if not.</returns>
        bool FindMovieByTitle(string title, out Movie movie);

        bool Contains(Movie movie);

        void AddMovie(Movie movie);
    }
}