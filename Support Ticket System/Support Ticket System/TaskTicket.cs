using System;
using System.Collections.Generic;

namespace Support_Ticket_System
{
    internal class TaskTicket : Ticket
    {
        private string ProjectName { get; set; }
        private DateTime DueDate { get; set; }

        public TaskTicket(int id, string summary, Status status, Priority priority, string submitter, string assigned, List<string> watching, string projectName, DateTime dueDate, ref IDisplay displayProgram)
        {
            Id = id;
            Summary = summary;
            Status = status;
            Priority = priority;
            Submitter = submitter;
            Assigned = assigned;
            Watching = watching;
            ProjectName = projectName;
            DueDate = dueDate;
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
            DisplayProgram.WriteLine("Project Name: " + ProjectName);
            DisplayProgram.WriteLine("Due Date: " + DueDate);
        }
    }
}
