using System.Collections.Generic;

namespace Class_Project
{
    interface IInput
    {
        List<Ticket> GetStoredTickets();
        int GetMaxID();
    }
}