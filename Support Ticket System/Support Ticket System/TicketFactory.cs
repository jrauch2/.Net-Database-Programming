using System.Collections.Generic;
using System.Globalization;

namespace Support_Ticket_System
{
    /// <summary>
    /// The <c>TicketFactory</c> class.
    /// </summary>
    internal sealed class TicketFactory
    {
        private static readonly TicketFactory Instance = new TicketFactory();
       
        private TicketFactory()
        {

        }

        /// <summary>
        /// Get the <c>TicketFactory</c> instance.
        /// </summary>
        /// <returns>The <c>TicketFactory</c> instance.</returns>
        public static TicketFactory GetTicketFactoryInstance()
        {
            return Instance;
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
        /// <param name="severity"></param>
        /// <returns>the <c>SupportTicket</c> object.</returns>
        /// <exception cref="System.ArgumentException">Thrown when the <c>ticketId</c> is less than the <c>ticketIdFloor</c>.</exception>
        public SupportTicket NewTicket(int ticketId, string summary, Status status, Priority priority, string submitter,
            string assigned, List<string> watching, Severity severity, ref IDisplay display)
        {
            return new SupportTicket(ticketId, summary, status, priority, submitter, assigned, watching, severity, ref display);
        }

        public EnhancementTicket NewTicket(int ticketId, string summary, Status status, Priority priority, string submitter,
            string assigned, List<string> watching, string software, double cost, string reason, string estimate, ref IDisplay display)
        {
            return new EnhancementTicket(ticketId, summary, status, priority, submitter, assigned, watching, software, cost, reason, estimate, ref display);
        }
    }
}