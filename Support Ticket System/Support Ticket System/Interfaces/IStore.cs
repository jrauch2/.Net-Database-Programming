using System.Collections.Generic;
using Support_Ticket_System.Tickets;

namespace Support_Ticket_System.Interfaces
{
    internal interface IStore
    {
        List<Ticket> GetAllTickets();
        List<Ticket> Search();
        int GetMaxId();
        bool FindId(int id, out Ticket t);
        void AddTicket(Ticket ticket);
    }
}
