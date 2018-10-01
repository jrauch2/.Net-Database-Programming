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

        public EnhancementTicket(int ticketId, string summary, Status status, Priority priority, string submitter, string assigned, List<string> watching, string software, double cost, string reason, string estimate, ref IDisplay display)
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
            Display = display;
        }

        public override void DisplayTicket()
        {

            Display.Write(WordWrap.Wrap(Id + "\n" +
                                                 Summary + "\n" +
                                                 Status + "\n" +
                                                 Priority + "\n" +
                                                 Submitter + "\n" +
                                                 Assigned + "\n" +
                                                 Watching.ToFormattedString() + "\n" +
                                                 Software + "\n" +
                                                 Cost + "\n" +
                                                 Reason + "\n" +
                                                 Estimate + "\n",
                Display.DisplayWidth));
        }
    }
}
