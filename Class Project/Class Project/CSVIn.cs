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

        /// <summary>
        /// Constructor for <c>CSVIn</c>.
        /// Requires the name of the file to be opened as an argument.
        /// </summary>
        /// <param name="fileName">The name of the file to be opened.</param>
        public CsvIn(string fileName)
        {
            SetFileName(fileName);
            using (var file = new StreamReader(fileName))
            {
                try
                {
                    while (!file.EndOfStream)
                    {
                        string line = file.ReadLine();
                        Console.WriteLine(line);
                        StoredTickets.Add(TicketFactory.StringToTicket(line, Regex));
                    }
                }
                catch (Exception)
                {
                    //TODO
                    Console.WriteLine("There was an Exception in CsvIn constructor.");
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
        public List<Ticket> GetStoredTickets()
        {
            return StoredTickets;
        }

        //Get the highest used ID.
        public int GetMaxId()
        {
            int maxId = StoredTickets.Max(ticket => ticket.GetTicketId());
            return maxId;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Ticket FindId(int id)
        {
            //TODO
            throw new NotImplementedException();
        }
    }
}