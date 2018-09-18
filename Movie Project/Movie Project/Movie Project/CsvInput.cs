using System;
using System.IO;
using System.Linq;
using NLog;

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
            StartCsvInput();
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
    }
}