using Class_Project;
using Database_Functions;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Conversion_Class_Project
{
    class Conversion
    {
        public static TicketEntity ToTicketEntity(Ticket ticket)
        {
            TicketEntity ticketEntity = new TicketEntity
            {
                TicketId = ticket.GetTicketId(),
                Summary = ticket.GetSummary(),
                Status = ticket.GetStatus().ToString(),
                Priority = ticket.GetPriority().ToString(),
                Submitter = ticket.GetSubmitter(),
                Assigned = ticket.GetAssigned(),
                Watching = ticket.GetWatchingString()
            };

            return ticketEntity;
        }

        public static Ticket StringToTicket(string ticketString, string regex)
        {
            string[] subs = Regex.Split(ticketString, regex);

            List<string> watching = new List<string>(subs[6].Split('|'));

            Ticket ticket = new Ticket(StringToInt(subs[0]), subs[1], StringToStatus(subs[2]), StringToPriority(subs[3]), subs[4], subs[5], watching);

            return ticket;
        }

        public static int StringToInt(string s)
        {
            int i;

            if (!int.TryParse(s, out i))
            {
                //TODO
            }

            return i;
        }

        public static Status StringToStatus(string statusString)
        {
            Status status = Status.OPEN;
            foreach (Status s in Enum.GetValues(typeof(Status)))
            {
                if (statusString.Equals(s.ToString()))
                {
                    status = s;
                }
            }

            return status;
        }

        public static Priority StringToPriority(string priorityString)
        {
            Priority priority = Priority.LOW;
            foreach (Priority p in Enum.GetValues(typeof(Priority)))
            {
                if (priorityString.Equals(p.ToString()))
                {
                    priority = p;
                }
            }

            return priority;
        }
    }
}