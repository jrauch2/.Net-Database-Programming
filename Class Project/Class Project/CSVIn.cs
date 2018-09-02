using System;
using System.Collections.Generic;
using System.IO;

namespace Class_Project
{
    /// <summary>
    /// The <c>CSVIn</c> class.
    /// Implements the <c>IInput</c> interface.
    /// Used to load stored tickets from a CSV file.
    /// </summary>
    class CSVIn : IInput
    {
        private List<Ticket> storedTickets = new List<Ticket>();
        private string regex = ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)";
        private string fileName;

        /// <summary>
        /// Constructor for <c>CSVIn</c>.
        /// Requires the name of the file to be opened as an argument.
        /// </summary>
        /// <param name="fileName">The name of the file to be opened.</param>
        public CSVIn(string fileName)
        {
            SetFileName(fileName);
            StreamReader file = new StreamReader(fileName);

            while (!file.EndOfStream)
            {
                string line = file.ReadLine();
                storedTickets.Add(TicketFactory.StringToTicket(line, regex));
            }
        }


        /// <summary>
        /// Set <c>fileName</c>.
        /// </summary>
        /// <param name="fileName">The name of the file to be opened.</param>
        private void SetFileName(string fileName)
        {
            this.fileName = fileName;
        }

        /// <summary>
        /// Get a <c>List<Ticket></c> of stored tickets.
        /// </summary>
        /// <returns>A <c>List<Ticket></c></returns>
        public List<Ticket> GetStoredTickets()
        {
            return storedTickets;
        }
                
        /// <summary>
        /// Get the highest ID stored.
        /// </summary>
        /// <returns><c>int</c> of the highest ID stored.</returns>
        public int GetMaxId()
        {
            int maxId = 0;
            foreach (Ticket ticket in storedTickets)
            {
                int ticketId = ticket.GetTicketId();
                if (ticketId > maxId)
                {
                    maxId = ticketId;
                }
            }
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