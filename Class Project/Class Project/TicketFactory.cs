using Conversion_Class_Project;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Class_Project
{
    /// <summary>
    /// The <c>TicketFactory</c> class.
    /// This class follows a thread-safe singlton pattern.
    /// To access <c>TicketFactory</c>, call <c>GetTicketFactory(int id)</c>.
    /// </summary>
    sealed class TicketFactory
    {
        private static readonly TicketFactory _instance = new TicketFactory();
        private static int lastId;
        private static readonly int _ticketIdFloor = 1;

        private TicketFactory()
        {

        }
        
        /// <summary>
        /// Get the <c>TicketFactory</c> instance.
        /// When called, the last used <c>ticketId</c> must be passed for new <c>Tickets</c> to be generated without conflicting IDs.
        /// </summary>
        /// <param name="id">THe last used ID.</param>
        /// <returns>THe <c>TicketFactory</c> instance.</returns>
        public static TicketFactory GetTicketFactory(int id)
        {
            SetLastId(id);
            return _instance;
        }


        /// <summary>
        /// Set the last used ID.
        /// If the current value of <c>lastId</c> is greater than the argument passed, the argument passed is ignored.
        /// </summary>
        /// <param name="id">The last ID used to generate a ticket.</param>
        private static void SetLastId(int id)
        {
            if (lastId < id)
            {
                lastId = id;
            }
        }

        /// <summary>
        /// Generate a previously created ticket that already has a <c>ticketId</c>.
        /// </summary>
        /// <param name="ticketId">The <c>ticketId</c> of the ticket.</param>
        /// <param name="summary">The <c>summary</c> of the ticket.</param>
        /// <param name="status">The <c>status</c> of the ticket.</param>
        /// <param name="priority">The <c>priority</c> of the ticket.</param>
        /// <param name="submitter">The <c>submitter</c> of the ticket.</param>
        /// <param name="assigned">The <c>assigned</c> of the ticket.</param>
        /// <param name="watching">The <c>watching</c> of the ticket.</param>
        /// <returns>the <c>Ticket</c> object.</returns>
        /// <exception cref="System.ArgumentException">Thrown when the <c>ticketId</c> is less than the <c>ticketIdFloor</c>.</exception>
        public static Ticket NewTicket(int ticketId, string summary, Status status, Priority priority, string submitter, string assigned, List<string> watching)
        {
            Ticket ticket = new Ticket(ticketId, summary, status, priority, submitter, assigned, watching);

            if (ticket.GetTicketId() < _ticketIdFloor)
            {
                throw new System.ArgumentException("Invalid ticket ID", "ticketId");
            }
            return ticket;
        }

        /// <summary>
        /// Generate a new ticket with a <c>watching</c> argument.
        /// </summary>
        /// <param name="summary">The <c>summary</c> of the ticket.</param>
        /// <param name="status">The <c>status</c> of the ticket.</param>
        /// <param name="priority">The <c>priority</c> of the ticket.</param>
        /// <param name="submitter">The <c>submitter</c> of the ticket.</param>
        /// <param name="assigned">The <c>assigned</c> of the ticket.</param>
        /// <param name="watching">The <c>watching</c> of the ticket.</param>
        /// <returns>the <c>Ticket</c> object.</returns>
        /// <exception cref="System.ArgumentException">Thrown when the <c>ticketId</c> is less than the <c>ticketIdFloor</c>.</exception>
        public static Ticket NewTicket(string summary, Status status, Priority priority, string submitter, string assigned, List<string> watching)
        {
            Ticket ticket = new Ticket(++lastId, summary, status, priority, submitter, assigned, watching);

            if (ticket.GetTicketId() < 1)
            {
                throw new System.ArgumentException("Invalid ticket ID", "ticketId");
            }
            return ticket;
        }

        /// <summary>
        /// Generate a new ticket without a <c>watching</c> argument.
        /// The <c>watching</c> property is populated with the submitter.
        /// </summary>
        /// <param name="summary">The <c>summary</c> of the ticket.</param>
        /// <param name="status">The <c>status</c> of the ticket.</param>
        /// <param name="priority">The <c>priority</c> of the ticket.</param>
        /// <param name="submitter">The <c>submitter</c> of the ticket.</param>
        /// <param name="assigned">The <c>assigned</c> of the ticket.</param>
        /// <returns>the <c>Ticket</c> object.</returns>
        /// <exception cref="System.ArgumentException">Thrown when the <c>ticketId</c> is less than the <c>ticketIdFloor</c>.</exception>
        public static Ticket NewTicket(string summary, Status status, Priority priority, string submitter, string assigned)
        {
            List<string> watching = new List<string>();
            watching.Add(submitter);

            Ticket ticket = new Ticket(++lastId, summary, status, priority, submitter, assigned, watching);

            if (ticket.GetTicketId() < 1)
            {
                throw new System.ArgumentException("Invalid ticket ID", "ticketId");
            }
            return ticket;
        }

        /// <summary>
        /// Parses a formatted <c>string</c> into a <c>Ticket</c> object.
        /// </summary>
        /// <param name="ticketString">THe formatted <c>string</c> to be parsed.</param>
        /// <param name="regex">The regular expression needed to parse the formatted <c></c>string</c>, as a <c>string</c>.</param>
        /// <returns>A <c>Ticket</c> object, parsed from a formatted <c>string</c>.</returns>
        public static Ticket StringToTicket(string ticketString, string regex)
        {
            string[] subs = Regex.Split(ticketString, regex);

            List<string> watching = new List<string>(subs[6].Split('|'));

            Ticket ticket = new Ticket(Conversion.StringToInt(subs[0]), subs[1], Conversion.StringToStatus(subs[2]), Conversion.StringToPriority(subs[3]), subs[4], subs[5], watching);

            return ticket;
        }
    }
}