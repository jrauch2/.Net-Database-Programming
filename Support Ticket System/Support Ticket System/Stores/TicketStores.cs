using System;
using System.Collections;
using System.Collections.Generic;
using Support_Ticket_System.Interfaces;
using Support_Ticket_System.Tickets;

namespace Support_Ticket_System.Stores
{
    internal class TicketStores : IStore, IEnumerable
    {
        private readonly List<IStore> _stores = new List<IStore>();
private const string TicketExistsMessage = "Ticket already exists";

        public List<Ticket> GetAllTickets()
        {
            var allTickets = new List<Ticket>();
            foreach (var store in _stores)
            {
                allTickets.AddRange(store.GetAllTickets());
            }

            return allTickets;
        }

        public List<Ticket> Search(string searchString)
        {
            var tickets = new List<Ticket>();
            foreach (var store in _stores)
            {
                tickets.AddRange(store.Search(searchString));
            }

            return tickets;
        }

        public List<Ticket> Search()
        {
            throw new NotImplementedException();
        }

        //Get the highest used ID.
        public int GetMaxId()
        {
            var maxId = 0;
            foreach (var store in _stores)
            {
                var id = store.GetMaxId();
                if (id > maxId)
                {
                    maxId = id;
                }
            }

            return maxId;
        }

        public bool FindId(int id, out Ticket t)
        {
            foreach (var store in _stores)
            {
                return store.FindId(id, out t);
            }

            t = null;
            return false;
        }

        public void AddTicket(Ticket ticket)
        {
            foreach (var store in _stores)
            {
                if (ticket.GetType() != ((ITicketable)store).TicketType) continue;
                if (store.FindId(ticket.Id, out _))
                {
                    throw new ArgumentException(TicketExistsMessage, nameof(ticket));
                }
                else
                {
                    store.AddTicket(ticket);
                }
            }
        }
        // TODO fix this
        public User GetUserByName(string fName, string lName)
        {
            foreach (var store in _stores)
            {
                return store.GetUserByName(fName, lName);
            }

            return null;
        }

        public void AddTicketStore(IStore store)
        {
            _stores.Add(store);
        }

        public IEnumerator GetEnumerator()
        {
            return _stores.GetEnumerator();
        }

    }
}
