using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Support_Ticket_System
{
    interface ITicketable
    {
        Type TicketType { get; set; }
    }
}
