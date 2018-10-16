using System;
using System.Collections.Generic;
using NLog;
using System.Globalization;
using Support_Ticket_System.Enums;
using Support_Ticket_System.Interfaces;
using Support_Ticket_System.Stores;
using Support_Ticket_System.Stores.DB_Stores;
using Support_Ticket_System.Stores.File_Stores;
using Support_Ticket_System.Tickets;
using Support_Ticket_System.Utility;

namespace Support_Ticket_System
{
    internal class SupportTicketSystem
    {
        private readonly Logger _logger;                // Logger for the SupportTicketSystem class.
        private readonly FileStores _stores;          // TicketStore instance.
//        private readonly TicketFactory _ticketFactory;  // TicketFactory instance.
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
            const string regex = ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)";   // Regular Expression for selecting commas that are not between double quotes.
            _logger = LogManager.GetCurrentClassLogger();               // Instantiate _logger.
            
            _logger.Debug("Setting the display and display size...");
            _display = display;
            _display.SetWindowSize(52, 30);
            _display.SpecialCharacter = SpecialCharacter;
            _logger.Trace("... Display set.");
            _logger.Debug("Instantiating TicketStore...");
            _stores = new FileStores();
            _logger.Trace("...TicketStore instantiated.");
            //            _logger.Debug("Getting TicketFactory instance...");
            //            _ticketFactory = TicketFactory.GetTicketFactoryInstance();  // TicketFactory is a Singleton, must retrieve the instance from the class.
            //            _logger.Trace("... TicketFactory instance retreived.");

            // DB Ticket Store
            _logger.Debug("Adding DbTicketStore to the TicketStore...");
            _stores.AddTicketStore(new DbTicketStore(ref _display));
            _logger.Trace("... CsvBugTicketStore added to TicketStore.");

            // Bug Ticket Store
            _logger.Debug("Adding CsvBugTicketStore to the TicketStore...");
            const string bugFilePath = "..\\..\\support_tickets.csv";
            _stores.AddTicketStore(new CsvBugTicketStore(bugFilePath, ref _display, regex));
            _logger.Trace("... CsvBugTicketStore added to TicketStore.");

            // Enhancement Ticket Store
            _logger.Debug("Adding CsvEnhancementTicketStore to the TicketStore...");
            const string enhancementFilePath = "..\\..\\enhancement_tickets.csv";
            _stores.AddTicketStore(new CsvEnhancementTicketStore(enhancementFilePath, ref _display, regex));
            _logger.Trace("... CsvEnhancementTicketStore added to TicketStore.");

            // Task Ticket Store
            _logger.Debug("Adding CsvTaskTicketStore to the TicketStore...");
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
            do
            {
                _logger.Trace("Print Menu Header...");
                _display.WriteSpecialLine();
                _display.WriteLine(Header);
                _display.WriteSpecialLine();
                var types = new List<Type>();
                _logger.Debug("Get ticket Types from ticket stores...");
                foreach (ITicketable ticketStore in _stores)
                {
                    if (!types.Contains(ticketStore.TicketType))
                        types.Add(ticketStore.TicketType);
                }

                for (var index = 0; index < types.Count; index++)
                {
                    var type = types[index];
                    var i = type.Name.LastIndexOf("T", StringComparison.Ordinal);
                    _display.WriteLine((index + 1) + ") New " + type.Name + " Ticket");
                    if (index != types.Count - 1) continue;
                    _display.WriteLine((index + 2) + ") Update Ticket");
                    _display.WriteLine((index + 3) + ") Print All Tickets");
                    _display.WriteLine((index + 4) + ") Exit");
                }

//                _display.WriteLine("1) New Ticket (customer)");
//                _display.WriteLine("2) New Ticket (technician)");
//                _display.WriteLine("3) Update Ticket");
//                _display.WriteLine("4) Print All Tickets");
//                _display.WriteLine("5) Exit");
//                _display.WriteSpecialLine();
//                _display.Write("Select an option: ");
//
                var input = _display.GetInput();
                _display.Clear();
//                switch (input)
//                {
//                    case "1":
//                        NewTicketViaCustomer();
//                        break;
//                    case "2":
//                        NewTicketViaTechnician();
//                        break;
//                    case "3":
//                        UpdateTicket();
//                        break;
//                    case "4":
//                        PrintAllTickets();
//                        break;
//                    case "5":
//                        correct = true;
//                        CloseProgram();
//                        break;
//                    default:
//                        _display.Write(InvalidInput);
//                        break;
//                }
            } while (true);

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

        private Type GetTicketTypeInput()
        {
            throw new NotImplementedException();
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
            _display.WriteSpecialLine();
            _display.WriteLine(Header);
            _display.WriteSpecialLine();
            foreach (var ticket in _stores.GetAllTickets())
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
            _display.WriteLine("Enter submitter name:");
            _display.WriteSpecialLine();
            return _display.GetInput();
        }

        private string GetAssignedInput()
        {
            _display.WriteSpecialLine();
            _display.WriteLine(Header);
            _display.WriteLine(NewBugTicketHeader);
            _display.WriteSpecialLine();
            _display.WriteLine("Enter assigned technician:");
            _display.WriteSpecialLine();
            return _display.GetInput();
        }

        private string GetWatchingInput()
        {
            _display.WriteSpecialLine();
            _display.WriteLine(Header);
            _display.WriteLine(NewBugTicketHeader);
            _display.WriteSpecialLine();
            _display.WriteLine("Enter watching names, comma separated:");
            _display.WriteSpecialLine();
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
                _display.WriteSpecialLine();
                _display.WriteLine(Header);
                _display.WriteLine(NewBugTicketHeader);
                _display.WriteSpecialLine();
                _display.WriteLine("1) Summary: " + summary);
                _display.WriteLine("2) Priority: " + priority);
                _display.WriteLine("3) Submitter: " + submitter);
                _display.WriteSpecialLine();
                _display.Write("Would you like to make a change?\n(enter 0 to accept): ");
                var input = _display.GetInput();

                switch (input)
                {
                    case "0":
                        correct = true;
                        var id = _stores.GetMaxId();
                        //Save ticket
                        _stores.AddTicket(new Bug(
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
                _display.WriteSpecialLine();
                _display.WriteLine(Header);
                _display.WriteLine(NewBugTicketHeader);
                _display.WriteSpecialLine();
                _display.WriteLine("1) Summary: " + summary);
                _display.WriteLine("2) Priority: " + priority);
                _display.WriteLine("3) Submitter: " + submitter);
                _display.WriteLine("4) Assigned: " + assigned);
                _display.WriteLine("1) Watching: " + watching);
                _display.WriteSpecialLine();
                _display.Write("Would you like to make a change?\n(enter 0 to accept): ");
                var input = _display.GetInput();

                switch (input)
                {
                    case "0":
                        correct = true;
                        var id = _stores.GetMaxId();
                        //Save ticket
                        _stores.AddTicket(new Bug(
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