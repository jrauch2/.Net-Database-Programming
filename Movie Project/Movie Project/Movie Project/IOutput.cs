namespace Movie_Project
{
    internal interface IOutput
    {
        /// <summary>
        /// Write out <c>List</c> of <c>Movie</c> objects to storage medium.
        /// </summary>
        // Write out a List<Movie> to storage medium.
        void StoreMovies();
    }
}