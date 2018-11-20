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

        public override void DisplayTicket()
        {

            Display.WriteLine("ID: " + Id);
            Display.WriteLine("Summary: " + Summary);
            Display.WriteLine("Status: " + Status);
            Display.WriteLine("Priority: " + Priority);
            Display.WriteLine("Submitter: " + Submitter);
            Display.WriteLine("Assigned: " + Assigned);
            Display.WriteLine("Watching: " + Watching.ToFormattedString());
            Display.WriteLine("Software: " + Software);
            Display.WriteLine("Cost: " + Cost);
            Display.WriteLine("Reason: " + Reason);
            Display.WriteLine("Estimate: " + Estimate);
        }
    }
}
