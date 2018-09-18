using System;
using System.IO;

namespace Movie_Project
{
    /// <inheritdoc cref="IOutput" />
    /// <summary>
    /// The <c>CsvOutput</c> class.
    /// Extends the <c>CsvStore</c> class.
    /// Implements the <c>IOutput</c> interface.
    /// Used to store movies in a CSV file.
    /// </summary>
    /// <see cref="CsvStore"/>
    /// <seealso cref="IOutput"/>
    internal class CsvOutput : CsvStore, IOutput
    {
        private string _fileName;
        private const string MovieExistsMessage = "Movie not added, {0} already exists";

        /// <summary>
        /// Output <c>List</c> of <c>Movie</c> objects to a csv file.
        /// </summary>
        /// <param name="fileName">The name of the file to be written to.</param>
        public CsvOutput(string fileName)
        {
            SetFileName(fileName);
        }

        /// <summary>
        /// Set <c>fileName</c>.
        /// </summary>
        /// <param name="fileName">The name of the file to be opened.</param>
        private void SetFileName(string fileName)
        {
            _fileName = fileName;
        }

        public void AddMovie(Movie movie)
        {
            if (movie is null)
            {
                throw new ArgumentNullException();
            }
            else if (StoredMovies.Contains(movie))
            {
                throw new ArgumentException(MovieExistsMessage, nameof(movie));
            }
            else if (FindMovieByTitle(movie.GetTitle(), out _))
            {
                throw new ArgumentException(MovieExistsMessage, nameof(movie.GetTitle));
            }
            else if (FindMovieById(movie.GetId(), out _))
            {
                throw new ArgumentException(MovieExistsMessage, nameof(movie.GetId));
            }
            else
            {
                StoredMovies.Add(movie);
                if (File.Exists(_fileName))
                {
                    File.Copy(_fileName, _fileName + ".bak", true);
                }
                using (var output = new StreamWriter(_fileName, true))
                {
                    output.WriteLine(movie.ToString());
                }
            }
        }

//        /// <summary>
//        /// Write out <c>List</c> of <c>Movie</c> objects to storage medium.
//        /// </summary>
//        // Write out a List<Movie> to storage medium.
//        public void StoreMovies()
//        {
//            if (File.Exists(_fileName))
//            {
//                File.Copy(_fileName, _fileName + ".bak",true);
//            }
//            using (var output = new StreamWriter(_fileName))
//            {
//                foreach (var storedMovie in StoredMovies)
//                {
//                    output.WriteLine(storedMovie.ToString());
//                }
//            }
//        }
    }
}