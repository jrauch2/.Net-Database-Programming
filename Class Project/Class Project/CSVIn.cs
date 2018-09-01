using System;
using System.Collections.Generic;
using System.IO;
using Conversion_Class_Project;

namespace Class_Project
{
    class CSVIn : IInput
    {
        private List<Ticket> storedTickets = new List<Ticket>();
        private string rx = ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)";
        private string fileName;

        public CSVIn(string fileName)
        {
            SetFileName(fileName);
            StreamReader file = new StreamReader(fileName);

            while (!file.EndOfStream)
            {
                string line = file.ReadLine();
                storedTickets.Add(Conversion.StringToTicket(line, rx));
            }
        }

        private void SetFileName(string fileName)
        {
            this.fileName = fileName;
        }

        public List<Ticket> GetStoredTickets()
        {
            return storedTickets;
        }
                
        public int GetMaxID()
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
    }
}