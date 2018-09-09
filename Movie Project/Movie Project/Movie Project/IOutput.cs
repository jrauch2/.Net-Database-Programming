using System.Collections.Generic;

namespace Movie_Project
{
    internal interface IOutput
    {
        /// <summary>
        /// Write out <c>List</c> of <c>Movie</c> objects to storage medium.
        /// </summary>
        /// <param name="movies">A <c>List</c> of <c>Movie</c> objects</param>
        // Write out a List<Movie> to storage medium.
        void WriteAll(List<Movie> movies);
    }
}