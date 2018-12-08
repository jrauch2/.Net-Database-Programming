using System;
using System.Collections.Generic;
using NLog;
using System.Globalization;
using System.Linq;
using System.Data.Entity;
using Support_Ticket_System.Enums;
using Support_Ticket_System.Interfaces;
using Support_Ticket_System.Utility;

namespace Support_Ticket_System
{
    internal class SupportTicketSystem
    {
        private readonly Logger _logger;                // Logger for the SupportTicketSystem class.
        private readonly IDisplay _display;             // Display for the application.
        private readonly TextInfo _textInfo = new CultureInfo("en-US", false).TextInfo;

        // Set of strings used throughout the SupportTicketSystem class.
        private const char SpecialCharacter = '*';
        private const string Header = "Support Desk Ticket System";
        private const string NewBugTicketHeader = "New Bug Ticket";
        private const string NewEnhancementTicketHeader = "New Enhancement Ticket";
        private const string NewTaskTicketHeader = "New Task Ticket";
        private const string InvalidInput = "Try again. Invalid input: ";
        
        // Constructor for SupportTicketSystem.
        // Accepts an IDisplay object for user I/O
        public SupportTicketSystem(IDisplay display)
        {
            _logger = LogManager.GetCurrentClassLogger();               // Instantiate _logger.
            _logger.Debug("Setting the display and display size...");
            _display = display;
            _display.SetWindowSize(52, 30);
            _display.SpecialCharacter = SpecialCharacter;
            _logger.Trace("... Display set.");
        }

        // Start SupportTicketSystem
        public void Start()
        {
            _logger.Debug("Starting the Program...");
            Menu();
        }

        // The Main Menu for SupportTicketSystem
        private void Menu()
        {
            const string subHeader = "Main Menu";

            do
            {
                _display.Clear();
                PrintFullHeader(subHeader);
                _display.WriteLine("1) New Ticket");
                _display.WriteLine("2) Update Ticket");
                _display.WriteLine("3) Print All Tickets");
                _display.WriteLine("4) Search Tickets");
                _display.WriteLine("5) Delete Ticket");
                _display.WriteLine("6) Add New User");
                _display.WriteLine("0) Exit");
                _display.WriteSpecialLine();

                _logger.Trace("reading user input for Main Menu");
                _display.Write("Select an option: ");
                var input = _display.GetInput();

                switch (input)
                {
                    case "1": NewTicket();
                        break;              
                    case "2": UpdateTicket();
                        break;
                    case "3": PrintAllTickets();
                        break;
                    case "4": SearchAllTickets();
                        break;
                    case "5": DeleteTicket();
                        break;
                    case "6":
                        _logger.Debug("Opening connection to database to add new user...");
                        using (var db = new TicketDBContext())
                        {
                            try
                            {
                                _display.WriteLine("New user \"" + AddUser(db) + "\" added successfully.");
                                PressToContinue();
                            }
                            catch (Exception e)
                            {
                                _logger.Error(e.GetBaseException());
                                PrintExceptionCaughtMessage();
                            }
                        }
                        break;
                    case "0": CloseProgram();
                        break;
                    default: PrintInvalidInputMessage(input);
                        break;
                }
            } while (true);
            // ReSharper disable once FunctionNeverReturns
        }

        // Create a new Ticket
        private void NewTicket()
        {
            var subHeader = "Add a New Ticket";
            var newTicket = new Ticket { Status = Status.Open.ToString() };
            var valid = false;
            TicketType ticketType = null;

            _logger.Debug("Opening database connection for new ticket...");
            using (var db = new TicketDBContext())
            {
                try
                {
                    string input;
                    do
                    {
                        _display.Clear();
                        PrintFullHeader(subHeader);
                        _display.WriteLine("1) New Bug Ticket");
                        _display.WriteLine("2) New Enhancement Ticket");
                        _display.WriteLine("3) New Task Ticket");
                        _display.WriteLine("0) Cancel");
                        _display.WriteSpecialLine();

                        _display.Write("Select a ticket type: ");
                        input = _display.GetInput();

                        if (input.Equals("0")) return;

                        if (int.TryParse(input, out var inputResult))
                        {
                            valid = true;
                            ticketType = db.TicketTypes.SingleOrDefault(t => t.TicketTypeID == inputResult);
                        }
                        else PrintInvalidInputMessage(input);

                    } while (!valid);

                    if (ticketType == null) PrintInvalidInputMessage(input);
                        else
                        {
                            newTicket.TicketType = ticketType;
                            switch (ticketType.Description)
                            {
                                case "Bug":
                                    subHeader = NewBugTicketHeader;
                                    var bugAttribute = new BugAttribute
                                        { Severity = GetSeverityInput(subHeader).ToString() };
                                    newTicket.BugAttributes.Add(bugAttribute);
                                    break;
                                case "Enhancement":
                                    subHeader = NewEnhancementTicketHeader;
                                    var enhancementAttribute = new EnhancementAttribute
                                    {
                                        Cost = GetCostInput(subHeader),
                                        Estimate = GetEstimateInput(subHeader),
                                        Reason = GetReasonInput(subHeader),
                                        Software = GetSoftwareInput(subHeader)
                                    };
                                    newTicket.EnhancementAttributes.Add(enhancementAttribute);
                                    break;
                                case "Task":
                                    subHeader = NewTaskTicketHeader;
                                    var taskAttribute = new TaskAttribute
                                    {
                                        DueDate = GetDueDateInput(subHeader),
                                        ProjectName = GetProjectNameInput(subHeader)
                                    };
                                    newTicket.TaskAttributes.Add(taskAttribute);
                                    break;
                                default:
                                    _display.WriteLine("The ticket type \"" + ticketType.Description +
                                                       "\" is not supported by this application.");
                                    PressToContinue();
                                    break;
                            }
                        }

                    newTicket.Summary = GetSummaryInput(subHeader);
                    newTicket.Priority = GetPriorityInput(subHeader).ToString();

                    do
                    {
                        newTicket.SubmitterUser = GetUserFromInput(subHeader, "submitter", db);
                    } while (newTicket.SubmitterUser == null);

                    do
                    {
                        newTicket.AssignedUser = GetUserFromInput(subHeader, "assigned", db);
                    } while (newTicket.AssignedUser == null);

                    newTicket.WatchingUsers =
                        GetWatchingInput(subHeader, db, newTicket.SubmitterUser, newTicket.AssignedUser);

                    _logger.Debug("Adding Ticket to database...");
                    db.Tickets.Add(GetCorrectInput(subHeader, db, newTicket));
                    _logger.Debug("Saving changes to database...");
                    db.SaveChanges();
                    _display.Clear();
                    _display.WriteLine("Ticket added successfully.");
                    PressToContinue();
                }
                catch (Exception e)
                {
                    _logger.Error(e.GetBaseException());
                    PrintExceptionCaughtMessage();
                }
            }
        }

        // Search all of the tickets.
        // searchTerm is compared to the ticketID, summary, users' names, and users' departments.
        private void SearchAllTickets()
        {
            _display.Clear();
            var subHeader = "Search All Tickets";
            PrintFullHeader(subHeader);
            _display.Write("Enter search term: ");
            var searchTerm = _display.GetInput().ToLower();
            
            _display.Clear();
            subHeader = "Results for \"" + searchTerm + "\"";
            PrintFullHeader(subHeader);

            _logger.Debug("Opening connection to database to search...");
            using (var db = new TicketDBContext())
            {
                try
                {
                    var results = db.Tickets
                        .Include(wu => wu.WatchingUsers)
                        .Include(tt => tt.TicketType)
                        .Include(ba => ba.BugAttributes)
                        .Include(ea => ea.EnhancementAttributes)
                        .Include(ta => ta.TaskAttributes)
                        .Include(wu => wu.WatchingUsers.Select(u => u.User))
                        .Where(t => t.TicketID.ToString().ToLower().Equals(searchTerm)
                                    || t.Summary.ToLower().Contains(searchTerm)
                                    || t.SubmitterUser.FirstName.ToLower().Contains(searchTerm)
                                    || t.SubmitterUser.LastName.ToLower().Contains(searchTerm)
                                    || t.SubmitterUser.Department.ToLower().Contains(searchTerm)
                                    || t.AssignedUser.FirstName.ToLower().Contains(searchTerm)
                                    || t.AssignedUser.LastName.ToLower().Contains(searchTerm)
                                    || t.AssignedUser.Department.ToLower().Contains(searchTerm)
                                    || t.WatchingUsers.Any(wu => wu.User.FirstName.ToLower().Contains(searchTerm)
                                                                 || wu.User.LastName.ToLower().Contains(searchTerm)
                                                                 || wu.User.Department.ToLower().Contains(searchTerm)))
                        .Select(t => t);
                    _logger.Debug("Executing search query...");
                    foreach (var record in results) record.Print(_display);
                    PressToContinue();
                }
                catch (Exception e)
                {
                    _logger.Error(e.GetBaseException());
                    PrintExceptionCaughtMessage();
                }
            }
        }

        // Print all tickets in the database
        private void PrintAllTickets()
        {
            _display.Clear();
            const string subHeader = "All Tickets";
            PrintFullHeader(subHeader);

            _logger.Debug("Opening connection to database to print all tickets...");
            using (var db = new TicketDBContext())
            {
                try
                {
                    var results = db.Tickets
                        .Include(wu => wu.WatchingUsers)
                        .Include(tt => tt.TicketType)
                        .Include(ba => ba.BugAttributes)
                        .Include(ea => ea.EnhancementAttributes)
                        .Include(ta => ta.TaskAttributes)
                        .Include(wu => wu.WatchingUsers.Select(u => u.User))
                        .ToList();
                    _logger.Debug("Execute query to retrieve all tickets...");
                    foreach (var record in results) record.Print(_display);
                    PressToContinue();
                }
                catch (Exception e)
                {
                    _logger.Error(e.GetBaseException());
                    PrintExceptionCaughtMessage();
                }
            }
        }
        
        // Update a ticket, retrieve by TicketID
        private void UpdateTicket()
        {
            const string subHeader = "Update Ticket";
            var done = false;
            using (var db = new TicketDBContext())
            {
                try
                {
                    do
                    {
                        _display.Clear();
                        PrintFullHeader(subHeader);
                        _display.Write("Enter Ticket ID (0 to cancel): ");
                        var input = _display.GetInput();
                        if (int.TryParse(input, out var id))
                        {
                            if (id == 0) return;
                            if (!TryGetTicketById(db, id, out var ticket))
                            {
                                _display.WriteLine("Ticket ID not found.");
                                PressToContinue();
                                continue;
                            }
                            GetCorrectInput(subHeader, db, ticket);
                            _logger.Debug("Saving changes to ticket...");
                            db.SaveChanges();
                            _logger.Info("Ticket updated successfully.");
                            done = true;
                            _display.Clear();
                            _display.WriteLine("Ticket updated successfully.");
                            PressToContinue();
                        }
                        else PrintInvalidInputMessage(input);
                    } while (!done);
                }
                catch (Exception e)
                {
                    _logger.Error(e.GetBaseException());
                    PrintExceptionCaughtMessage();
                }
            }
        }

        private void DeleteTicket()
        {
            const string subHeader = "Delete Ticket";
            var done = false;
            using (var db = new TicketDBContext())
            {
                try
                {
                    do
                    {
                        _display.Clear();
                        PrintFullHeader(subHeader);
                        _display.Write("Enter Ticket ID (0 to cancel): ");
                        var input = _display.GetInput();
                        if (int.TryParse(input, out var id))
                        {
                            if (id == 0) return;
                            if (!TryGetTicketById(db, id, out var ticket))
                            {
                                _display.WriteLine("Ticket ID not found.");
                                PressToContinue();
                                continue;
                            }
                            _display.Clear();
                            PrintFullHeader(subHeader, "Ticket ID \"" + ticket.TicketID + "\" found");
                            ticket.Print(_display);
                            _display.WriteLine("Are you sure you want to delete this ticket?");
                            _display.WriteLine("THIS ACTION CANNOT BE UNDONE! (Y/N):");
                            input = _display.GetInput().ToUpper();
                            if (input.Equals("Y"))
                            {
                                db.WatchingUsers.RemoveRange(ticket.WatchingUsers);
                                db.BugAttributes.RemoveRange(ticket.BugAttributes);
                                db.EnhancementAttributes.RemoveRange(ticket.EnhancementAttributes);
                                db.TaskAttributes.RemoveRange(ticket.TaskAttributes);
                                db.Tickets.Remove(ticket);
                                db.SaveChanges();
                                done = true;
                                _display.Clear();
                                _display.WriteLine("Ticket deleted successfully.");
                                PressToContinue();
                            }
                            else if (input.Equals("N")) done = true;
                            else PrintInvalidInputMessage(input);
                        }
                        else PrintInvalidInputMessage(input);
                    } while (!done);
                }
                catch (Exception e)
                {
                    _logger.Error(e.GetBaseException());
                    PrintExceptionCaughtMessage();
                }
            }
        }
        
        // Get Severity from the user
        // accepts string subHeader to dynamically display sub-header depending on the calling method
        private Severity GetSeverityInput(string subHeader)
        {
            _display.Clear();
            var correct = false;
            var severity = Severity.Tier1;

            do
            {
                PrintFullHeader(subHeader, "Set support severity (Tier 1, Tier 2, Tier 3):");
                _display.SendWait("Tier ");

                var input = _display.GetInput();
                if (string.IsNullOrEmpty(input)) PrintInvalidInputMessage(input);
                else
                {
                    var severityInput = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input.Replace(" ", ""));
                    if (severityInput.Equals(Severity.Tier1.ToString())
                        || severityInput.Equals(Severity.Tier2.ToString())
                        || severityInput.Equals(Severity.Tier3.ToString()))
                    {
                        severity = severityInput.ToSeverity();
                        correct = true;
                    }
                }
                _display.Clear();
            } while (!correct);

            return severity;
        }

        // Get the Estimate input from the user
        // accepts string subHeader to dynamically display sub-header depending on the calling method
        // accepts string estimate to display the previous value for editing
        private string GetEstimateInput(string subHeader, string estimate = null)
        {
            _display.Clear();
            PrintFullHeader(subHeader, "Estimate:");
            if (estimate != null) _display.SendWait(estimate);
            return _display.GetInput();
        }

        // Get the Reason input from the user
        // accepts string subHeader to dynamically display sub-header depending on the calling method
        // accepts string reason to display the previous value for editing
        private string GetReasonInput(string subHeader, string reason = null)
        {
            _display.Clear();
            PrintFullHeader(subHeader, "Reason for enhancement:");
            if (reason != null) _display.SendWait(reason);
            return _display.GetInput();
        }

        // Get the Cost input from the user
        // accepts string subHeader to dynamically display sub-header depending on the calling method
        // accepts string cost to display the previous value for editing
        private string GetCostInput(string subHeader, string cost = null)
        {
            _display.Clear();
            PrintFullHeader(subHeader, "Cost of enhancement:");
            if (cost != null) _display.SendWait(cost);
            return _display.GetInput();
        }

        // Get the Software input from the user
        // accepts string subHeader to dynamically display sub-header depending on the calling method
        // accepts string software to display the previous value for editing
        private string GetSoftwareInput(string subHeader, string software = null)
        {
            _display.Clear();
            PrintFullHeader(subHeader, "Requested software:");
            if (software != null) _display.SendWait(software);
            return _display.GetInput();
        }

        // Get the DueDate input from the user
        // accepts string subHeader to dynamically display sub-header depending on the calling method
        // accepts string dueDate to display the previous value for editing
        private string GetDueDateInput(string subHeader, string dueDate = null)
        {
            _display.Clear();
            PrintFullHeader(subHeader, "Task Due Date:");
            if (dueDate != null) _display.SendWait(dueDate);
            return _display.GetInput();
        }

        // Get the ProjectName input from the user
        // accepts string subHeader to dynamically display sub-header depending on the calling method
        // accepts string projectName to display the previous value for editing
        private string GetProjectNameInput(string subHeader, string projectName = null)
        {
            _display.Clear();
            PrintFullHeader(subHeader, "Project Name:");
            if (projectName != null) _display.SendWait(projectName);
            return _display.GetInput();
        }

        // Get the Summary input from the user
        // accepts string subHeader to dynamically display sub-header depending on the calling method
        // accepts string summary to display the previous value for editing
        private string GetSummaryInput(string subHeader, string summary = null)
        {
            _display.Clear();
            PrintFullHeader(subHeader, "Write a summary of the issue (254 Char Max):");
            if ( summary != null) _display.SendWait(summary);
            return _display.GetInput();
        }

        // Get the Priority input from the user
        // accepts string subHeader to dynamically display sub-header depending on the calling method
        private Priority GetPriorityInput(string subHeader)
        {
            _display.Clear();
            var correct = false;
            var priority = Priority.Low;
            do
            {
                PrintFullHeader(subHeader, "Set a priority (Low, Medium, High, Severe):");
                var input = _display.GetInput();
                if (string.IsNullOrEmpty(input)) PrintInvalidInputMessage(input);
                else
                {
                    var priorityInput = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input);
                    if (priorityInput.Equals(Priority.Low.ToString())
                        || priorityInput.Equals(Priority.Medium.ToString())
                        || priorityInput.Equals(Priority.High.ToString())
                        || priorityInput.Equals(Priority.Severe.ToString()))
                    {
                        priority = priorityInput.ToPriority();
                        correct = true;
                    }
                }
                _display.Clear();
            } while (!correct);

            return priority;
        }

        // Get the Submitter name from the user
        // accepts string subHeader to dynamically display sub-header depending on the calling method
        // accepts the database context to retrieve a user
        private User GetUserFromInput(string subHeader, string userRole, TicketDBContext db, User user = null)
        {
            do
            {
                _display.Clear();
                PrintFullHeader(subHeader, "Enter " + userRole.ToLower() +" name (First Last):");
                if (user != null) _display.SendWait(user);
                var firstAndLastNames = _display.GetInput().Trim().Split(' ');
                
                if (firstAndLastNames.Length != 2) PrintInvalidInputMessage("Only one first name and one last name may be entered. ");
                else if (TryGetUserByName(db, firstAndLastNames[0], firstAndLastNames[1], out user)) return user;
                else
                {
                    var valid = false;
                    do
                    {
                        firstAndLastNames[0] = _textInfo.ToTitleCase(firstAndLastNames[0].ToLower());
                        firstAndLastNames[1] = _textInfo.ToTitleCase(firstAndLastNames[1].ToLower());
                        _display.Clear();
                        PrintFullHeader("User \"" + firstAndLastNames[0] + " " 
                                        + firstAndLastNames[1] + "\" does not exist.",
                                            " Would you like to add user? Y/N: ");
                        var input = _display.GetInput();

                        if (input.ToUpper().Equals("Y"))
                        {
                            valid = true;
                            AddUser(db, firstAndLastNames[0], firstAndLastNames[1]);
                        }
                        else if (input.ToUpper().Equals("N")) return user;
                        else PrintInvalidInputMessage(input);
                    } while (!valid);
                }
            } while (user == null);

            return user;
        }

        // Get the WatchingUsers names from the user
        // accepts string subHeader to dynamically display sub-header depending on the calling method
        // accepts the submitterUser and assignedUser to add to watchingUsers, as those users are required to be watching
        // accepts the database context to retrieve users
        private ICollection<WatchingUser> GetWatchingInput(string subHeader, TicketDBContext db, User submitter, User assigned, List<WatchingUser> watchingUsers = null)
        {
            var userList = new List<User>();

            if (watchingUsers != null)
            {
                foreach (var watchingUser in watchingUsers.ToArray())
                {
                    if (watchingUser.User.Equals(submitter))
                        watchingUsers.Remove(watchingUser);
                    if (watchingUser.User.Equals(assigned))
                        watchingUsers.Remove(watchingUser);
                }
            }

            _display.Clear();
            PrintFullHeader(subHeader, "Enter watching names, comma separated(First Last, First Last):");
            if (submitter != assigned) _display.Write(submitter + ", " + assigned + ", ");
            else _display.Write(submitter + ", ");
            if (watchingUsers != null) _display.SendWait(watchingUsers.ToFormattedString());
            var userInput = _display.GetInput();

            if (submitter != assigned)
            {
                watchingUsers = new List<WatchingUser>
                {
                    new WatchingUser {User = submitter}, new WatchingUser {User = assigned}
                };
            }
            else watchingUsers = new List<WatchingUser> { new WatchingUser { User = submitter } };

            if (userInput == "") return watchingUsers;
            var names = userInput.Trim().Split(',');

            foreach (var name in names)
            {
                var firstAndLastNames = name.Trim().Split(' ');

                if (firstAndLastNames.Length != 2) PrintInvalidInputMessage("Only one first name and one last name may be entered. ");
                else if (TryGetUserByName(db, firstAndLastNames[0], firstAndLastNames[0], out var user)) userList.Add(user);
                else
                {
                    var valid = false;
                    do
                    {
                        firstAndLastNames[0] = _textInfo.ToTitleCase(firstAndLastNames[0].ToLower());
                        firstAndLastNames[1] = _textInfo.ToTitleCase(firstAndLastNames[1].ToLower());
                        _display.Clear();
                        PrintFullHeader("User \"" + firstAndLastNames[0] + " "
                                        + firstAndLastNames[1] + "\" does not exist.",
                            " Would you like to add user? Y/N: ");
                        var input = _display.GetInput();

                        if (input.ToUpper().Equals("Y"))
                        {
                            valid = true;
                            AddUser(db, firstAndLastNames[0], firstAndLastNames[1]);
                        }
                        else if (input.ToUpper().Equals("N")) valid = true;
                        else PrintInvalidInputMessage(input);
                    } while (!valid);
                }
            }

            foreach (var user in userList)
            {
                for (var i = 0; i < watchingUsers.Count; i++)
                {
                    if (watchingUsers[i].User == user) continue;
                    watchingUsers.Add(new WatchingUser { User = user });
                }
            }

            return watchingUsers;
        }

        //Ask the user to confirm that the information is correct
        // accepts string subHeader to dynamically display sub-header depending on the calling method
        // accepts the database context to retrieve information from the database
        // accepts the ticket to be validated by the user
        private Ticket GetCorrectInput(string subHeader, TicketDBContext db, Ticket ticket)
        {
            do
            {
                _display.Clear();
                PrintFullHeader(subHeader);
                _display.WriteLine("1) Summary: " + ticket.Summary);
                _display.WriteLine("2) Priority: " + ticket.Priority);
                _display.WriteLine("3) Submitter: " + ticket.SubmitterUser);
                _display.WriteLine("4) Assigned: " + ticket.AssignedUser);
                _display.WriteLine("5) Watching: " + ticket.WatchingUsers.ToFormattedString());
                if (ticket.BugAttributes.Count > 0)
                    foreach (var ticketBugAttribute in ticket.BugAttributes)
                        _display.WriteLine("6) Severity: Tier " + (int) ticketBugAttribute.Severity.ToSeverity());

                if (ticket.EnhancementAttributes.Count > 0)
                    foreach (var ticketEnhancementAttribute in ticket.EnhancementAttributes)
                    {
                        _display.WriteLine("7) Cost: " + ticketEnhancementAttribute.Cost);
                        _display.WriteLine("8) Estimate: " + ticketEnhancementAttribute.Estimate);
                        _display.WriteLine("9) Reason: " + ticketEnhancementAttribute.Reason);
                        _display.WriteLine("10) Software: " + ticketEnhancementAttribute.Software);
                    }

                if (ticket.TaskAttributes.Count > 0)
                    foreach (var ticketTaskAttribute in ticket.TaskAttributes)
                    {
                        _display.WriteLine("11) Project Name: " + ticketTaskAttribute.ProjectName);
                        _display.WriteLine("12) Due Date: " + ticketTaskAttribute.DueDate);
                    }
                // START HERE
                _display.WriteLine();
                _display.WriteSpecialLine();
                _display.Write("Would you like to make a change?\n(enter 0 to accept): ");
                var input = _display.GetInput();

                switch (input)
                {
                    case "0": return ticket;
                    case "1": ticket.Summary = GetSummaryInput(subHeader, ticket.Summary);
                        break;
                    case "2": ticket.Priority = GetPriorityInput(subHeader).ToString();
                        break;
                    case "3": ticket.SubmitterUser = GetUserFromInput(subHeader, "submitter", db, ticket.SubmitterUser);
                        break;
                    case "4": ticket.AssignedUser = GetUserFromInput(subHeader, "assigned", db, ticket.AssignedUser);
                        break;
                    case "5":
                        var watchingUsers = GetWatchingInput(subHeader, db, ticket.SubmitterUser, ticket.AssignedUser,
                            ticket.WatchingUsers.ToList());
                        db.WatchingUsers.RemoveRange(ticket.WatchingUsers);
                        ticket.WatchingUsers = watchingUsers;
                        break;
                    case "6": ticket.BugAttributes.Single().Severity = GetSeverityInput(subHeader).ToString();
                        break;
                    case "7":
                        ticket.EnhancementAttributes.Single().Cost =
                            GetCostInput(subHeader, ticket.EnhancementAttributes.Single().Cost);
                        break;
                    case "8":
                        ticket.EnhancementAttributes.Single().Estimate = GetEstimateInput(subHeader,
                            ticket.EnhancementAttributes.Single().Estimate);
                        break;
                    case "9":
                        ticket.EnhancementAttributes.Single().Reason =
                            GetReasonInput(subHeader, ticket.EnhancementAttributes.Single().Reason);
                        break;
                    case "10":
                        ticket.EnhancementAttributes.Single().Software = GetSoftwareInput(subHeader,
                            ticket.EnhancementAttributes.Single().Software);
                        break;
                    case "11":
                        ticket.TaskAttributes.Single().ProjectName =
                            GetProjectNameInput(subHeader, ticket.TaskAttributes.Single().ProjectName);
                        break;
                    case "12":
                        ticket.TaskAttributes.Single().DueDate =
                            GetDueDateInput(subHeader, ticket.TaskAttributes.Single().DueDate);
                        break;
                    default: PrintInvalidInputMessage(input);
                        break;
                }

                _display.Clear();
            } while (true);
        }

        // Try to get the user by first and last names
        // accepts the database context for retrieving a user
        // accepts first and last names as strings
        // output argument "user" is the user retrieved from the database
        private bool TryGetUserByName(TicketDBContext db, string fName, string lName, out User user)
        {
            IQueryable<User> users = null;
            try
            {
                users = db.Users
                    .Where((u => (fName.Trim().Equals(u.FirstName)) && (lName.Trim().Equals(u.LastName))))
                    .Select(u => u);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetBaseException());
                PrintExceptionCaughtMessage();
            }
            if (users?.Count() == 1)
            {
                user = users.First();
                return true;
            }

            if (users?.Count() > 1)
            {
                do
                {
                    _display.WriteLine("More than 1 result found");
                    _display.WriteLine("Select which user: ");
                    foreach (var user1 in users)
                    {
                        _display.WriteLine(
                            user1.UserID + ") " + user1.FirstName + " " + user1.LastName + ", " +
                            user1.Department);
                    }

                    _display.WriteLine("0) None of the above.");
                    var input = _display.GetInput();
                    if (!int.TryParse(input, out var inputResult)) continue;
                    if (inputResult == 0)
                    {
                        user = null;
                        return false;
                    }

                    foreach (var u in users)
                    {
                        if (inputResult != u.UserID) continue;
                        user = u;
                        return true;
                    }
                } while (true);
            }
            user = null;
            return false;
        }

        private bool TryGetTicketById(TicketDBContext db, int id, out Ticket ticket)
        {
            ticket = null;
            
            if (id < 1) return false;
            _logger.Debug("Opening connection to database to retrieve and update ticket...");
            try
            {
                var result = db.Tickets
                    .Include(wu => wu.WatchingUsers)
                    .Include(tt => tt.TicketType)
                    .Include(ba => ba.BugAttributes)
                    .Include(ea => ea.EnhancementAttributes)
                    .Include(ta => ta.TaskAttributes)
                    .Include(wu => wu.WatchingUsers.Select(u => u.User))
                    .SingleOrDefault(t => t.TicketID == id);
                if (result == null) PrintInvalidInputMessage(id);
                else
                {
                    ticket = result;
                    return true;
                }
            }
            catch (Exception e)
            {
                _logger.Error(e.GetBaseException());
                PrintExceptionCaughtMessage();
            }
            
            return false;
        }

        // add a user to the database from first and last name
        // accepts the database context and the strings for first and last names
        private User AddUser(TicketDBContext db, string fName = null, string lName = null)
        {
            string dept;
            if (fName != null && lName != null)
            {
                var done = false;

                _display.WriteLine("Enter user's first name:");
                fName = _display.GetInput();
                _display.WriteLine("Enter user's last name:");
                lName = _display.GetInput();
                _display.WriteLine("Enter user's department: ");
                dept = _display.GetInput();
                do
                {
                    _display.WriteLine("1) " + fName);
                    _display.WriteLine("2) " + lName);
                    _display.WriteLine("3) " + dept);
                    _display.Write("Would you like to make a change?\n(enter 0 to accept): ");
                    var input = _display.GetInput();
                    switch (input.ToUpper())
                    {
                        case "0":
                            done = true;
                            break;
                        case "1":
                            _display.WriteLine("Enter user's first name:");
                            fName = _display.GetInput();
                            break;
                        case "2":
                            _display.WriteLine("Enter user's last name:");
                            lName = _display.GetInput();
                            break;
                        case "3":
                            _display.WriteLine("Enter user's department: ");
                            dept = _display.GetInput();
                            break;
                        default:
                            PrintInvalidInputMessage(input);
                            break;
                    }
                } while (!done);
            }
            var valid = false;
            if (TryGetUserByName(db, fName, lName, out var user))
            {
                do
                {
                    _display.WriteLine("That user's name already exist. Is this a new user with the same name? Y/N:");
                    var input = _display.GetInput();
                    switch (input.ToUpper())
                    {
                        case "Y": valid = true;
                            break;
                        case "N": break;
                        default: PrintInvalidInputMessage(input);
                            break;
                    }
                } while (!valid);
            }
            valid = false;
            do
            {
                _display.WriteLine("Enter user's department: ");
                dept = _display.GetInput();
                _display.Write("Is this correct?: " + dept + "\nY/N: ");
                var input = _display.GetInput();
                switch (input.ToUpper())
                {
                    case "Y": valid = true;
                        break;
                    case "N": break;
                    default: PrintInvalidInputMessage(input);
                        break;
                }
            } while (!valid);
            user = new User { FirstName = fName, LastName = lName, Department = dept };
            db.Users.Add(user);
            db.SaveChanges();
            return user;
        }

        private void PrintInvalidInputMessage<T>(T input)
        {
            _display.Clear();
            PrintFullHeader(InvalidInput + "\"" + input + "\"");
            PressToContinue();
        }

        private void PrintExceptionCaughtMessage()
        {
            _display.WriteLine("There was an error completing that action. Check the log for details.");
            PressToContinue();
        }

        private void PrintFullHeader(string subHeader, string requestForInput)
        {
            _display.WriteSpecialLine();
            _display.WriteLine(Header);
            _display.WriteLine();
            _display.WriteLine(subHeader);
            _display.WriteSpecialLine();
            _display.WriteLine(requestForInput);
            _display.WriteSpecialLine();
        }

        private void PrintFullHeader(string subHeader)
        {
            _display.WriteSpecialLine();
            _display.WriteLine(Header);
            _display.WriteLine(subHeader);
            _display.WriteSpecialLine();
        }

        private void PressToContinue()
        {
            _display.WriteLine("Press any key to continue...");
            _display.GetInput();
        }
        
        // Terminate the program with an exit code of 0
        private void CloseProgram()
        {
            _logger.Trace("...Program closed.");
            Environment.Exit(0);
        }
    }
}