using System.Collections.Generic;
using Support_Ticket_System.Enums;
using Support_Ticket_System.Interfaces;
using Support_Ticket_System.Utility;

namespace Support_Ticket_System.Tickets
{
    internal class Bug : Ticket
    {
        public Severity Severity { get; }

        public Bug(int id, string summary, Status status, Priority priority, User submitter, User assigned, List<User> watching, Severity severity, ref IDisplay displayProgram)
        {
            Id = id;
            Summary = summary;
            Status = status;
            Priority = priority;
            Submitter = submitter;
            Assigned = assigned;
            Watching = watching;
            DisplayProgram = displayProgram;
            Severity = severity;
            TicketType = typeof(Bug);
        }

        public void AppendSummary(string newSummary)
        {
            Summary += "\n" + newSummary;
        }
        
        public override string ToString()
        {
            return $"{Id},\"{Summary}\",{Status},{Priority},{Submitter},{Assigned},{Watching.ToDelimitedString('|')},{Severity}";
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
            DisplayProgram.WriteLine("Severity: " + Severity.ToString().Insert(Severity.ToString().Length -1, " "));
        }
    }
}
