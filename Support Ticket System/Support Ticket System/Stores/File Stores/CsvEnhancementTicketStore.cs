﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NLog;
using Support_Ticket_System.Interfaces;
using Support_Ticket_System.Tickets;
using Support_Ticket_System.Utility;

namespace Support_Ticket_System.Stores.File_Stores
{
    internal class CsvEnhancementTicketStore : IStore, ITicketable
    {

        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private string FilePath { get; }
        private IDisplay _display;
        private string RegexString { get; }
//        private readonly TicketFactory _ticketFactory;
        public Type TicketType { get; set; }


        private const string TicketNotFoundMessage = "Ticket not found.";
        private const string TicketExistsMessage = "Ticket already exists";
        private const string WrongTypeMessage = "Not an Enhancement ticket. Check type before calling method.";

        public CsvEnhancementTicketStore(string filePath, ref IDisplay display, string regexString)
        {
            TicketType = typeof(Enhancement);
            _display = display;
//            _ticketFactory = TicketFactory.GetTicketFactoryInstance();
            FilePath = filePath;
            RegexString = regexString;
        }

        //Get all stored Tickets
        
        public List<Ticket> GetAllTickets()
        {
            var tickets = new List<Ticket>();
            if (!File.Exists(FilePath))
            {
                _logger.Debug("File not found.");
                _display.WriteLine($"File {FilePath} does not exist, would you like to create it? (Y/N): ");
                var input = _display.GetInput();
                if (!input.Equals("Y") && !input.Equals("y")) return tickets;
                _logger.Trace("Generating new file...");
                WriteToFile("TicketId,Summary,Status,Priority,Submitter,Assigned,Watching,Software,Cost,Reason,Estimate");
                _logger.Debug("New file generated.");
                return tickets;
            }
            using (var file = new StreamReader(FilePath))
            {
                try
                {
                    while (!file.EndOfStream)
                    {
                        var line = file.ReadLine();
                        if (line == null) continue;
                        if (!int.TryParse(line[0].ToString(), out _)) continue;
//                        tickets.Add(StringToTicket(line));
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                }
            }

            return tickets;
        }

        public List<Ticket> Search(string searchString)
        {
            throw new NotImplementedException();
        }

        public List<Ticket> Search()
        {
            throw new NotImplementedException();
        }

        //Get the highest used ID.
        public int GetMaxId()
        {
            var tickets = GetAllTickets();
            if (!tickets.Any()) return 0;
            var maxId = tickets.Max(ticket => ticket.Id);
            return maxId;
        }

        // Get the ticket with matching id
        public bool FindId(int id, out Ticket t)
        {
            var tickets = GetAllTickets();
            var ticket = tickets.Find(ti => ti.Id == id);
            if (ticket != null)
            {
                t = ticket;
                return true;
            }

            t = null;
            _logger.Info(TicketNotFoundMessage);
            return false;
        }

        // Add ticket to the CSV file
        public void AddTicket(Ticket ticket)
        {
            var tickets = GetAllTickets();
            if (ticket is null)
            {
                throw new ArgumentNullException();
            }

            if (tickets.Contains(ticket) || FindId(ticket.Id, out _))
            {
                throw new ArgumentException(TicketExistsMessage, nameof(ticket));
            }

            if (ticket is Enhancement)
            {
                tickets.Add(ticket);
                WriteToFile(ticket.ToString());
            }
            else
            {
                _logger.Error(WrongTypeMessage);
            }
        }

        public User GetUserByName(string fName, string lName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Parses a formatted <c>string</c> into an <c>Enhancement</c> ticket object.
        /// </summary>
        /// <param name="ticketString">The formatted <c>string</c> to be parsed.</param>
        /// <returns>An <c>Enhancement</c> ticket object, parsed from a formatted <c>string</c>.</returns>
//        private Enhancement StringToTicket(string ticketString)
//        {
//            var subs = Regex.Split(ticketString, RegexString);
//
//            var watching = new List<string>(subs[6].Split('|'));
//
//            for (var i = 0; i < subs.Length; i++)
//            {
//                subs[i] = subs[i].Replace("\"", "");
//            }
//
////            var ticket = _ticketFactory.NewTicket(subs[0].ToInt(), subs[1], subs[2].ToStatus(), subs[3].ToPriority(), subs[4], subs[5], watching, subs[7], subs[8].ToDouble(), subs[9], subs[10], ref _display);
//
//            return ticket;
//        }

        private void WriteToFile(string s)
        {
            if (File.Exists(FilePath))
            {
                File.Copy(FilePath, FilePath + ".bak", true);
            }

            using (var output = new StreamWriter(FilePath, true))
            {
                output.WriteLine(s);
            }
        }
    }
}