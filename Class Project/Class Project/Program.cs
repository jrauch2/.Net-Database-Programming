using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_Project
{
    class Program
    {
        static void Main(string[] args)
        {
            IInput csvIn = new CSVIn("support_tickets.csv");
            IOutput dbOut = new DbOut();
            IInput dbIn = new DbIn();

            List<Ticket> tickets = csvIn.GetStoredTickets();
            dbOut.WriteAll(tickets);

            Console.WriteLine("CSV");
            foreach (Ticket ticket in tickets)
            {
                Console.WriteLine(ticket.ToString());
            }

            List<Ticket> dbTickets = dbIn.GetStoredTickets();

            Console.WriteLine("DB");
            foreach (Ticket ticket in dbTickets)
            {
                Console.WriteLine(ticket.ToString());
            }

            //Test ToString on null
            Console.WriteLine(dbIn.FindId(42));


            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
