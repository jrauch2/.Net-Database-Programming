using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
    internal class CsvInput : CsvStore, IInput
    {
        private static readonly MovieFactory MovieFactory = MovieFactory.GetMovieFactoryInstance();
        private const string Regex = ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)";
        private string _fileName;
        private const string MovieNotFoundMessage = "Movie not found.";
        private const string ExceptionMessage = "There was an Exception in ";

        /// <summary>
        /// Constructor for <c>CsvInput</c>.
        /// Requires the name of the file to be opened as an argument.
        /// </summary>
        /// <param name="fileName">The name of the file to be opened.</param>
        // Constructor for CsvInput.
        // Requires the name of the file to be opened as an argument.
        public CsvInput(string fileName)
        {
            SetFileName(fileName);
            if (File.Exists(fileName))
            {
                using (var file = new StreamReader(_fileName))
                {
                    try
                    {
                        while (!file.EndOfStream)
                        {
                            string line = file.ReadLine();
                            StoredMovies.Add(MovieFactory.StringToTicket(line, Regex));
                        }
                    }
                    catch (Exception ex)
                    {
                        //TODO
                        Console.WriteLine(ExceptionMessage + nameof(CsvIn));
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            else
            {
                //TODO
                Console.WriteLine("File does not exist.");
            }

        }

        /// <summary>
        /// Set <c>fileName</c>.
        /// </summary>
        /// <param name="fileName">The name of the file to be opened.</param>
        private void SetFileName(string fileName)
        {
            _fileName = fileName;
        }

        //Get a List of all stored tickets.
        /// <inheritdoc />
        public List<Ticket> GetStoredTickets()
        {
            return StoredMovies;
        }

        //Get the highest used ID.
        /// <inheritdoc />
        public int GetMaxId()
        {
            if (StoredMovies.Any())
            {
                int maxId = StoredMovies.Max(ticket => ticket.GetTicketId());
                return maxId;
            }
            else
            {
                return 0;
            }

        }

        // Get the ticket with matching id
        /// <inheritdoc />
        public Ticket FindId(int id)
        {
            Ticket ticket = StoredMovies.Find(t => t.GetTicketId() == id);
            if (ticket == null)
            {
                //TODO
                //Make generic
                Console.WriteLine(MovieNotFoundMessage);
            }
            return ticket;
        }

        public List<string> ParseMovieGenres(string genreString)
        {
            var list = new List<string>();
            if (genreString != null)
            {
                var sa = genreString.Split('|');

                list.AddRange(sa);
            }
            list.Sort();

            return list;
        }
    }
}