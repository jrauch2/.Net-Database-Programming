using System.Collections.Generic;
using Database_Functions;
using Conversion_Class_Project;

namespace Class_Project
{
    class DbOut : IOutput
    {
        public void WriteAll(List<Ticket> tickets)
        {
            using (var db = new TicketingContext())
            {
                foreach (Ticket ticket in tickets)
                {
                    TicketEntity ticketEntity = Conversion.ToTicketEntity(ticket);
                    db.Tickets.Add(ticketEntity);
                }
                db.SaveChanges();
            }
        }

        
    }
}