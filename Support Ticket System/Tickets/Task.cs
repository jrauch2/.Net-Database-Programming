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

        public override void DisplayTicket()
        {
            Display.WriteLine("ID: " + Id);
            Display.WriteLine("Summary: " + Summary);
            Display.WriteLine("Status: " + Status);
            Display.WriteLine("Priority: " + Priority);
            Display.WriteLine("Submitter: " + Submitter);
            Display.WriteLine("Assigned: " + Assigned);
            Display.WriteLine("Watching: " + Watching.ToFormattedString());
            Display.WriteLine("Project Name: " + ProjectName);
            Display.WriteLine("Due Date: " + DueDate);
        }
    }
}
