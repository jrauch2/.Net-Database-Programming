using System.Collections.Generic;
using Support_Ticket_System.Enums;
using Support_Ticket_System.Interfaces;
using Support_Ticket_System.Utility;

namespace Support_Ticket_System.Tickets
{
    internal class Bug : Ticket
    {
        public Severity Severity { get; set; }

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
            Display.WriteLine("ID: " + Id);
            Display.WriteLine("Summary: " + Summary);
            Display.WriteLine("Status: " + Status);
            Display.WriteLine("Priority: " + Priority);
            Display.WriteLine("Submitter: " + Submitter.FName + " " + Submitter.LName);
            Display.WriteLine("Assigned: " + Assigned.FName + " " + Assigned.LName);
            Display.WriteLine("Watching: " + Watching.ToFormattedString());
            Display.WriteLine("Severity: " + Severity.ToString().Insert(Severity.ToString().Length -1, " "));
        }
    }
}
