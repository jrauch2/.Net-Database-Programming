using System;
using System.Collections.Generic;
using NLog;
using System.Globalization;

namespace Support_Ticket_System
{
    internal class SupportTicketSystem
    {
        private readonly Logger _logger;                // Logger for the SupportTicketSystem class.
        private readonly TicketStores _stores;          // TicketStore instance.
        private readonly TicketFactory _ticketFactory;  // TicketFactory instance.
        private IDisplay _display;                      // Display for the application.

        // Set of strings used throughout the SupportTicketSystem class.
        private const string PaddedFiftyStarLine = " ************************************************** \n";
        private const string Header = "Support Desk Ticket System";
        private const string NewSupportTicketHeader = "New Support Ticket";
        private const string NewEnhancementTicketHeader = "New Enhancement Ticket";
        private const string NewTaskTicketHeader = "New Task Ticket";
        private const string PressToContinue = "Press any key to continue...\n";
        private const string InvalidInput = "Invalid input. Try again.\n";
        
        public SupportTicketSystem(IDisplay display)
        {
            const string regex = ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)";   // Regular Expression for selecting commas that are not between double quotes.
            _logger = LogManager.GetCurrentClassLogger();               // Instantiate _logger.
            
            _logger.Trace("Setting the display and display size...");
            _display = display;
            _display.SetWindowSize(52, 30);
            _display.LeftPadding = " * ";
            _display.RightPadding = "* ";
            _logger.Trace("... Display set.");
            _logger.Trace("Instantiating TicketStore...");
            _stores = new TicketStores();
            _logger.Trace("...TicketStore instantiated.");
            _logger.Trace("Getting TicketFactory instance...");
            _ticketFactory = TicketFactory.GetTicketFactoryInstance();  // TicketFactory is a Singleton, must retrieve the instance from the class.
            _logger.Trace("... TicketFactory instance retreived.");
            
            // Support Ticket Store
            _logger.Trace("Adding CsvSupportTicketStore to the TicketStore...");
            const string supportFilePath = "..\\..\\support_tickets.csv";
            _stores.AddTicketStore(new CsvSupportTicketStore(supportFilePath, ref _display, regex));
            _logger.Trace("... CsvSupportTicketStore added to TicketStore.");

            // Enhancement Ticket Store
            _logger.Trace("Adding CsvEnhancementTicketStore to the TicketStore...");
            const string enhancementFilePath = "..\\..\\enhancement_tickets.csv";
            _stores.AddTicketStore(new CsvEnhancementTicketStore(enhancementFilePath, ref _display, regex));
            _logger.Trace("... CsvEnhancementTicketStore added to TicketStore.");

            // Task Ticket Store
            _logger.Trace("Adding CsvTaskTicketStore to the TicketStore...");
            const string taskFilePath = "..\\..\\task_tickets.csv";
            _stores.AddTicketStore(new CsvTaskTicketStore(taskFilePath, ref _display, regex));
            _logger.Trace("... CsvTaskTicketStore added to TicketStore.");
        }

        public void Start()
        {
            Menu();
        }

        private void Menu()
        {
            var correct = false;

            do
            {
                _display.Write(PaddedFiftyStarLine);
                _display.WriteLine(Header);
                _display.Write(PaddedFiftyStarLine);
                _display.WriteLine("1) New Ticket (customer)");
                _display.WriteLine("2) New Ticket (technician)");
                _display.WriteLine("3) Update SupportTicket");
                _display.WriteLine("4) Print All Tickets");
                _display.WriteLine("5) Exit");
                _display.Write(PaddedFiftyStarLine);
                _display.Write("Select an option: ");

                var input = _display.GetInput();
                _display.Clear();
                switch (input)
                {
                    case "1":
                        NewTicketViaCustomer();
                        break;
                    case "2":
                        NewTicketViaTechnician();
                        break;
                    case "3":
                        UpdateTicket();
                        break;
                    case "4":
                        PrintAllTickets();
                        break;
                    case "5":
                        correct = true;
                        CloseProgram();
                        break;
                    default:
                        _display.Write(InvalidInput);
                        break;
                }
            } while (!correct);

        }

        //Ask the user for input to generate a new ticket.
        private void NewTicketViaCustomer()
        {
            var ticketType = GetTicketTypeInput();

            var summary = GetSummaryInput();
            _display.Clear();

            var priority = GetPriorityInput();
            _display.Clear();

            var submitter = GetSubmitterInput();
            _display.Clear();

            GetCorrectInput(summary, priority, submitter);
        }

        private void NewTicketViaTechnician()
        {
            string summary = GetSummaryInput();
            _display.Clear();

            Priority priority = GetPriorityInput();
            _display.Clear();

            string submitter = GetSubmitterInput();
            _display.Clear();

            string assigned = GetAssignedInput();
            _display.Clear();

            string watching = GetWatchingInput();
            _display.Clear();

            Severity severity = GetSeverityInput();

            GetCorrectInput(summary, priority, submitter, assigned, watching, severity);

        }

        private void UpdateTicket()
        {
            //TODO
        }

        private void PrintAllTickets()
        {
            _display.Write(PaddedFiftyStarLine);
            _display.WriteLine(Header);
            _display.Write(PaddedFiftyStarLine);
            foreach (var ticket in _stores.GetAllTickets())
            {
                ticket.DisplayTicket();
                _display.Write(PaddedFiftyStarLine);
            }
            _display.Write(PressToContinue);
            _display.GetInput();
            _display.Clear();
        }

        private TicketType GetTicketTypeInput()
        {
            var correct = false;
            var ticketType = TicketType.Support;
            do
            {
                _display.Write(PaddedFiftyStarLine);
                _display.WriteLine(Header);
                _display.WriteLine(NewSupportTicketHeader);
                _display.Write(PaddedFiftyStarLine);
                _display.WriteLine("What do type of service do you require?");
                foreach (var name in Enum.GetNames(typeof(TicketType)))
                {
                    _display.WriteLine(name);
                }
                _display.Write(PaddedFiftyStarLine);
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
                    foreach (var name in Enum.GetNames(typeof(TicketType)))
                    {
                        if (!priorityInput.Equals(name)) continue;
                        ticketType = priorityInput.ToTicketType();
                        correct = true;
                    }
                }
                _display.Clear();
            } while (!correct);

            return ticketType;
        }

        //Ask user for summary.
        private string GetSummaryInput()
        {
            _display.Write(PaddedFiftyStarLine);
            _display.WriteLine(Header);
            _display.WriteLine(NewSupportTicketHeader);
            _display.Write(PaddedFiftyStarLine);
            _display.WriteLine("Write a summary of the issue (254 Char Max):");
            _display.Write(PaddedFiftyStarLine);
            return _display.GetInput();
        }

        //Ask user for Priority.
        private Priority GetPriorityInput()
        {
            var correct = false;
            var priority = Priority.Low;
            do
            {
                _display.Write(PaddedFiftyStarLine);
                _display.WriteLine(Header);
                _display.WriteLine(NewSupportTicketHeader);
                _display.Write(PaddedFiftyStarLine);
                _display.WriteLine("Set a priority (Low, Medium, High, Severe):");
                _display.Write(PaddedFiftyStarLine);
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
            _display.Write(PaddedFiftyStarLine);
            _display.WriteLine(Header);
            _display.WriteLine(NewSupportTicketHeader);
            _display.Write(PaddedFiftyStarLine);
            _display.WriteLine("Enter submitter name:");
            _display.Write(PaddedFiftyStarLine);
            return _display.GetInput();
        }

        private string GetAssignedInput()
        {
            _display.Write(PaddedFiftyStarLine);
            _display.WriteLine(Header);
            _display.WriteLine(NewSupportTicketHeader);
            _display.Write(PaddedFiftyStarLine);
            _display.WriteLine("Enter assigned technician:");
            _display.Write(PaddedFiftyStarLine);
            return _display.GetInput();
        }

        private string GetWatchingInput()
        {
            _display.Write(PaddedFiftyStarLine);
            _display.WriteLine(Header);
            _display.WriteLine(NewSupportTicketHeader);
            _display.Write(PaddedFiftyStarLine);
            _display.WriteLine("Enter watching names, comma separated:");
            _display.Write(PaddedFiftyStarLine);
            return _display.GetInput();
        }

        private Severity GetSeverityInput()
        {
            //TODO
            return Severity.Tier1;
        }
        
        //Ask the user to confirm that the information is correct
        private void GetCorrectInput(string summary, Priority priority, string submitter)
        {
            var correct = false;

            do
            {
                _display.Write(PaddedFiftyStarLine);
                _display.WriteLine(Header);
                _display.WriteLine(NewSupportTicketHeader);
                _display.Write(PaddedFiftyStarLine);
                _display.WriteLine("1) Summary: " + summary);
                _display.WriteLine("2) Priority: " + priority);
                _display.WriteLine("3) Submitter: " + submitter);
                _display.Write(PaddedFiftyStarLine);
                _display.Write("Would you like to make a change?\n(enter 0 to accept): ");
                var input = _display.GetInput();

                switch (input)
                {
                    case "0":
                        correct = true;
                        var id = _stores.GetMaxId();
                        //Save ticket
                        _stores.AddTicket(_ticketFactory.NewTicket(
                            ++id, summary, Status.Open, priority, submitter, "", new List<string>() {submitter}, Severity.Tier1, ref _display));
                        break;
                    case "1":
                        _display.Clear();
                        summary = GetSummaryInput();
                        break;
                    case "2":
                        _display.Clear();
                        priority = GetPriorityInput();
                        break;
                    case "3":
                        _display.Clear();
                        submitter = GetSubmitterInput();
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

        //Ask the user to confirm that the information is correct
        private void GetCorrectInput(string summary, Priority priority, string submitter, string assigned, string watching, Severity severity)
        {
            var correct = false;

            do
            {
                _display.Write(PaddedFiftyStarLine);
                _display.WriteLine(Header);
                _display.WriteLine(NewSupportTicketHeader);
                _display.Write(PaddedFiftyStarLine);
                _display.WriteLine("1) Summary: " + summary);
                _display.WriteLine("2) Priority: " + priority);
                _display.WriteLine("3) Submitter: " + submitter);
                _display.WriteLine("4) Assigned: " + assigned);
                _display.WriteLine("1) Watching: " + watching);
                _display.Write(PaddedFiftyStarLine);
                _display.Write("Would you like to make a change?\n(enter 0 to accept): ");
                var input = _display.GetInput();

                switch (input)
                {
                    case "0":
                        correct = true;
                        var id = _stores.GetMaxId();
                        //Save ticket
                        _stores.AddTicket(_ticketFactory.NewTicket(
                            ++id, summary, Status.Open, priority, submitter, assigned, watching.ToStringList(), severity, ref _display));
                        break;
                    case "1":
                        _display.Clear();
                        summary = GetSummaryInput();
                        break;
                    case "2":
                        _display.Clear();
                        priority = GetPriorityInput();
                        break;
                    case "3":
                        _display.Clear();
                        submitter = GetSubmitterInput();
                        break;
                    case "4":
                        _display.Clear();
                        assigned = GetAssignedInput();
                        break;
                    case "5":
                        _display.Clear();
                        watching = GetWatchingInput();
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