using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Support_Ticket_System
{
    internal class SupportTicket : Ticket
    {
        private Severity Severity { get; }

        public SupportTicket(int id, string summary, Status status, Priority priority, string submitter, string assigned, List<string> watching, Severity severity, ref IDisplay displayProgram)
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
