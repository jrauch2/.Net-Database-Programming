using System;
using System.Collections.Generic;
using Support_Ticket_System.Enums;
using Support_Ticket_System.Interfaces;

namespace Support_Ticket_System.Tickets
{
    internal abstract class Ticket
    {
        public int Id { get; set; }
        public string Summary { get; set; }
        public Status Status { get; set; }
        public Priority Priority { get; set; }
        public User Submitter { get; set; }
        public User Assigned { get; set; }
        public List<User> Watching { get; set; }
        public Type TicketType { get; set; }
        public IDisplay Display { get; set; }

        public abstract void DisplayTicket();


        public void AddWatching(User watcher)
        {
            Watching.Add(watcher);
        }
    }
}
