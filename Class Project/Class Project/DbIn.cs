using System.Collections.Generic;
using Database_Functions;
using System.Linq;
using System;

namespace Class_Project
{
    /// <summary>
    /// The <c>DbIn</c> class.
    /// Implements the <c>IInput</c> interface.
    /// Used to read records from a database.
    /// </summary>
    class DbIn : IInput
    {
        TicketingContext db = new TicketingContext();
        private string regex = ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)";

        /// <summary>
        /// Find ticket by Id.
        /// </summary>
        /// <param name="id">The ID of the ticket to be found</param>
        /// <returns><c>Ticket</c></returns>
        public Ticket FindId(int id)
        {
            Ticket ticket = null;
            try
            {
                TicketEntity ticketEntity = db.Tickets.Find(id);
                ticket = TicketFactory.StringToTicket(ticketEntity.ToString(), regex);
            }
            catch (NullReferenceException)
            {
                //make this generic
                Console.WriteLine("Ticket not found");
            }
            return ticket;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetMaxID()
        {
            int maxId = -1;
            using (db)
            {
                var query = from t in db.Tickets
                            select t;
                if (query.Any())
                {
                    maxId = db.Tickets.Max(t => t.TicketId);
                }
            }
            return maxId;
        }

        /// <summary>
        /// Get a list of all tickets stored in the database.
        /// </summary>
        /// <returns><c>List<Ticket></c></returns>
        public List<Ticket> GetStoredTickets()
        {
            List<Ticket> storedTickets = new List<Ticket>();
            using (db)
            {
                var query = from t in db.Tickets
                            orderby t.TicketId
                            select t;
                foreach (TicketEntity ticketEntity in query)
                {
                    Ticket ticket = TicketFactory.StringToTicket(ticketEntity.ToString(), regex);
                    storedTickets.Add(ticket);
                }
            }
            return storedTickets;
        }
    }
}