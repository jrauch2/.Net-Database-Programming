using System.Collections.Generic;

namespace Support_Ticket_System
{
    abstract class Ticket
    {
        public int Id { get; protected set; }
        protected string Summary { get; set; }
        protected Status Status { get; set; }
        protected Priority Priority { get; set; }
        protected string Submitter { get; set; }
        protected string Assigned { get; set; }
        protected List<string> Watching { get; set; }
        protected IDisplay DisplayProgram { get; set; }

        public abstract void DisplayTicket();
    }
}
