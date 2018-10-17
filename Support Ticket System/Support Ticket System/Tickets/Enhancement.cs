using System.Collections.Generic;
using Support_Ticket_System.Enums;
using Support_Ticket_System.Interfaces;
using Support_Ticket_System.Utility;

namespace Support_Ticket_System.Tickets
{
    internal class Enhancement : Ticket
    {
        public string Software { get; set; }
        public string Cost { get; set; }
        public string Reason { get; set; }
        public string Estimate { get; set; }

        public Enhancement(int ticketId, string summary, Status status, Priority priority, User submitter, User assigned, List<User> watching, string software, string cost, string reason, string estimate, ref IDisplay displayProgram)
        {
            Id = ticketId;
            Summary = summary;
            Status = status;
            Priority = priority;
            Submitter = submitter;
            Assigned = assigned;
            Watching = watching;
            Software = software;
            Cost = cost;
            Reason = reason;
            Estimate = estimate;
            DisplayProgram = displayProgram;
            TicketType = typeof(Enhancement);
        }

        public override void DisplayTicket()
        {

            DisplayProgram.WriteLine("ID: " + Id);
            DisplayProgram.WriteLine("Summary: " + Summary);
            DisplayProgram.WriteLine("Status: " + Status);
            DisplayProgram.WriteLine("Priority: " + Priority);
            DisplayProgram.WriteLine("Submitter: " + Submitter);
            DisplayProgram.WriteLine("Assigned: " + Assigned);
            DisplayProgram.WriteLine("Watching: " + Watching.ToFormattedString());
            DisplayProgram.WriteLine("Software: " + Software);
            DisplayProgram.WriteLine("Cost: " + Cost);
            DisplayProgram.WriteLine("Reason: " + Reason);
            DisplayProgram.WriteLine("Estimate: " + Estimate);
        }
    }
}
