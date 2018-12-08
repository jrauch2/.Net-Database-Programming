using System.Linq;
using Support_Ticket_System.Interfaces;
using Support_Ticket_System.Utility;

namespace Support_Ticket_System
{
    public partial class Ticket
    {
        public void Print(IDisplay display)
        {
            display.WriteLine("Ticket ID: " + TicketID);
            display.WriteLine("Status: " + Status + "     Priority: " + Priority);
            display.WriteLine("Summary: " + Summary);
            if (TicketType.Description.Equals("Bug"))
            {
                var severity = BugAttributes?.Select(ba => ba.Severity).FirstOrDefault();
                display.WriteLine("Severity: Tier " + (int) severity.ToSeverity());
            }
            if (TicketType.Description.Equals("Enhancement"))
            {
                display.WriteLine("Software: " +
                                   EnhancementAttributes?.Select(ea => ea.Software).FirstOrDefault());
                display.WriteLine("Reason: " +
                                   EnhancementAttributes?.Select(ea => ea.Reason).FirstOrDefault());
                display.WriteLine("Estimate: " +
                                   EnhancementAttributes?.Select(ea => ea.Estimate).FirstOrDefault());
                display.WriteLine("Cost: " + EnhancementAttributes?.Select(ea => ea.Cost)
                                       .FirstOrDefault());
            }
            if (TicketType.Description.Equals("Task"))
            {
                display.WriteLine("Project Name: " +
                                   TaskAttributes?.Select(ta => ta.ProjectName).FirstOrDefault());
                display.WriteLine("Due Date: " +
                                   TaskAttributes?.Select(ta => ta.DueDate).FirstOrDefault());
            }
            display.WriteLine("Submitter: " + SubmitterUser);
            display.WriteLine("Assigned: " + AssignedUser);
            display.WriteLine("Watching Users:");
            display.WriteLine(WatchingUsers.ToFormattedString());
            display.WriteSpecialLine();
        }
    }
}
