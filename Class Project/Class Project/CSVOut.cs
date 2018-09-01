using System.Collections.Generic;
using System.IO;

namespace Class_Project
{
    class CSVOut : IOutput
    {
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