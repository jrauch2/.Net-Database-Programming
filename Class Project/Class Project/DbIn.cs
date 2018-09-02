using System.Collections.Generic;
using Database_Functions;
using System.Linq;

namespace Class_Project
{
    class DbIn : IInput
    {
        TicketingContext db = new TicketingContext();

        public Ticket findId(int id)
        {
            throw new System.NotImplementedException();
        }

        public int GetMaxID()
        {
            throw new System.NotImplementedException();
        }

        public List<Ticket> GetStoredTickets()
        {
            List<Ticket> storedTickets = new List<Ticket>();
            using (db)
            {
                var query = from t in db.Tickets
                            orderby t.TicketId
                            select t;
                foreach (Ticket ticketEntity in query)
                {

                    storedTickets.Add(ticket);
                }
            }
        }
    }
}