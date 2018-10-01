using System.Collections.Generic;

namespace Support_Ticket_System
{
    internal interface IStore
    {
        List<Ticket> GetAllTickets();
        int GetMaxId();
        bool FindId(int id, out Ticket t);
        void AddTicket(Ticket ticket);
    }
}
