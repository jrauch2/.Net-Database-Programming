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
            ArrayList watching = new ArrayList();
            watching.Add("this");
            watching.Add("that");
            watching.Add("another");

            Ticket ticket = new Ticket("1", "none", Status.OPEN, Priority.LOW, "me", "you", watching);

            Console.WriteLine(ticket.ToString());
            Console.ReadKey();
        }
    }
}
