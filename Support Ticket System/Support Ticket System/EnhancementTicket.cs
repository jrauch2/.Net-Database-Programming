using System;
using System.Collections.Generic;

namespace Support_Ticket_System
{
    class EnhancementTicket : Ticket
    {
        public string Software { get; set; }
        public double Cost { get; set; }
        public string Reason { get; set; }
        public string Estimate { get; set; }

        public EnhancementTicket(int ticketId, string summary, Status status, Priority priority, string submitter, string assigned, List<string> watching, string software, double cost, string reason, string estimate, ref IDisplay displayProgram)
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
