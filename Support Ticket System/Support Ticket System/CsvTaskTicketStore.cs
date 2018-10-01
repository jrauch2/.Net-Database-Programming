﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NLog;

namespace Support_Ticket_System
{
    internal class CsvTaskTicketStore : IStore
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private string FilePath { get; }
        private string RegexString { get; }
        private readonly TicketFactory _ticketFactory;
        private IDisplay _display;

        private const string TicketNotFoundMessage = "Ticket not found.";
        private const string MovieExistsMessage = "Movie not added, {0} already exists";

        public CsvTaskTicketStore(string filePath, ref IDisplay display, string regexString)
        {
            _display = display;
            _ticketFactory = TicketFactory.GetTicketFactoryInstance();
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
                WriteToFile("TicketId,Summary,Status,Priority,Submitter,Assigned,Watching,ProjectName,DueDate");
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
                        tickets.Add(StringToTicket(line));
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                }
            }

            return tickets;
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

            if (tickets.Contains(ticket))
            {
                throw new ArgumentException(MovieExistsMessage, nameof(ticket));
            }

            if (FindId(ticket.Id, out _))
            {
                throw new ArgumentException(MovieExistsMessage, nameof(ticket.Id));
            }

            WriteToFile(ticket.ToString());
        }

        /// <summary>
        /// Parses a formatted <c>string</c> into a <c>SupportTicket</c> object.
        /// </summary>
        /// <param name="ticketString">The formatted <c>string</c> to be parsed.</param>
        /// <returns>A <c>SupportTicket</c> object, parsed from a formatted <c>string</c>.</returns>
        private SupportTicket StringToTicket(string ticketString)
        {
            var subs = Regex.Split(ticketString, RegexString);

            var watching = new List<string>(subs[6].Split('|'));

            for (var i = 0; i < subs.Length; i++)
            {
                subs[i] = subs[i].Replace("\"", "");
            }

            var ticket = _ticketFactory.NewTicket(subs[0].ToInt(), subs[1], subs[2].ToStatus(), subs[3].ToPriority(), subs[4], subs[5], watching, subs[7].ToSeverity(), ref _display);

            return ticket;
        }

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