using System;
using Database_Functions;

namespace Class_Project
{
    /// <summary>
    /// The <c>Conversion</c> class.
    /// Contains all methods for parsing stored data into a <c>Ticket</c> object.
    /// </summary>
    internal class Conversion
    {
        /// <summary>
        /// Converts a <c>Ticket</c> to a <c>TicketEntity</c> to be stored in a database.
        /// </summary>
        /// <param name="ticket">The <c>Ticket</c> to be stored.</param>
        /// <returns>The store-able <c>TicketEntity</c></returns>
        public static TicketEntity ToTicketEntity(Ticket ticket)
        {
            var ticketEntity = new TicketEntity
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
            int.TryParse(s, out var i);
            return i;
        }

        /// <summary>
        /// Parse a <c>string</c> to a <c>Status</c>.
        /// </summary>
        /// <param name="statusString">The <c>string</c> to be parsed.</param>
        /// <returns>A <c>Status</c>.</returns>
        public static Status StringToStatus(string statusString)
        {
            return (Status)Enum.Parse(typeof(Status), statusString.ToUpper());
        }

        /// <summary>
        /// Parse a <c>string</c> to a <c>Priority</c>.
        /// </summary>
        /// <param name="priorityString">The <c>string</c> to be parsed.</param>
        /// <returns>A <c>Priority</c>.</returns>
        public static Priority StringToPriority(string priorityString)
        {
            return (Priority)Enum.Parse(typeof(Priority), priorityString.ToUpper());
        }
    }
}