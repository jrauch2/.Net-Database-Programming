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

            //TicketingContext db = new TicketingContext();

            //ArrayList watching = new ArrayList();
            //watching.Add("this");
            //watching.Add("that");
            //watching.Add("another");

            ////TicketEntity ticketEntity = new TicketEntity { TicketId = "1", Summary = "none", Status = Status.OPEN, Priority = Priority.LOW, Submitter = "me", Assigned = "you", Watching = watching };

            //TicketEntity ticketEntity = new TicketEntity { TicketId = "1", Summary = "none", Submitter = "me", Assigned = "you"};

            //db.Tickets.Add(ticketEntity);

            //var query = from b in db.Tickets
            //            orderby b.TicketId
            //            select b;

            //Console.WriteLine("All tickets in the database:");
            //foreach (var item in query)
            //{
            //    //Console.WriteLine(item.TicketId + ",\"" + item.Summary + "\"," + item.Status + "," + item.Priority + "," + item.Submitter + "," + item.Assigned + "," + item.Watching);
            //    Console.WriteLine(item.TicketId + ",\"" + item.Summary + "\"," + item.Submitter + "," + item.Assigned + ",");
            //}

            //Console.WriteLine("Press any key to exit...");
            //Console.ReadKey();

            //using (var db = new TicketingContext())
            //{
            //    Create and save a new Ticket
            //    Console.Write("Enter a new ticket ID: ");
            //    var ticketId = Console.ReadLine();

            //    var ticketEntity = new TicketEntity { TicketId = ticketId };
            //    db.Tickets.Add(ticketEntity);
            //    db.SaveChanges();

            //    Display all Tickets from the database
            //    var query = from b in db.Tickets
            //                orderby b.TicketId
            //                select b;

            //    Console.WriteLine("All tickets in the database:");
            //    foreach (var item in query)
            //    {
            //        //Console.WriteLine(item.TicketId);
            //        db.Tickets.Remove(item);
            //    }



            IInput csvIn = new CSVIn("support_tickets.csv");
            List<Ticket> allTickets = csvIn.GetStoredTickets();

            foreach (Ticket ticket in allTickets)
            {
                Console.WriteLine(ticket.ToString());
            }
            

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            //}
        }
    }
}
