using System.Collections.Generic;
using Database_Functions;
using System.Linq;
using System;

namespace Class_Project
{
    /// <inheritdoc />
    /// <summary>
    /// The <c>DbIn</c> class.
    /// Implements the <c>IInput</c> interface.
    /// Used to read records from a database.
    /// </summary>
    internal class DbIn : IInput
    {
        TicketingContext db;
        private const string Regex = ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)";

        public Ticket FindId(int id)
        {
            Ticket ticket = null;
            try
            {
                using (db = new TicketingContext())
                {
                    TicketEntity ticketEntity = db.Tickets.Find(id);
                    ticket = TicketFactory.StringToTicket(ticketEntity.ToString(), Regex);
                }
            }
            catch (NullReferenceException)
            {
                //TODO
                //make this generic
                Console.WriteLine("Ticket not found");
            }
            return ticket;
        }

        /// <summary>
        /// Get the max ID stored.
        /// </summary>
        /// <returns><c>int</c> of maxId</returns>
        public int GetMaxId()
        {
            int maxId = -1;
            using (db = new TicketingContext())
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
            using (db = new TicketingContext())
            {
                var query = from t in db.Tickets
                            orderby t.TicketId
                            select t;
                foreach (TicketEntity ticketEntity in query)
                {
                    Ticket ticket = TicketFactory.StringToTicket(ticketEntity.ToString(), Regex);
                    storedTickets.Add(ticket);
                }
            }
            return storedTickets;
        }
    }
}