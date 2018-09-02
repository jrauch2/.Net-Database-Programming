using System.Collections.Generic;
using Database_Functions;
using Conversion_Class_Project;
using System.Linq;

namespace Class_Project
{
    /// <summary>
    /// 
    /// </summary>
    class DbOut : IOutput
    {
        private TicketingContext db = new TicketingContext();

        /// <summary>
        /// Write all tickets to the database.
        /// </summary>
        /// <param name="tickets">List of <c>Tickets</c></param>
        public void WriteAll(List<Ticket> tickets)
        {
            int id = 0;
            foreach (Ticket ticket in tickets)
            {
                id = ticket.GetTicketId();
                if (db.Tickets.Any(t => t.TicketId == id))
                {
                    continue;
                }
                else
                {
                    TicketEntity ticketEntity = Conversion.ToTicketEntity(ticket);
                    db.Tickets.Add(ticketEntity);
                }
            }
            db.SaveChanges();
        }   
    }
}