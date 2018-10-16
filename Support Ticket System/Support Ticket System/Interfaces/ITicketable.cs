using System;

namespace Support_Ticket_System.Interfaces
{
    interface ITicketable
    {
        Type TicketType { get; set; }
    }
}
