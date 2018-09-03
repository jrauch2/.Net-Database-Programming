using System.Collections.Generic;
using Database_Functions;
using System.Linq;
using System;

namespace Class_Project
{
    /// <inheritdoc />
    /// <summary>
    /// The <c>DbIn</c> class.
    /// Implements the <c>IInput</c> interface.
    /// Used to read records from a database.
    /// </summary>
    internal class DbIn : IInput
    {
        private const string Regex = ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)";
        private const string TicketNotFoundMessage = "Ticket not found.";
        private const string ExceptionMessage = "There was an Exception in ";

        //Find Ticket by ID.
        public Ticket FindId(int id)
        {
            Ticket ticket = null;
            using (var db = new TicketingContext())
            {
                try
                {
                    TicketEntity ticketEntity = db.Tickets.Find(id);
                    if (ticketEntity != null)
                    {
                        ticket = TicketFactory.StringToTicket(ticketEntity.ToString(), Regex);
                    }
                    else
                    {
                        //TODO
                        //Make generic
                        Console.WriteLine(TicketNotFoundMessage);
                    }
                }
                catch (Exception)
                {
                    //TODO
                }
                return ticket;
            }
        }

        //Get the highest used ID.
        public int GetMaxId()
        {
            using (var db = new TicketingContext())
            {
                var maxId = -1;
                try
                {
                    var query = from t in db.Tickets
                        select t;
                    if (query.Any())
                    {
                        maxId = db.Tickets.Max(t => t.TicketId);
                    }
                }
                catch (Exception ex)
                {
                    //TODO
                    //Make generic
                    Console.WriteLine(ExceptionMessage + nameof(GetMaxId));
                    Console.WriteLine(ex.Message);
                }
                return maxId;
            }
        }

        //Get a List of all stored tickets.
        //CAUTION! This will be expensive with a large database!
        public List<Ticket> GetStoredTickets()
        {
            using (var db = new TicketingContext())
            {
                var list = new List<Ticket>();
                try
                {
                    var query = from t in db.Tickets
                        orderby t.TicketId
                        select t;
                    foreach (TicketEntity ticketEntity in query)
                    {
                        Ticket ticket = TicketFactory.StringToTicket(ticketEntity.ToString(), Regex);
                        list.Add(ticket);
                    }
                }
                catch (Exception ex)
                {
                    //TODO
                    //Make generic
                    Console.WriteLine(ExceptionMessage + nameof(GetStoredTickets));
                    Console.WriteLine(ex.Message);
                }
                return list;
            }
        }
    }
}