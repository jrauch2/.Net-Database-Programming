using System.Collections.Generic;
using System.IO;

namespace Class_Project
{
    /// <summary>
    /// The <c>CSVOut</c> class.
    /// Implements the <c>IOutput</c> interface.
    /// Used to store tickets in a CSV file.
    /// </summary>
    class CSVOut : IOutput
    {
        /// <summary>
        /// Write all <c>Ticket</c> objects to the CSV file.
        /// </summary>
        /// <param name="tickets">List of all <c>Tickets</c></param>
        public void WriteAll(List<Ticket> tickets)
        {
            StreamWriter file = new StreamWriter("file.csv");
            foreach (Ticket ticket in tickets)
            {
                file.WriteLine(ticket.ToString());
            }
            file.Close();
        }
    }
}