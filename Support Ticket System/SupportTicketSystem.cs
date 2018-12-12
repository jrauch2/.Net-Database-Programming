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
        private const string BugTicketHeader = "New Bug Ticket";
        private const string EnhancementTicketHeader = "New Enhancement Ticket";
        private const string TaskTicketHeader = "New Task Ticket";
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
            var subHeader = "Main Menu";

            do
            {
                _display.Clear();
                PrintFullHeader(subHeader);
                _display.WriteLine("1) Manage Tickets");
                _display.WriteLine("2) Manage Users");
                _display.WriteLine("3) Statistics");
                _display.WriteLine("0) Exit");
                _display.WriteSpecialLine();

                _logger.Trace("reading user input for Main Menu");
                _display.Write("Select an option: ");
                var input = _display.GetInput();

                switch (input)
                {
                    case "1":
                        subHeader = "Ticket Menu";
                        _display.Clear();
                        PrintFullHeader(subHeader);
                        _display.WriteLine("1) New Ticket");
                        _display.WriteLine("2) Update Ticket");
                        _display.WriteLine("3) Print All Tickets");
                        _display.WriteLine("4) Search Tickets");
                        _display.WriteLine("5) Delete Ticket");
                        _display.WriteLine("0) Cancel");
                        _logger.Trace("reading user input for Main Menu");
                        _display.Write("Select an option: ");
                        var ticketMenuInput = _display.GetInput();

                        switch (ticketMenuInput) { 
                            case "1": AddTicket(); break;
                            case "2": UpdateTicket(); break;
                            case "3": PrintAllTickets(); break;
                            case "4": SearchAllTickets(); break;
                            case "5": DeleteTicket(); break;
                            case "0": break;
                            default: PrintInvalidInputMessage(input); break;
                        }
                        break;
                    case "2":
                        subHeader = "User Menu";
                        _display.Clear();
                        PrintFullHeader(subHeader);
                        _display.WriteLine("1) Add User");
                        _display.WriteLine("2) Update User");
                        _display.WriteLine("3) Print All Users");
                        _display.WriteLine("4) Search Users");
                        _display.WriteLine("5) Delete User");
                        _display.WriteLine("0) Cancel");
                        _logger.Trace("reading user input for Main Menu");
                        _display.Write("Select an option: ");
                        var userMenuInput = _display.GetInput();

                        switch (userMenuInput)
                        {
                            case "1": AddUser(out _); break;
                            case "2": UpdateUser(); break;
                            case "3": PrintAllUsers(); break;
                            case "4": SearchAllUsers(); break;
                            case "5": DeleteUser(); break;
                            case "0": break;
                            default: PrintInvalidInputMessage(userMenuInput); break;
                        }
                        break;
                    case "3":
                        _display.Clear();
                        subHeader = "Statistics";
                        PrintFullHeader(subHeader);
                        _display.WriteSpecialLine();
                        _display.WriteLine("1) Tickets With Most Watching Users");
                        _display.WriteLine("0) Cancel");
                        _logger.Trace("reading user input for Main Menu");
                        _display.Write("Select an option: ");
                        var statisticsMenuInput = _display.GetInput();

                        switch (statisticsMenuInput)
                        {
                            case "1": TicketsWithMostWatchingUsers(); break;
                            case "0": break;
                            default: PrintInvalidInputMessage(statisticsMenuInput); break;
                        }
                        break;
                    case "0": CloseProgram(); break;
                    default: PrintInvalidInputMessage(input); break;
                }
            } while (true);
            // ReSharper disable once FunctionNeverReturns
        }

        private void TicketsWithMostWatchingUsers()
        {
            var tickets = new List<TicketResult>();
            var ticketResult = new TicketResult { TicketID = 0 };

            using (var db = new TicketDBContext())
            {
                var list = db.TicketWithMostWatchingUsers().Select(t => t).ToList();
                
                foreach (var ticketWithMostWatchingUsersResult in list)
                {
                    if (ticketWithMostWatchingUsersResult.TicketID == null) continue;
                    var ticketId = (int) ticketWithMostWatchingUsersResult.TicketID;
                    if (ticketResult.TicketID == ticketId)
                    {
                        if (ticketWithMostWatchingUsersResult.WatchingID == null) continue;
                        var watchingId = (int) ticketWithMostWatchingUsersResult.WatchingID;
                        if (ticketWithMostWatchingUsersResult.WatchingEnabled == null) continue;
                        var watchingUser = new WatchingUser
                        {
                            User = new User
                            {
                                UserID = watchingId,
                                FirstName = ticketWithMostWatchingUsersResult.WatchingFirstName,
                                LastName = ticketWithMostWatchingUsersResult.WatchingLastName,
                                Department = ticketWithMostWatchingUsersResult.WatchingDepartment,
                                Enabled = (int)ticketWithMostWatchingUsersResult.WatchingEnabled
                            }
                        };
                        if (!ticketResult.WatchingUsers.Contains(watchingUser))
                        {
                            ticketResult.WatchingUsers.Add(watchingUser);
                        }
                    }
                    else
                    {
                        tickets.Add(ticketResult);
                        ticketResult = new TicketResult
                        {
                            TicketID = ticketWithMostWatchingUsersResult.TicketID,
                            Summary = ticketWithMostWatchingUsersResult.Summary,
                            Status = ticketWithMostWatchingUsersResult.Status,
                            Priority = ticketWithMostWatchingUsersResult.Priority,
                            SubmitterID = ticketWithMostWatchingUsersResult.SubmitterID,
                            SubmitterFirstName = ticketWithMostWatchingUsersResult.SubmitterFirstName,
                            SubmitterLastName = ticketWithMostWatchingUsersResult.SubmitterLastName,
                            SubmitterDepartment = ticketWithMostWatchingUsersResult.SubmitterDepartment,
                            SubmitterEnabled = ticketWithMostWatchingUsersResult.SubmitterEnabled,
                            AssignedID = ticketWithMostWatchingUsersResult.AssignedID,
                            AssignedFirstName = ticketWithMostWatchingUsersResult.AssignedFirstName,
                            AssignedLastName = ticketWithMostWatchingUsersResult.AssignedLastName,
                            AssignedDepartment = ticketWithMostWatchingUsersResult.AssignedDepartment,
                            AssignedEnabled = ticketWithMostWatchingUsersResult.AssignedEnabled,
                            TicketType = ticketWithMostWatchingUsersResult.TicketType,
                            BugAttrID = ticketWithMostWatchingUsersResult.BugAttrID,
                            Severity = ticketWithMostWatchingUsersResult.Severity,
                            EnhanceAttrID = ticketWithMostWatchingUsersResult.EnhanceAttrID,
                            Cost = ticketWithMostWatchingUsersResult.Cost,
                            Estimate = ticketWithMostWatchingUsersResult.Estimate,
                            Reason = ticketWithMostWatchingUsersResult.Reason,
                            Software = ticketWithMostWatchingUsersResult.Software,
                            TaskAttrID = ticketWithMostWatchingUsersResult.TaskAttrID,
                            ProjectName = ticketWithMostWatchingUsersResult.ProjectName,
                            DueDate = ticketWithMostWatchingUsersResult.DueDate,
                            WatchingUsers = new List<WatchingUser>()
                        };

                        if (ticketWithMostWatchingUsersResult.WatchingID == null) continue;
                        if (ticketWithMostWatchingUsersResult.WatchingEnabled != null)
                            ticketResult.WatchingUsers.Add(
                                new WatchingUser
                                {
                                    User = new User()
                                    {
                                        UserID = (int) ticketWithMostWatchingUsersResult.WatchingID,
                                        FirstName = ticketWithMostWatchingUsersResult.WatchingFirstName,
                                        LastName = ticketWithMostWatchingUsersResult.WatchingLastName,
                                        Department = ticketWithMostWatchingUsersResult.WatchingDepartment,
                                        Enabled = (int) ticketWithMostWatchingUsersResult.WatchingEnabled
                                    }
                                });
                    }
                }
            }

            var subHeader = "Tickets With the Most Watching Users";
            _display.Clear();
            PrintFullHeader(subHeader, "Number of Watching Users: " + ticketResult.WatchingUsers.Count);
            _display.WriteLine("Ticket ID: " + ticketResult.TicketID);
            _display.WriteLine("Status: " + ticketResult.Status + "     Priority: " + ticketResult.Priority);
            _display.WriteLine("Summary: " + ticketResult.Summary);
            if (ticketResult.TicketType.Equals("Bug"))
            {
                _display.WriteLine("Severity: Tier " + (int)ticketResult.Severity.ToSeverity());
            }
            if (ticketResult.TicketType.Equals("Enhancement"))
            {
                _display.WriteLine("Software: " + ticketResult.Software);
                _display.WriteLine("Reason: " + ticketResult.Reason);
                _display.WriteLine("Estimate: " + ticketResult.Estimate);
                _display.WriteLine("Cost: " + ticketResult.Cost);
            }
            if (ticketResult.TicketType.Equals("Task"))
            {
                _display.WriteLine("Project Name: " + ticketResult.ProjectName);
                _display.WriteLine("Due Date: " + ticketResult.DueDate);
            }
            _display.WriteLine("Submitter: " + ticketResult.SubmitterFirstName + " " + ticketResult.SubmitterLastName);
            _display.WriteLine("Assigned: " + ticketResult.AssignedFirstName + " " + ticketResult.AssignedLastName);
            _display.WriteLine("Watching Users:");
            _display.WriteLine(ticketResult.WatchingUsers.ToFormattedString());
            _display.WriteSpecialLine();
            PressToContinue();
        }

        private void DeleteUser()
        {
            const string subHeader = "Delete User";
            var done = false;
            using (var db = new TicketDBContext())
            {
                try
                {
                    do
                    {
                        _display.Clear();
                        PrintFullHeader(subHeader);
                        _display.Write("Enter User ID (0 to cancel): ");
                        var input = _display.GetInput();
                        if (int.TryParse(input, out var id))
                        {
                            if (id < 1) return;
                            if (!TryGetUserById(id, db, out var user))
                            {
                                _display.WriteLine("User ID not found.");
                                PressToContinue();
                                continue;   
                            } 
                            _display.Clear();
                            PrintFullHeader(subHeader, "User ID \"" + user.UserID + "\" found");
                            user.Print(_display);
                            _display.WriteLine("Are you sure you want to delete this user?");
                            _display.WriteLine("THIS ACTION CANNOT BE UNDONE! (Y/N):");
                            input = _display.GetInput().ToUpper();
                            if (input.Equals("Y"))
                            {
                                db.Users.Remove(user);
                                db.SaveChanges();
                                done = true;
                                _display.Clear();
                                PrintFullHeader(subHeader);
                                _display.WriteLine("User deleted successfully.");
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

        private bool TryGetUserById(int id, TicketDBContext db, out User user)
        {
            user = null;

            if (id < 1) return false;
            _logger.Debug("Opening connection to database to retrieve and update ticket...");
            try
            {
                var result = db.Users.SingleOrDefault(u => u.UserID == id);
                    
                if (result == null) return false;
                if (result.Enabled == 0)
                {
                    user = result;
                    return false;
                }
                user = result;
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e.GetBaseException());
                PrintExceptionCaughtMessage();
            }

            return false;
        }

        private void SearchAllUsers()
        {
            _display.Clear();
            var subHeader = "Search All Users";
            PrintFullHeader(subHeader);
            _display.Write("Enter search term: ");
            var searchTerm = _display.GetInput().ToLower();

            _display.Clear();
            subHeader = "Results for \"" + searchTerm + "\"";
            PrintFullHeader(subHeader);

            _logger.Debug("Opening connection to database to search users...");
            using (var db = new TicketDBContext())
            {
                try
                {
                    var results = db.Users
                        .Where(u => u.UserID.ToString().ToLower().Equals(searchTerm)
                                    || u.FirstName.ToLower().Contains(searchTerm)
                                    || u.LastName.ToLower().Contains(searchTerm)
                                    || u.Department.ToLower().Contains(searchTerm))
                        .Select(u => u);
                    _logger.Debug("Executing user search query \"" + searchTerm + "\"...");
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

        private void PrintAllUsers()
        {
            _display.Clear();
            const string subHeader = "All Users";
            PrintFullHeader(subHeader);

            _logger.Debug("Opening connection to database to print all users...");
            using (var db = new TicketDBContext())
            {
                try
                {
                    var results = db.Users.ToList();
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

        // Create a new Ticket
        private void AddTicket()
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
                                subHeader = BugTicketHeader;
                                var bugAttribute = new BugAttribute
                                    {Severity = GetSeverityInput(subHeader).ToString()};
                                newTicket.BugAttributes.Add(bugAttribute);
                                break;
                            case "Enhancement":
                                subHeader = EnhancementTicketHeader;
                                var enhancementAttribute = new EnhancementAttribute
                                {
                                    Cost = GetInput(subHeader, "Cost of enhancement:"),
                                    Estimate = GetInput(subHeader, "Estimate: "),
                                    Reason = GetInput(subHeader, "Reason for enhancement:"),
                                    Software = GetInput(subHeader, "Requested software:")
                                };
                                newTicket.EnhancementAttributes.Add(enhancementAttribute);
                                break;
                            case "Task":
                                subHeader = TaskTicketHeader;
                                var taskAttribute = new TaskAttribute
                                {
                                    DueDate = GetInput(subHeader, "Task Due Date:"),
                                    ProjectName = GetInput(subHeader, "Project Name:")
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

                    newTicket.Summary = GetInput(subHeader, "Write a summary of the issue (254 Char Max):");
                    newTicket.Priority = GetPriorityInput(subHeader).ToString();

                    do
                    {
                        newTicket.SubmitterUser = GetUserFromInput(db, subHeader, "submitter", "first and last name");
                    } while (newTicket.SubmitterUser == null);

                    do
                    {
                        newTicket.AssignedUser = GetUserFromInput(db, subHeader, "assigned", "first and last name");
                    } while (newTicket.AssignedUser == null);

                    newTicket.WatchingUsers =
                        GetWatchingInput(subHeader, newTicket.SubmitterUser, newTicket.AssignedUser, db);
                    var correctedTicket = GetCorrectTicketInput(subHeader, db, newTicket);

                    _logger.Debug("Adding Ticket to database...");
                    db.Tickets.Add(correctedTicket);
                    _logger.Debug("Saving changes to database...");
                    db.SaveChanges();
                    _display.Clear();
                    PrintFullHeader(subHeader);
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
                            if (!TryGetTicketById(id, db, out var ticket))
                            {
                                _display.Clear();
                                PrintFullHeader(subHeader);
                                _display.WriteLine("Ticket ID not found.");
                                PressToContinue();
                                continue;
                            }
                            GetCorrectTicketInput(subHeader, db, ticket);
                            _logger.Debug("Saving changes to ticket...");
                            db.SaveChanges();
                            _logger.Info("Ticket updated successfully.");
                            done = true;
                            _display.Clear();
                            PrintFullHeader(subHeader);
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
                            if (!TryGetTicketById(id, db, out var ticket))
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
                                PrintFullHeader(subHeader);
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
        private string GetInput(string subHeader, string requestForInput, string previousValue = null)
        {
            _display.Clear();
            PrintFullHeader(subHeader, requestForInput);
            if (previousValue != null) _display.SendWait(previousValue);
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
        private User GetUserFromInput(TicketDBContext db, string subHeader, string userRole, string infoType = "", string previousValue = "", User user = null)
        {
            do
            {
                var firstAndLastNames = GetUserInfo(subHeader, userRole, infoType, user == null ? previousValue : user.ToString()).Trim().Split(' ');
                if (firstAndLastNames.Length != 2) PrintInvalidInputMessage("Only one first name and one last name may be entered. ");
                else if (TryGetUserByName(subHeader, firstAndLastNames[0], firstAndLastNames[1], db, out user)) return user;
                else if (user != null)
                {
                    _display.Clear();
                    PrintInvalidInputMessage("User \"" + user.UserID + ") " + user.FirstName + " " + user.LastName + "\" was found, but is disabled.\nPlease re-enable user to use.");
                     _display.WriteLine();  
                }
                else
                {
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
                            AddUser(out user, subHeader, firstAndLastNames[0], firstAndLastNames[1]);
                            return user;
                        }

                        if (input.ToUpper().Equals("N")) return null;
                        PrintInvalidInputMessage(input);
                    } while (true);
                }
            } while (true);
        }

        // Get the WatchingUsers names from the user
        // accepts string subHeader to dynamically display sub-header depending on the calling method
        // accepts the submitterUser and assignedUser to add to watchingUsers, as those users are required to be watching
        // accepts the database context to retrieve users
        private ICollection<WatchingUser> GetWatchingInput(string subHeader, User submitter, User assigned, TicketDBContext db, List<WatchingUser> watchingUsers = null)
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
            if (!submitter.Equals(assigned)) _display.Write(submitter + ", " + assigned + ", ");
            else _display.Write(submitter + ", ");
            if (watchingUsers != null) _display.SendWait(watchingUsers.ToFormattedString());
            var userInput = _display.GetInput();

            if (!submitter.Equals(assigned))
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
                else if (TryGetUserByName(subHeader, firstAndLastNames[0], firstAndLastNames[1], db, out var user))
                {
                    userList.Add(user);
                }
                else if (user != null)
                {
                    _display.Clear();
                    PrintFullHeader(subHeader, "User \"" + user.UserID + ") " + user.FirstName + " " + user.LastName + "\" was found, but is disabled.\nPlease re-enable user to use.");
                    _display.WriteLine();
                }
            }

            foreach (var user in userList)
            {
                var watchingUser = new WatchingUser{ User = user};
                if (!watchingUsers.Contains(watchingUser))
                {
                    watchingUsers.Add(watchingUser);
                }
            }

            
            return watchingUsers;
        }

        //Ask the user to confirm that the information is correct
        // accepts string subHeader to dynamically display sub-header depending on the calling method
        // accepts the database context to retrieve information from the database
        // accepts the ticket to be validated by the user
        private Ticket GetCorrectTicketInput(string subHeader, TicketDBContext db, Ticket ticket)
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
                _display.WriteLine();
                _display.WriteSpecialLine();
                _display.Write("Would you like to make a change?\n(enter 0 to accept): ");
                var input = _display.GetInput();

                switch (input)
                {
                    case "0": return ticket;
                    case "1": ticket.Summary = GetInput(subHeader, "Write a summary of the issue (254 Char Max):", ticket.Summary);
                        break;
                    case "2": ticket.Priority = GetPriorityInput(subHeader).ToString();
                        break;
                    case "3": ticket.SubmitterUser = GetUserFromInput(db, subHeader, "submitter", "first and last name", user: ticket.SubmitterUser);
                        break;
                    case "4": ticket.AssignedUser = GetUserFromInput(db, subHeader, "assigned", "first and last name", user: ticket.AssignedUser);
                        break;
                    case "5":
                        var watchingUsers = GetWatchingInput(subHeader, ticket.SubmitterUser, ticket.AssignedUser, db,
                            ticket.WatchingUsers.ToList());
                        db.WatchingUsers.RemoveRange(ticket.WatchingUsers);
                        ticket.WatchingUsers = watchingUsers;
                        break;
                    case "6":
                        if (ticket.BugAttributes.Count > 0)
                        {
                            ticket.BugAttributes.Single().Severity = GetSeverityInput(subHeader).ToString();
                            break;
                        }
                        else
                        {
                            PrintInvalidInputMessage(input);
                            continue;
                        }
                    case "7":
                        if (ticket.EnhancementAttributes.Count > 0)
                        {
                            ticket.EnhancementAttributes.Single().Cost =
                                GetInput(subHeader, "Cost of enhancement:", ticket.EnhancementAttributes.Single().Cost);
                            break;
                        }
                        else
                        {
                            PrintInvalidInputMessage(input);
                            continue;
                        }
                    case "8":
                        if (ticket.EnhancementAttributes.Count > 0)
                        {
                            ticket.EnhancementAttributes.Single().Estimate = GetInput(subHeader,
                                ticket.EnhancementAttributes.Single().Estimate);
                            break;
                        }
                        else
                        {
                            PrintInvalidInputMessage(input);
                            continue;
                        }
                    case "9":
                        if (ticket.EnhancementAttributes.Count > 1)
                        {
                            ticket.EnhancementAttributes.Single().Reason =
                                GetInput(subHeader, ticket.EnhancementAttributes.Single().Reason);
                            break;
                        }
                        else
                        {
                            PrintInvalidInputMessage(input);
                            continue;
                        }
                    case "10":
                        if (ticket.EnhancementAttributes.Count > 1)
                        {
                            ticket.EnhancementAttributes.Single().Software = GetInput(subHeader, "Requested software:",
                                ticket.EnhancementAttributes.Single().Software);
                            break;
                        }
                        else
                        {
                            PrintInvalidInputMessage(input);
                            continue;
                        }
                    case "11":
                        if (ticket.TaskAttributes.Count > 1)
                        {
                            ticket.TaskAttributes.Single().ProjectName =
                                GetInput(subHeader, "Project Name:", ticket.TaskAttributes.Single().ProjectName);
                            break;
                        }
                        else
                        {
                            PrintInvalidInputMessage(input);
                            continue;
                        }
                    case "12":
                        if (ticket.TaskAttributes.Count > 1)
                        {
                            ticket.TaskAttributes.Single().DueDate =
                                GetInput(subHeader, "Task Due Date:", ticket.TaskAttributes.Single().DueDate);
                            break;
                        }
                        else
                        {
                            PrintInvalidInputMessage(input);
                            continue;
                        }
                    default: PrintInvalidInputMessage(input);
                        continue;
                }
            } while (true);
        }

        // Try to get the user by first and last names
        // accepts the database context for retrieving a user
        // accepts first and last names as strings
        // output argument "user" is the user retrieved from the database
        private bool TryGetUserByName(string subHeader, string fName, string lName, TicketDBContext db, out User user)
        {
            user = null;
            
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
                return user.Enabled != 0;
            }

            if (!(users?.Count() > 1)) return false;
            
            do
            {
                _display.Clear();
                PrintFullHeader(subHeader, "More than 1 result found. Select which user:");
                _display.WriteLine();
                foreach (var user1 in users)
                {
                    _display.WriteLine(
                        user1.UserID + ") " + user1.FirstName + " " + user1.LastName + ", " +
                        user1.Department);
                }

                _display.WriteLine("0) None of the above.");
                var input = _display.GetInput();
                if (!int.TryParse(input, out var inputResult)) continue;
                if (inputResult == 0) return false;
                foreach (var u in users)
                {
                    if (inputResult != u.UserID) continue;
                    user = u;
                    return true;
                }
            } while (true);
        }

        private bool TryGetTicketById(int id, TicketDBContext db, out Ticket ticket)
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
                if (result == null) return false;
                ticket = result;
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e.GetBaseException());
                PrintExceptionCaughtMessage();
            }
            
            return false;
        }

        private string GetUserInfo(string subHeader, string userRole = "", string infoType = "", string previousValue = "")
        {
            _display.Clear();
            PrintFullHeader(subHeader, "Enter " + userRole +" user's " + infoType.ToLower() + " (0 to cancel):");
            if (previousValue != null) _display.SendWait(previousValue);
            var input = _display.GetInput();
            return input == "0" ? null : input;
        }

        // add a user to the database from first and last name
        // accepts the database context and the strings for first and last names
        private void AddUser(out User user, string subHeader = "Add User", string fName = null, string lName = null)
        {
            user = null;
            
            try
            {
                if (fName == null && lName == null)
                {
                    var correct = false;
                    do
                    {
                        var name = GetUserInfo(subHeader, "", "first and last name");
                        if (name == null) return;
                        var nameArray = name.Trim().Split();
                        if (nameArray.Length != 2)
                            PrintInvalidInputMessage(
                                "Enter exactly one first name and one last name, separated by a space.");
                        else
                        {
                            fName = nameArray[0];
                            lName = nameArray[1];
                            correct = true;
                        }
                    } while (!correct);
                }

                var dept = GetUserInfo(subHeader,"", "department");
                if (dept == null)
                {
                    user = null;
                    return;
                }

                GetCorrectUserInput(subHeader, fName, lName, dept, out _);
            }
            catch (Exception e)
            {
                _logger.Error(e.GetBaseException());
                PrintExceptionCaughtMessage();
            }
            
            user = null;
        }

        private void UpdateUser()
        {
            using (var db = new TicketDBContext())
            {
                var subHeader = "Update Ticket";
                var valid = false;
                var done = false;
                User user;

                do
                {
                    var input = GetUserInfo(subHeader, "", "first and last name to update");
                    if (input == null) return;
                    var firstAndLastName = input.Trim().Split(' ');
                    if (firstAndLastName.Length != 2)
                        PrintInvalidInputMessage("Enter exactly one first name and one last name");
                    if (!TryGetUserByName(subHeader, firstAndLastName[0], firstAndLastName[1], db, out user))
                    {
                        if (user != null)
                        {
                            _display.Clear();
                            PrintFullHeader(subHeader, "User \"" + user.UserID + ") " + user.FirstName + " " + user.LastName + "\" was found, but is disabled.\nPlease re-enable user to use.");
                            _display.WriteLine();
                        }
                        else
                        {
                            continue;
                        }
                    }
                    valid = true;
                } while (!valid);
                do
                {
                    _display.Clear();
                    PrintFullHeader(subHeader);
                    _display.WriteLine("User ID: " + user.UserID);
                    _display.WriteLine("1) First Name: " + user.FirstName);
                    _display.WriteLine("2) Last Name: " + user.LastName);
                    _display.WriteLine("3) Department: " + user.Department);
                    _display.WriteLine("4) Enabled: " + (user.Enabled == 1 ? "Yes" : "No"));
                    _display.WriteSpecialLine();
                    _display.Write("Would you like to make a change?\n(enter 0 to accept): ");
                    var input = _display.GetInput();

                    switch (input)
                    {
                        case "0":
                            done = true;
                            db.SaveChanges();
                            break;
                        case "1": user.FirstName = GetUserInfo(subHeader, "", "first name", user.FirstName); break;
                        case "2": user.LastName = GetUserInfo(subHeader, "", "last name", user.LastName); break;
                        case "3": user.Department = GetUserInfo(subHeader, "", "department", user.Department); break;
                        case "4":
                            var enabled = GetUserInfo(subHeader, "", "enabled", (user.Enabled == 1 ? "Yes" : "No"));
                            if (enabled.ToLower() == "yes" || enabled.ToLower() == "no")
                                user.Enabled = enabled.ToLower() == "yes" ? 1 : 0;
                            break;
                        default: PrintInvalidInputMessage(input); break;
                    }
                } while (!done);
            }
        }

        private void GetCorrectUserInput(string subHeader, string fName, string  lName, string dept, out User user)
        {
            var done = false;
            do
            {
                _display.Clear();
                PrintFullHeader(subHeader);
                _display.WriteLine("1) " + fName + " " + lName);
                _display.WriteLine("2) " + dept);
                _display.WriteSpecialLine();
                _display.Write("Would you like to make a change?\n(enter 0 to accept): ");
                var input = _display.GetInput();
                switch (input)
                {
                    case "0":
                        done = true;
                        break;
                    case "1":
                        var name = GetUserInfo(subHeader, "", "first and last name", fName + " " + lName);
                        if (name == null)
                        {
                            user = null;
                            return;
                        }
                        var nameArray = name.Trim().Split();
                        if (nameArray.Length != 2) PrintInvalidInputMessage("Enter exactly one first name and one last name, separated by a space.");
                        fName = nameArray[0];
                        lName = nameArray[1];
                        continue;
                    case "2":
                        dept = GetUserInfo(subHeader, "", "department", dept);
                        continue;
                    default:
                        PrintInvalidInputMessage(input);
                        continue;
                }
            } while (!done);

            using (var db = new TicketDBContext())
            {
                if (TryGetUserByName(subHeader, fName, lName, db, out user))
                {
                    var valid = false;
                    do
                    {
                        _display.Clear();
                        PrintFullHeader(subHeader,
                            "That user's name already exist. Is this a new user with the same name?");
                        _display.Write("Y/N:");
                        var input = _display.GetInput();
                        switch (input.ToUpper())
                        {
                            case "Y":
                                valid = true;
                                break;
                            case "N": return;
                            default:
                                PrintInvalidInputMessage(input);
                                break;
                        }
                    } while (!valid);
                }

                user = new User {FirstName = fName, LastName = lName, Department = dept, Enabled = 1};
                db.Users.Add(user);
                db.SaveChanges();
            }

            _display.Clear();
            PrintFullHeader(subHeader, "New user \"" + user + "\" added successfully.");
            _display.WriteLine();
            PressToContinue();
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

        private void PrintFullHeader(string subHeader, string requestForInput = null)
        {
            _display.WriteSpecialLine();
            _display.WriteLine(Header);
            _display.WriteLine();
            _display.WriteLine(subHeader);
            _display.WriteSpecialLine();
            if (requestForInput == null) return;
            _display.WriteLine(requestForInput);
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