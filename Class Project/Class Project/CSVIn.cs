using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Class_Project
{
    /// <inheritdoc cref="IInput" />
    /// <summary>
    /// The <c>CSVIn</c> class.
    /// Implements the <c>IInput</c> interface.
    /// Used to load stored tickets from a CSV file.
    /// </summary>
    internal class CsvIn : CsvTickets, IInput
    {
        private const string Regex = ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)";
        private string _fileName;
        private const string TicketNotFoundMessage = "Ticket not found.";
        private const string ExceptionMessage = "There was an Exception in ";

        /// <summary>
        /// Constructor for <c>CSVIn</c>.
        /// Requires the name of the file to be opened as an argument.
        /// </summary>
        /// <param name="fileName">The name of the file to be opened.</param>
        public CsvIn(string fileName)
        {
            SetFileName(fileName);
            using (var file = new StreamReader(_fileName))
            {
                try
                {
                    while (!file.EndOfStream)
                    {
                        string line = file.ReadLine();
                        StoredTickets.Add(TicketFactory.StringToTicket(line, Regex));
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
        
        /// <summary>
        /// Set <c>fileName</c>.
        /// </summary>
        /// <param name="fileName">The name of the file to be opened.</param>
        private void SetFileName(string fileName)
        {
            this._fileName = fileName;
        }

        //Get a List of all stored tickets.
        /// <inheritdoc />
        public List<Ticket> GetStoredTickets()
        {
            return StoredTickets;
        }

        //Get the highest used ID.
        /// <inheritdoc />
        public int GetMaxId()
        {
            int maxId = StoredTickets.Max(ticket => ticket.GetTicketId());
            return maxId;
        }

        // Get the ticket with matching id
        /// <inheritdoc />
        public Ticket FindId(int id)
        {
           Ticket ticket = StoredTickets.Find(t => t.GetTicketId() == id);
            if (ticket == null)
            {
                //TODO
                //Make generic
                Console.WriteLine(TicketNotFoundMessage);
            }
            return ticket;
        }
    }
}