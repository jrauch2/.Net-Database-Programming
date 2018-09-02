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
        
        Ticket ticket0 = dbIn.FindId(42);
        Console.WriteLine(ticket0.ToString());

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
        }
    }
}
