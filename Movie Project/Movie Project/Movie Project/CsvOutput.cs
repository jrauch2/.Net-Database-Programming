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

        public void StoreMovies()
        {
            using (var output = new StreamWriter(_fileName))
            {
                try
                {
                    StoredMovies.Sort();
                    foreach (var storedMovie in StoredMovies)
                    {
                        output.WriteLine(storedMovie.ToString());
                    }
                }
                catch (Exception e)
                {
                    if (e.InnerException != null) throw e.InnerException;
                }
            }
        }
    }
}