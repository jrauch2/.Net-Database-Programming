using System.Collections.Generic;

namespace Class_Project
{
    /// <summary>
    /// The abstract <c>CsvTickets</c> class.
    /// Contains a <c>List</c> of stored tickets in <c>StoredTickets</c>.
    /// </summary>
    internal abstract class CsvTickets
    {
        protected static List<Ticket> StoredTickets = new List<Ticket>();

        /// <summary>
        /// Takes a <c>List</c> of <c>Ticket</c> objects and adds them to the <c>StoredTickets</c> list.
        /// Will replace matching IDs in <c>StoredTickets</c> before appending new <c>Ticket</c> objects.
        /// </summary>
        /// <param name="tickets"></param>
        protected static void UpdateStoredTickets(List<Ticket> tickets)
        {
            for (var i = 0; i < tickets.Count; i++)
            {
                for (var j = 0; j < StoredTickets.Count; j++)
                {
                    if (StoredTickets[j].GetTicketId() != tickets[i].GetTicketId()) continue;
                    StoredTickets[j] = tickets[i];
                    tickets.RemoveAt(i);
                }
            }
            tickets.Sort((t1, t2) => t1.GetTicketId().CompareTo(t2.GetTicketId()));
            foreach (var ticket in tickets)
            {
                StoredTickets.Add(ticket);
            }
        }
    }
}