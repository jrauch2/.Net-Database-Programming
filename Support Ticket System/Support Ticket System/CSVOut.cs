using System;
using System.Collections.Generic;
using System.IO;
using Support_Ticket_System;

namespace Class_Project
{
    /// <inheritdoc cref="IOutput" />
    /// <summary>
    /// The <c>CSVOut</c> class.
    /// Extends the <c>CsvTickets</c> class.
    /// Implements the <c>IOutput</c> interface.
    /// Used to store tickets in a CSV file.
    /// </summary>
    /// <see cref="CsvTickets"/>
    /// <seealso cref="IOutput"/>
    internal class CsvOut : CsvTickets, IOutput
    {
        private string _fileName;
        private const string ExceptionMessage = "There was an Exception in ";

        /// <summary>
        /// Output <c>List</c> of <c>Ticket</c> objects to a csv file.
        /// </summary>
        /// <param name="fileName">The name of the file to be written to.</param>
        public CsvOut(string fileName)
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

        /// <inheritdoc />
        /// <summary>
        /// Write all <c>Ticket</c> objects to the CSV file.
        /// </summary>
        /// <param name="tickets">List of all active <c>Ticket</c> objects to be added.</param>
        public void WriteAll(List<Ticket> tickets)
        {
            UpdateStoredTickets(tickets);
            
            using (var csv = new StreamWriter(_fileName))
            {
                try
                {
                    foreach (var ticket in StoredTickets)
                    {
                        csv.WriteLine(ticket.ToString());
                    }
                }
                catch (Exception ex)
                {
                    //TODO
                    //Make generic
                    Console.WriteLine(ExceptionMessage + nameof(WriteAll));
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}