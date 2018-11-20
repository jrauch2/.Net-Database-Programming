using System;
using System.Collections.Generic;
using NLog;
using System.Globalization;
using Support_Ticket_System.Enums;
using Support_Ticket_System.Interfaces;
using Support_Ticket_System.Stores;
using Support_Ticket_System.Tickets;
using Support_Ticket_System.Utility;

namespace Support_Ticket_System
{
    internal class SupportTicketSystem
    {
        private readonly Logger _logger;                // Logger for the SupportTicketSystem class.
        private IDisplay _display;                      // Display for the application.
        
        // Set of strings used throughout the SupportTicketSystem class.
        private const char SpecialCharacter = '*';
        private const string Header = "Support Desk Ticket System";
        private const string NewBugTicketHeader = "New Bug Ticket";
        private const string NewEnhancementTicketHeader = "New Enhancement Ticket";
        private const string NewTaskTicketHeader = "New Task Ticket";
        private const string PressToContinue = "Press any key to continue...\n";
        private const string InvalidInput = "Invalid input. Try again.\n";
        
        public SupportTicketSystem(IDisplay display)
        {
            _logger = LogManager.GetCurrentClassLogger();               // Instantiate _logger.
            
            _logger.Debug("Setting the display and display size...");
            _display = display;
            _display.SetWindowSize(52, 30);
            _display.SpecialCharacter = SpecialCharacter;
            _logger.Trace("... Display set.");
            
        }

        public void Start()
        {
            Menu();
        }

        private void Menu()
        {
            do
            {
                _logger.Trace("Print Menu Header...");
                _display.WriteSpecialLine();
                _display.WriteLine(Header);
                _display.WriteSpecialLine();                
                _display.WriteLine("1) New Ticket");
                _display.WriteLine("2) Update Ticket");
                _display.WriteLine("3) Print All Tickets");
                _display.WriteLine("4) Search Tickets");
                _display.WriteLine("5) Exit");
                _display.WriteSpecialLine();
                _display.Write("Select an option: ");

                var input = _display.GetInput();

                switch (input)
                {
                    case "1":
                        NewTicket();
                        break;              
                    case "2":
                        UpdateTicket();
                        break;
                    case "3":
                        PrintAllTickets();
                        break;
                    case "4":
                        SearchAllTickets();
                        break;
                    case "5":
                        CloseProgram();
                        break;
                    default:
                        _display.Write(InvalidInput);
                        break;
                }
            } while (true);

        }

        private void NewTicket()
        {
            _display.Clear();
            _display.WriteSpecialLine();
            _display.WriteLine(Header);
            _display.WriteSpecialLine();
            _display.WriteLine("1) New Bug Ticket");
            _display.WriteLine("2) New Enhancement Ticket");
            _display.WriteLine("3) New Task Ticket");

            var input = _display.GetInput();

            switch (input)
            {
                case "1":
                    NewBugTicket();
                    break;
                case "2":
                    NewEnhancementTicket();
                    break;
                case "3":
                    NewTaskTicket();
                    break;                
                default:
                    _display.Write(InvalidInput);
                    break;
            }
        }

        private void SearchAllTickets()
        {
            _display.Write("Enter search term: ");
            var searchTerm = _display.GetInput();

            //TODO
            
        }

        //Ask the user for input to generate a new ticket.
        private void NewBugTicket()
        {
            _display.Clear();
            string summary = GetSummaryInput();
            _display.Clear();

            Priority priority = GetPriorityInput();
            _display.Clear();

            string submitter = GetSubmitterInput();
            _display.Clear();

            string assigned = GetAssignedInput();
            _display.Clear();

            string watching = GetWatchingInput(submitter, assigned);
            _display.Clear();

            Severity severity = GetSeverityInput();

            GetCorrectInput(summary, priority, submitter, assigned, watching, severity);
        }

        private void NewEnhancementTicket()
        {
            string summary = GetSummaryInput();
            _display.Clear();

            Priority priority = GetPriorityInput();
            _display.Clear();

            string submitter = GetSubmitterInput();
            _display.Clear();

            string assigned = GetAssignedInput();
            _display.Clear();

            string watching = GetWatchingInput(submitter, assigned);
            _display.Clear();

            string software = GetSoftwareInput();

            string cost = GetCostInput();

            string reason = GetReasonInput();

            string estimate = GetEstimateInput();

            GetCorrectInput(summary, priority, submitter, assigned, watching, cost, reason, estimate);

        }

        private string GetEstimateInput()
        {
            throw new NotImplementedException();
        }

        private string GetReasonInput()
        {
            throw new NotImplementedException();
        }

        private string GetCostInput()
        {
            throw new NotImplementedException();
        }

        private void GetCorrectInput(string summary, Priority priority, string submitterString, string assignedString, string watchingString, string severity, string reason, string estimate)
        {
            throw new NotImplementedException();
        }

        private string GetSoftwareInput()
        {
            throw new NotImplementedException();
        }

        private void NewTaskTicket()
        {
            string summary = GetSummaryInput();
            _display.Clear();

            Priority priority = GetPriorityInput();
            _display.Clear();

            string submitter = GetSubmitterInput();
            _display.Clear();

            string assigned = GetAssignedInput();
            _display.Clear();

            string watching = GetWatchingInput(submitter, assigned);
            _display.Clear();

            string projectName = GetProjectNameInput();

            string dueDate = GetDueDateInput();

            GetCorrectInput(summary, priority, submitter, assigned, watching, projectName, dueDate);
        }

        private string GetDueDateInput()
        {
            throw new NotImplementedException();
        }

        private string GetProjectNameInput()
        {
            throw new NotImplementedException();
        }

        private void GetCorrectInput(string summary, Priority priority, string submitterString, string assignedString, string watchingString, string projectName, string dueDate)
        {
            throw new NotImplementedException();
        }

        private void UpdateTicket()
        {
            //TODO
        }

        private void PrintAllTickets()
        {
            _display.WriteSpecialLine();
            _display.WriteLine(Header);
            _display.WriteSpecialLine();
            var list = _store.GetAllTickets();
            foreach (var ticket in list)
            {
                ticket.DisplayTicket();
                _display.WriteSpecialLine();
            }
            _display.Write(PressToContinue);
            _display.GetInput();
            _display.Clear();
        }

        //Ask user for summary.
        private string GetSummaryInput()
        {
            _display.WriteSpecialLine();
            _display.WriteLine(Header);
            _display.WriteLine(NewBugTicketHeader);
            _display.WriteSpecialLine();
            _display.WriteLine("Write a summary of the issue (254 Char Max):");
            _display.WriteSpecialLine();
            return _display.GetInput();
        }

        //Ask user for summary.
        private string GetSummaryInput(string summary)
        {
            _display.WriteSpecialLine();
            _display.WriteLine(Header);
            _display.WriteLine(NewBugTicketHeader);
            _display.WriteSpecialLine();
            _display.WriteLine("Write a summary of the issue (254 Char Max):");
            _display.WriteSpecialLine();
            _display.SendWait(summary);
            return _display.GetInput();
        }

        //Ask user for Priority.
        private Priority GetPriorityInput()
        {
            var correct = false;
            var priority = Priority.Low;
            do
            {
                _display.WriteSpecialLine();
                _display.WriteLine(Header);
                _display.WriteLine(NewBugTicketHeader);
                _display.WriteSpecialLine();
                _display.WriteLine("Set a priority (Low, Medium, High, Severe):");
                _display.WriteSpecialLine();
                var input = _display.GetInput();
                if (input is null)
                {
                    _display.Write(InvalidInput);
                    _display.Write(PressToContinue);
                    _display.GetInput();
                }
                else
                {
                    var priorityInput = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input);
                    if (priorityInput.Equals(Priority.Low.ToString()) ||
                        priorityInput.Equals(Priority.Medium.ToString()) ||
                        priorityInput.Equals(Priority.High.ToString()) ||
                        priorityInput.Equals(Priority.Severe.ToString()))
                    {
                        priority = priorityInput.ToPriority();
                        correct = true;
                    }
                }
                _display.Clear();
            } while (!correct);

            return priority;
        }

        //Ask user for name (submitter).
        private string GetSubmitterInput()
        {
            _display.WriteSpecialLine();
            _display.WriteLine(Header);
            _display.WriteLine(NewBugTicketHeader);
            _display.WriteSpecialLine();
            _display.WriteLine("Enter submitter name (First Last):");
            _display.WriteSpecialLine();
            return _display.GetInput();
        }

        //Ask user for name (submitter).
        private string GetSubmitterInput(string submitter)
        {
            _display.WriteSpecialLine();
            _display.WriteLine(Header);
            _display.WriteLine(NewBugTicketHeader);
            _display.WriteSpecialLine();
            _display.WriteLine("Enter submitter name (First Last):");
            _display.WriteSpecialLine();
            _display.SendWait(submitter);
            return _display.GetInput();
        }
        
        private string GetAssignedInput()
        {
            _display.WriteSpecialLine();
            _display.WriteLine(Header);
            _display.WriteLine(NewBugTicketHeader);
            _display.WriteSpecialLine();
            _display.WriteLine("Enter assigned technician (First Last):");
            _display.WriteSpecialLine();
            return _display.GetInput();
        }

        private string GetAssignedInput(string assigned)
        {
            _display.WriteSpecialLine();
            _display.WriteLine(Header);
            _display.WriteLine(NewBugTicketHeader);
            _display.WriteSpecialLine();
            _display.WriteLine("Enter assigned technician (First Last):");
            _display.WriteSpecialLine();
            _display.SendWait(assigned);
            return _display.GetInput();
        }

        private string GetWatchingInput(string submitter, string assigned)
        {
            var done = false;
            var input = "";
            do
            {
                _display.WriteSpecialLine();
                _display.WriteLine(Header);
                _display.WriteLine(NewBugTicketHeader);
                _display.WriteSpecialLine();
                _display.WriteLine("Enter watching names, comma separated(First Last, First Last):");
                _display.WriteSpecialLine();
                _display.SendWait(submitter + ", " + assigned);
                input = _display.GetInput();
                if (input.Trim().Equals(""))
                {
                    _display.Write(InvalidInput);
                    _display.Write(PressToContinue);
                    _display.GetInput();
                }
                else
                {
                    done = true;
                }
            } while (!done);

            return input;
        }

        private Severity GetSeverityInput()
        {
            var correct = false;
            var severity = Severity.Tier1;
            do
            {
                _display.WriteSpecialLine();
                _display.WriteLine(Header);
                _display.WriteLine(NewBugTicketHeader);
                _display.WriteSpecialLine();
                _display.WriteLine("Set a severity of support (Tier 1, Tier 2, Tier 3):");
                _display.WriteSpecialLine();
                _display.SendWait("Tier ");

                var input = _display.GetInput();
                if (input is null || input == "")
                {
                    _display.Write(InvalidInput);
                    _display.Write(PressToContinue);
                    _display.GetInput();
                }
                else
                {
                    var severityInput = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input.Replace(" ", ""));
                    if (severityInput.Equals(Severity.Tier1.ToString()) ||
                        severityInput.Equals(Severity.Tier2.ToString()) ||
                        severityInput.Equals(Severity.Tier3.ToString()))
                    {
                        severity = severityInput.ToSeverity();
                        correct = true;
                    }
                }
                _display.Clear();
            } while (!correct);

            return severity;
        }
        
        //Ask the user to confirm that the information is correct
        private void GetCorrectInput(string summary, Priority priority, string submitterString, string assignedString, string watchingString, Severity severity)
        {
            var correct = false;

            do
            {
                _display.WriteSpecialLine();
                _display.WriteLine(Header);
                _display.WriteLine(NewBugTicketHeader);
                _display.WriteSpecialLine();
                _display.WriteLine("1) Summary: " + summary);
                _display.WriteLine("2) Priority: " + priority);
                _display.WriteLine("3) Submitter: " + submitterString);
                _display.WriteLine("4) Assigned: " + assignedString);
                _display.WriteLine("5) Watching: " + watchingString);
                _display.WriteLine("6) Severity: " + severity);
                _display.WriteSpecialLine();
                _display.Write("Would you like to make a change?\n(enter 0 to accept): ");
                var input = _display.GetInput();

                switch (input)
                {
                    case "0":
                        correct = true;

                        Bug newBugTicket = new Bug();
                        
                        var submitterName = submitterString.Trim().Split(' ');
                        var submitter = _store.GetUserByName(submitterName[0], submitterName[1]);
                        
                        var assignedName = assignedString.Trim().Split(' ');
                        var assigned = _store.GetUserByName(assignedName[0], assignedName[1]);
                        
                        var watching = new List<User>();            
                        var nameString = watchingString.Split(',');
                        foreach (var name in nameString)
                        {
                            var n = name.Trim().Split(' ');
                            var user = _store.GetUserByName(n[0], n[1]);
                        }

                        //Save ticket
                        _store.AddTicket(newBugTicket);
                        break;
                    case "1":
                        _display.Clear();
                        summary = GetSummaryInput(summary);
                        break;
                    case "2":
                        _display.Clear();
                        priority = GetPriorityInput();
                        break;
                    case "3":
                        _display.Clear();
                        submitterString = GetSubmitterInput(submitterString);
                        break;
                    case "4":
                        _display.Clear();
                        assignedString = GetAssignedInput(assignedString);
                        break;
                    case "5":
                        _display.Clear();
                        watchingString = GetWatchingInput(submitterString, assignedString);
                        break;
                    default:
                        _display.Write(InvalidInput);
                        _display.Write(PressToContinue);
                        _display.GetInput();
                        break;
                }

                _display.Clear();
            } while (!correct);
        }

        private void CloseProgram()
        {
            Environment.Exit(0);
        }
    }
}