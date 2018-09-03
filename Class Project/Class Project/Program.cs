using System;
using System.Collections.Generic;


namespace Class_Project
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            IInput csvIn = new CsvIn("support_tickets.csv");
            IOutput dbOut = new DbOut();
            IInput dbIn = new DbIn();

            List<Ticket> tickets = csvIn.GetStoredTickets();
            dbOut.WriteAll(tickets);

            Console.WriteLine("CSV");
            foreach (var ticket in tickets)
            {
                Console.WriteLine(ticket.ToString());
            }

            List<Ticket> dbTickets = dbIn.GetStoredTickets();

            Console.WriteLine("DB");
            foreach (var ticket in dbTickets)
            {
                Console.WriteLine(ticket.ToString());
            }

//            Test ToString on null
            Console.WriteLine(dbIn.FindId(42));


            Console.WriteLine(csvIn.GetMaxId());

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
