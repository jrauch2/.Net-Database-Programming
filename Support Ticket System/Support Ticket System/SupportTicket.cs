using System.Collections.Generic;

namespace Support_Ticket_System
{
    internal class SupportTicket : Ticket
    {
        private Severity Severity { get; }

        public SupportTicket(int id, string summary, Status status, Priority priority, string submitter, string assigned, List<string> watching, Severity severity, ref IDisplay display)
        {
            Id = id;
            Summary = summary;
            Status = status;
            Priority = priority;
            Submitter = submitter;
            Assigned = assigned;
            Watching = watching;
            Display = display;
            Severity = severity;
        }

        public void AppendSummary(string newSummary)
        {
            Summary += "\n" + newSummary;
        }

        public void AddWatching(string watcher)
        {
            Watching.Add(watcher);
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
            Display.WriteLine("Submitter: " + Submitter);
            Display.WriteLine("Assigned: " + Assigned);
            Display.WriteLine("Watching: " + Watching.ToFormattedString());
            Display.WriteLine("Severity: " + Severity);}
    }
}
