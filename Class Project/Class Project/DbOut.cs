using System.Collections.Generic;
using Database_Functions;
using Conversion_Class_Project;

namespace Class_Project
{
    /// <summary>
    /// 
    /// </summary>
    class DbOut : IOutput
    {
        /// <summary>
        /// Write all tickets to the database.
        /// </summary>
        /// <param name="tickets">List of <c>Tickets</c></param>
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