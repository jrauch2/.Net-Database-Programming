using System;
using System.Collections.Generic;

namespace Support_Ticket_System
{
    internal class TicketStores : IStore
    {
        private readonly List<IStore> _stores = new List<IStore>();

        public List<Ticket> GetAllTickets()
        {
            var allTickets = new List<Ticket>();
            foreach (var store in _stores)
            {
                allTickets.AddRange(store.GetAllTickets());
            }

            return allTickets;
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
            throw new NotImplementedException();
        }

        public void AddTicketStore(IStore store)
        {
            _stores.Add(store);
        }
    }
}
