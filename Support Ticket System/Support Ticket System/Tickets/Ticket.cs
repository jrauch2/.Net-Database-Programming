using System.Collections.Generic;
using Support_Ticket_System.Enums;
using Support_Ticket_System.Interfaces;

namespace Support_Ticket_System.Tickets
{
    internal abstract class Ticket
    {
        public int Id { get; protected set; }
        protected string Summary { get; set; }
        protected Status Status { get; set; }
        protected Priority Priority { get; set; }
        protected User Submitter { get; set; }
        protected User Assigned { get; set; }
        protected List<User> Watching { get; set; }
        protected IDisplay DisplayProgram { get; set; }

        public abstract void DisplayTicket();


        public void AddWatching(User watcher)
        {
            Watching.Add(watcher);
        }
    }
}
