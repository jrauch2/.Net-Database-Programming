using System.Collections.Generic;
using Support_Ticket_System.Enums;
using Support_Ticket_System.Interfaces;
using Support_Ticket_System.Utility;

namespace Support_Ticket_System.Tickets
{
    internal class Task : Ticket
    {
        public string ProjectName { get; set; }
        public string DueDate { get; set; }

        public Task(int id, string summary, Status status, Priority priority, User submitter, User assigned, List<User> watching, string projectName, string dueDate, ref IDisplay displayProgram)
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
            TicketType = typeof(Task);
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
