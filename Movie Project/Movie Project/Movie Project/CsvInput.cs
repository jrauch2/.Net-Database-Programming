using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NLog;
using NUnit.Framework.Api;

namespace Movie_Project
{
    /// <inheritdoc cref="IInput" />
    /// <summary>
    /// The <c>CsvInput</c> class.
    /// Implements the <c>IInput</c> interface.
    /// Extends the <c>CsvStore</c> class.
    /// Used to load stored movies from a CSV file.
    /// </summary>
    // The CsvInput class.
    // Implements the IInput interface.
    // Extends the CsvStore class.
    // Used to oad stored movies from a CSV file.
    internal sealed class CsvInput : CsvStore, IInput
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static readonly CsvInput Instance = new CsvInput();
        private static readonly MovieFactory MovieFactoryInstance = MovieFactory.GetMovieFactoryInstance();
        private const string RegularExpression = ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)";
        private static string _fileName;
        private const string MovieNotFoundMessage = "Movie not found.";
        private const string ExceptionMessage = "There was an Exception in ";
        private const string FileNotExistMessage = "The file does not exist.";

        /// <summary>
        /// Private constructor for <c>CsvInput</c>.
        /// </summary>
        private CsvInput()
        {
            
        }

        public static CsvInput GetCsvInputInstance(string fileName)
        {
            SetFileName(fileName);
            return Instance;
        }

        private static void StartCsvInput()
        {
            SetFileName(_fileName);
            if (File.Exists(_fileName))
            {
                using (var file = new StreamReader(_fileName))
                {
                    var count = 0;
                    try
                    {
                        while (!file.EndOfStream)
                        {
                            if (count++ == 0)
                            {
                                file.ReadLine();
                            }
                            var line = file.ReadLine();
                            StoredMovies.Add(MovieFactoryInstance.NewMovie(line, RegularExpression));
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("{0} {1} {2} {3}", ExceptionMessage, ex.Source, ex.Message, ex.StackTrace);
                    }
                }
            }
            else
            {
                Logger.Warn(FileNotExistMessage);
            }
        }

        /// <summary>
        /// Set <c>fileName</c>.
        /// </summary>
        /// <param name="fileName">The name of the file to be opened.</param>
        private static void SetFileName(string fileName)
        {
            _fileName = fileName;
        }

        //Get the highest used ID.
        public int GetMaxId()
        {
            if (StoredMovies.Any())
            {
                var maxId = StoredMovies.Max(movie => movie.GetId());
                return maxId;
            }
            else
            {
                return 0;
            }

        }

        // Get the movie with matching ID.
        public Movie FindMovieById(int id)
        {
            var movie = StoredMovies.Find(m => m.GetId() == id);
            if (movie == null)
            {
                throw new KeyNotFoundException(MovieNotFoundMessage);
            }
            return movie;
        }

        public bool FindMovieByTitle(string title, out Movie movie)
        {
            if (title != null)
            {
                if (title == "") throw new ArgumentException("Argument string cannot be empty");
            }
            else
                throw new ArgumentNullException();

            foreach (var storedMovie in StoredMovies)
            {
                if (!title.ToUpper().Equals(storedMovie.GetTitle().ToUpper())) continue;
                movie = storedMovie;
                return true;
            }

            movie = null;
            return false;
        }
    }
}