using Class_Project;
using Database_Functions;
using System;

namespace Conversion_Class_Project
{
    /// <summary>
    /// The <c>Conversion</c> class.
    /// Contains all methods for parsing stored data into a <c>Ticket</c> object.
    /// </summary>
    class Conversion
    {
        /// <summary>
        /// Converts a <c>Ticket</c> to a <c>TicketEntity</c> to be stored in a database.
        /// </summary>
        /// <param name="ticket">The <c>Ticket</c> to be stored.</param>
        /// <returns>The storable <c>TicketEntity</c></returns>
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

        /// <summary>
        /// Parse a <c>string</c> to an <c>int</c>. 
        /// </summary>
        /// <param name="s">The <c>string</c> to be parsed into and <c>int</c>.</param>
        /// <returns>An <c>int</c> parsed from a <c>string</c>.</returns>
        public static int StringToInt(string s)
        {
            int i;
            int.TryParse(s, out i);
            return i;
        }

        /// <summary>
        /// Parse a <c>string</c> to a <c>Status</c>.
        /// </summary>
        /// <param name="statusString">The <c>string</c> to be parsed.</param>
        /// <returns>A <c>Status</c>.</returns>
        /// <exception cref="System.ArgumentException">Thrown when the <c>statusString</c> argument cannot be parsed into a Status.</exception>
        public static Status StringToStatus(string statusString)
        {
            Status status = Status.OPEN;
            foreach (Status s in Enum.GetValues(typeof(Status)))
            {
                if (!statusString.Equals(s.ToString()))
                {
                    throw new System.ArgumentException("string argument does not match a Status.", "statusString");
                }
                status = s;
            }

            return status;
        }

        /// <summary>
        /// Parse a <c>string</c> to a <c>Priority</c>.
        /// </summary>
        /// <param name="priorityString">The <c>string</c> to be parsed.</param>
        /// <returns>A <c>Priority</c>.</returns>
        /// <exception cref="System.ArgumentException">Thrown when the <c>priorityString</c> argument cannot be parsed into a Priority.</exception>
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