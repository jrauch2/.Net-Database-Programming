using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using NLog;
using Support_Ticket_System.Interfaces;
using Support_Ticket_System.Tickets;
using Support_Ticket_System.Utility;

namespace Support_Ticket_System.Stores
{
    internal class DbTicketStore
    {
        private readonly Logger _logger;
        private readonly IDisplay _display;
        private const string InvalidTicketTypeMessage = "Invalid ticket type. Check for a ticket type and try again.";

        public DbTicketStore(ref IDisplay display)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _display = display;
        }

        private List<Ticket> GetTickets(string commandText)
        {
            var list = new List<Ticket>();

            // Connect to the DB
            using (var conn = new SqlConnection(Properties.Settings.Default.TicketDBConnStr))
            {
                // open the connection
                conn.Open();
                // Create our command
                using (var cmd = new SqlCommand())
                {
                    // assign the connection
                    cmd.Connection = conn;
                    cmd.CommandText = commandText;
                    using (var dr = cmd.ExecuteReader())
                    {
                        var prevTicketId = "";
                        Ticket record = null;

                        while (dr.Read())
                        {
                            if (prevTicketId != dr["TicketId"].ToString())
                            {
                                if (prevTicketId != "")
                                {
                                    list.Add(record);
                                }

                                var watching = new List<User>();
                                var type = dr["TicketType"].ToString();
                                int.TryParse(dr["TicketID"].ToString(), out var id);

                                var submitter = new User();
                                int.TryParse(dr["Submitter"].ToString(), out var submitterId);
                                submitter.UserID = submitterId;
                                submitter.FirstName = dr["SubmitterFName"].ToString();
                                submitter.LastName = dr["SubmitterLName"].ToString();
                                submitter.Department = dr["SubmitterDepartment"].ToString();
                                int.TryParse(dr["SubmitterEnabled"].ToString(), out var submitterEnabled);
                                submitter.Enabled = submitterEnabled;

                                var assigned = new User();
                                int.TryParse(dr["Assigned"].ToString(), out var assignedId);
                                assigned.UserID = assignedId;
                                assigned.FirstName = dr["AssignedFName"].ToString();
                                assigned.LastName = dr["AssignedLName"].ToString();
                                assigned.Department = dr["AssignedDepartment"].ToString();
                                int.TryParse(dr["AssignedEnabled"].ToString(), out var assignedEnabled);
                                assigned.Enabled = assignedEnabled;

                                var watcher = new User();
                                int.TryParse(dr["UserID"].ToString(), out var watcherId);
                                watcher.UserID = watcherId;
                                watcher.FirstName = dr["FirstName"].ToString();
                                watcher.LastName = dr["LastName"].ToString();
                                watcher.Department = dr["Department"].ToString();
                                int.TryParse(dr["Enabled"].ToString(), out var watcherEnabled);
                                watcher.Enabled = watcherEnabled;

                                watching.Add(watcher);

                                switch (type)
                                {
                                    case "Bug":
                                        record = new Bug
                                        {
                                            Id =id,
                                            Summary = dr["Summary"].ToString(),
                                            Status = dr["Status"].ToString().ToStatus(),
                                            Priority = dr["Priority"].ToString().ToPriority(),
                                            Submitter = submitter,
                                            Assigned = assigned,
                                            Watching = watching,
                                            Severity = dr["Severity"].ToString().ToSeverity(),
                                            Display = _display

                                        };
                                        break;
                                    case "Enhancement":
                                        record = new Enhancement
                                        {
                                            Id = id,
                                            Summary = dr["Summary"].ToString(),
                                            Status = dr["Status"].ToString().ToStatus(),
                                            Priority = dr["Priority"].ToString().ToPriority(),
                                            Submitter = submitter,
                                            Assigned = assigned,
                                            Watching = watching,
                                            Software = dr["Software"].ToString(),
                                            Cost = dr["Cost"].ToString(),
                                            Reason = dr["Reason"].ToString(),
                                            Estimate = dr["Estimate"].ToString(),
                                            Display = _display

                                        };
                                        break;
                                    case "Task":
                                        record = new Task
                                        {
                                            Id = id,
                                            Summary = dr["Summary"].ToString(),
                                            Status = dr["Status"].ToString().ToStatus(),
                                            Priority = dr["Priority"].ToString().ToPriority(),
                                            Submitter = submitter,
                                            Assigned = assigned,
                                            Watching = watching,
                                            ProjectName = dr["ProjectName"].ToString(),
                                            DueDate = dr["DueDate"].ToString(),
                                            Display = _display

                                        };
                                        break;
                                    default:
                                        _display.WriteLine(InvalidTicketTypeMessage);
                                        break;
                                }

                                prevTicketId = dr["TicketId"].ToString();
                            }
                            else
                            {
                                var watcher = new User();
                                int.TryParse(dr["UserID"].ToString(), out var watcherId);
                                watcher.Id = watcherId;
                                watcher.FName = dr["FirstName"].ToString();
                                watcher.LName = dr["LastName"].ToString();
                                watcher.Department = dr["Department"].ToString();
                                int.TryParse(dr["Enabled"].ToString(), out var watcherEnabled);
                                watcher.Enabled = watcherEnabled;

                                record?.AddWatching(watcher);
                            }
                        }

                        list.Add(record);
                    }
                }
            }
            // Read from our command and populate our list of objects
            return list;
        }

        public List<Ticket> GetAllTickets()
        {
            const string commandText = "SELECT t.TicketID, t.Summary, t.Status, t.Priority, t.Submitter, su.FirstName as SubmitterFName, su.LastName as SubmitterLName, su.Department as SubmitterDepartment, su.Enabled as SubmitterEnabled, t.Assigned, au.FirstName as AssignedFName, au.LastName as AssignedLName, au.Department as AssignedDepartment, au.Enabled as AssignedEnabled, t.TicketTypeID, tt.Description as TicketType, u.*, ba.BugAttrID, ba.Severity, ea.EnhanceAttrID, ea.Software, ea.Cost, ea.Reason, ea.Estimate, ta.TaskAttrID, ta.ProjectName, ta.DueDate " +
                                       "FROM Tickets t join Users su on su.UserID = t.Submitter " +
                                       "join Users au on au.UserID = t.Assigned " +
                                       "join TicketType tt on tt.TicketTypeID = t.TicketTypeID " +
                                       "join WatchingUsers wu on wu.TicketID = t.TicketID " +
                                       "join Users u on u.UserID = wu.UserID " +
                                       "left join BugAttributes ba on t.TicketID = ba.TicketID " +
                                       "left join EnhancementAttributes ea on t.TicketID = ea.TicketID " +
                                       "left join TaskAttributes ta on t.TicketID = ta.TicketID ";
            return GetTickets(commandText);
        }

        public List<Ticket> Search(string searchString)
        {
            if (int.TryParse(searchString, out var searchInt))
            {


                var commandText = "SELECT t.TicketID, t.Summary, t.Status, t.Priority, t.Submitter, " +
                                  "su.FirstName as SubmitterFName, su.LastName as SubmitterLName, " +
                                  "su.Department as SubmitterDepartment, su.Enabled as SubmitterEnabled, " +
                                  "t.Assigned, au.FirstName as AssignedFName, au.LastName as AssignedLName, " +
                                  "au.Department as AssignedDepartment, au.Enabled as AssignedEnabled, " +
                                  "t.TicketTypeID, tt.Description, u.*, ba.BugAttrID, ba.Severity, " +
                                  "ea.EnhanceAttrID, ea.Software, ea.Cost, ea.Reason, ea.Estimate, " +
                                  "ta.TaskAttrID, ta.ProjectName, ta.DueDate " +
                                  "FROM Tickets t join Users su on su.UserID like t.Submitter " +
                                  "join Users au on au.UserID like t.Assigned " +
                                  "join TicketType tt on tt.TicketTypeID like t.TicketTypeID " +
                                  "join WatchingUsers wu on wu.TicketID like t.TicketID " +
                                  "join Users u on u.UserID like wu.UserID " +
                                  "left join BugAttributes ba on t.TicketID like ba.TicketID " +
                                  "left join EnhancementAttributes ea on t.TicketID like ea.TicketID " +
                                  "left join TaskAttributes ta on t.TicketID like ta.TicketID " +
                                  "where t.TicketID like '" + searchInt + "' or t.Summary like '" + searchString +
                                  "' or t.Status like '" + searchString + "' or t.Priority like '" + searchString +
                                  "' or t.Submitter like '" + searchString + "' or SubmitterFName like '" + searchString +
                                  "' or SubmitterLName like '" + searchString + "' or SubmitterDepartment like '" +
                                  searchString + "' or SubmitterEnabled like '" + searchString + "' or t.Assigned like '" +
                                  searchString + "' or AssignedFName like '" + searchString + "' or AssignedLName like '" +
                                  searchString + "' or AssignedDepartment like '" + searchString +
                                  "' or AssignedEnabled like '" + searchString + "' or t.TicketTypeID like '" + searchInt +
                                  "' or tt.Description like '" + searchString + "' or u.UserID like '" + searchInt +
                                  "' or u.FirstName like '" + searchString + "' or u.LastName like '" + searchString +
                                  "' or u.Department like '" + searchString + "' or u.Enabled like '" + searchString +
                                  "' or ba.BugAttrID like '" + searchString + "' or ba.Severity like '" + searchString +
                                  "' or ea.EnhanceAttrID like '" + searchString + "' or ea.Software like '" +
                                  searchString + "' or ea.Cost like '" + searchString + "' or ea.Reason like '" +
                                  searchString + "' or ea.Estimate like '" + searchString + "' or ta.TaskAttrID like '" +
                                  searchString + "' or ta.ProjectName like '" + searchString + "' or ta.DueDate like '" +
                                  searchString + "'";
                return GetTickets(commandText);
            }
            else
            {
                var commandText =
                    "SELECT t.TicketID, t.Summary, t.Status, t.Priority, t.Submitter, su.FirstName as SubmitterFName, su.LastName as SubmitterLName, su.Department as SubmitterDepartment, su.Enabled as SubmitterEnabled, t.Assigned, au.FirstName as AssignedFName, au.LastName as AssignedLName, au.Department as AssignedDepartment, au.Enabled as AssignedEnabled, t.TicketTypeID, tt.Description, u.*, ba.BugAttrID, ba.Severity, ea.EnhanceAttrID, ea.Software, ea.Cost, ea.Reason, ea.Estimate, ta.TaskAttrID, ta.ProjectName, ta.DueDate " +
                    "FROM Tickets t join Users su on su.UserID like t.Submitter " +
                    "join Users au on au.UserID like t.Assigned " +
                    "join TicketType tt on tt.TicketTypeID like t.TicketTypeID " +
                    "join WatchingUsers wu on wu.TicketID like t.TicketID " +
                    "join Users u on u.UserID like wu.UserID " +
                    "left join BugAttributes ba on t.TicketID like ba.TicketID " +
                    "left join EnhancementAttributes ea on t.TicketID like ea.TicketID " +
                    "left join TaskAttributes ta on t.TicketID like ta.TicketID " +
                    "where t.Summary like '" + searchString + "' " +
                    "or t.Status like '" + searchString + "' or t.Priority like '" + searchString + "' " +
                    "or su.FirstName like '" + searchString + "' " +
                    "or su.LastName like '" + searchString + "' or su.Department like '" + searchString + "' " +
                    "or au.FirstName like '" + searchString + "' or au.LastName like '" + searchString + "' " +
                    "or au.Department like '" + searchString + "' " +
                    "or tt.Description like '" + searchString + "' " +
                    "or u.FirstName like '" + searchString + "' or u.LastName like '" + searchString + "' " +
                    "or u.Department like '" + searchString + "' " +
                    "or ba.Severity like '" + searchString + "' " +
                    "or ea.Software like '" + searchString + "' " +
                    "or ea.Cost like '" + searchString + "' or ea.Reason like '" + searchString + "' " +
                    "or ea.Estimate like '" + searchString + "' " +
                    "or ta.ProjectName like '" + searchString + "' or ta.DueDate like '" + searchString + "' ";
                return GetTickets(commandText);
            }
        }

        public bool FindId(int id, out Ticket t)
        {
            var commandText = "SELECT t.TicketID, t.Summary, t.Status, t.Priority, t.Submitter, su.FirstName as SubmitterFName, su.LastName as SubmitterLName, su.Department as SubmitterDepartment, su.Enabled as SubmitterEnabled, t.Assigned, au.FirstName as AssignedFName, au.LastName as AssignedLName, au.Department as AssignedDepartment, au.Enabled as AssignedEnabled, t.TicketTypeID, tt.Description, u.*, ba.BugAttrID, ba.Severity, ea.EnhanceAttrID, ea.Software, ea.Cost, ea.Reason, ea.Estimate, ta.TaskAttrID, ta.ProjectName, ta.DueDate " +
                                       "FROM Tickets t join Users su on su.UserID = t.Submitter " +
                                       "join Users au on au.UserID = t.Assigned " +
                                       "join TicketType tt on tt.TicketTypeID = t.TicketTypeID " +
                                       "join WatchingUsers wu on wu.TicketID = t.TicketID " +
                                       "join Users u on u.UserID = wu.UserID " +
                                       "left join BugAttributes ba on t.TicketID = ba.TicketID " +
                                       "left join EnhancementAttributes ea on t.TicketID = ea.TicketID " +
                                       "left join TaskAttributes ta on t.TicketID = ta.TicketID " +
                                       "where t.TicketID = " + id;
            
            var list = GetTickets(commandText);
            t = list[0];
            return list[0] != null;
        }

        public void AddTicket(Ticket ticket)
        {
            try
            {
                using (var conn = new SqlConnection(Properties.Settings.Default.TicketDBConnStr))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;

                        var insertSql = "declare @ticket_id int "
                            + "insert into Tickets "
                            + "values (@summary, @status, @priority, @submitter, @assigned, @ticketTypeID) "
                            + "select @ticket_id = SCOPE_IDENTITY() "
                            + "insert into WatchingUsers "
                            + "select @ticket_id, UserID "
                            + "from users "
                            + "where description in ({0}) ";

                        cmd.Parameters.AddWithValue("@summary", ticket.Summary);
                        cmd.Parameters.AddWithValue("@status", ticket.Status);
                        cmd.Parameters.AddWithValue("@priority", ticket.Priority);
                        cmd.Parameters.AddWithValue("@submitter", ticket.Submitter.Id);
                        cmd.Parameters.AddWithValue("@assigned", ticket.Assigned.Id);

                        var values = new List<string>();
                        for (var i = 0; i < ticket.Watching.Count; i++)
                        {
                            values.Add($"@p{i}");
                        }
                        var inClause = String.Join(",", values);

                        insertSql = String.Format(insertSql, inClause);

                        for (var i = 0; i < ticket.Watching.Count; i++)
                        {
                            cmd.Parameters.AddWithValue(values[i], ticket.Watching[i].Id);
                        }

                        if (ticket.TicketType == typeof(Bug))
                        {
                            cmd.Parameters.AddWithValue("@ticketTypeID", 1);
                            var newBug = (Bug)ticket;
                            insertSql += "; INSERT INTO BugAttribute "
                                         + "values (@ticket_id, @severity)";
                            cmd.Parameters.AddWithValue("@severity", newBug.Severity);
                        }
                        else if (ticket.TicketType == typeof(Enhancement))
                        {
                            cmd.Parameters.AddWithValue("@ticketTypeID", 2);
                            var newEnhancement = (Enhancement)ticket;
                            insertSql += "; INSERT INTO EnhancementAttribute "
                                         + "values (@ticket_id, @software, @cost, @reason, #estimate)";
                            cmd.Parameters.AddWithValue("@software", newEnhancement.Software);
                            cmd.Parameters.AddWithValue("@cost", newEnhancement.Cost);
                            cmd.Parameters.AddWithValue("@reason", newEnhancement.Reason);
                            cmd.Parameters.AddWithValue("@estimate", newEnhancement.Estimate);
                        }
                        else if (ticket.TicketType == typeof(Task))
                        {

                            cmd.Parameters.AddWithValue("@ticketTypeID", 3);
                            var newTask = (Task)ticket;
                            insertSql += "; INSERT INTO BugAttribute "
                                         + "values (@ticket_id, @projectName, @dueDate)";
                            cmd.Parameters.AddWithValue("@projectName", newTask.ProjectName);
                            cmd.Parameters.AddWithValue("@dueDate", newTask.DueDate);
                        }

                        cmd.CommandText = insertSql;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                _logger.Fatal(e);
            }
        }

        private void AddUser(User user)
        {
            try
            {
                using (var conn = new SqlConnection(Properties.Settings.Default.TicketDBConnStr))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;

                        var insertSql = "insert into Users "
                                        + "values (@firstName, @lastName, @department, @enabled) ";
                            

                        cmd.Parameters.AddWithValue("@firstName", user.FName);
                        cmd.Parameters.AddWithValue("@lastName", user.LName);
                        cmd.Parameters.AddWithValue("@department", user.Department);
                        cmd.Parameters.AddWithValue("@enabled", user.Enabled);

                        cmd.CommandText = insertSql;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                _logger.Fatal(e);
            }
        }

        public User GetUserByName(string fName, string lName)
        {
            while (true)
            {
                var users = new List<User>();
                User user;
                // Connect to the DB
                using (var conn = new SqlConnection(Properties.Settings.Default.TicketDBConnStr))
                {
                    // open the connection
                    conn.Open();
                    // Create our command
                    using (var cmd = new SqlCommand())
                    {
                        // assign the connection
                        cmd.Connection = conn;
                        cmd.CommandText = "select * from users where FirstName = '" + fName + "' and LastName = '" + lName + "'";
                        using (var dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                if (!int.TryParse(dr["UserID"].ToString(), out var id)) continue;
                                user = new User
                                {
                                    Id = id,
                                    FName = dr["FirstName"].ToString(),
                                    LName = dr["LastName"].ToString(),
                                    Department = dr["Department"].ToString(),
                                    Enabled = int.Parse(dr["Enabled"].ToString())
                                };
                                users.Add(user);
                            }
                        }
                    }
                }

                switch (users.Count)
                {
                    case 0:
                        user = new User {FName = fName, LName = lName};
                        AddUser(user);
                        break;
                    case 1:
                        return users[0];
                    default:
                    {
                        if (users.Count > 1)
                        {
                            _display.WriteLine("More than 1 result found");
                            _display.WriteLine("Select which user: ");
                            foreach (var user1 in users)
                            {
                                _display.WriteLine(user1.Id + ") " + user1.FName + " " + user1.LName + ", " + user1.Department);
                            }

                            _display.WriteLine("0) Add New User");
                            var input = _display.GetInput();
                            User selectedUser = null;
                            if (int.TryParse(input, out var inputInt))
                            {
                                if (inputInt == 0)
                                {
                                    _display.Write("Enter the department: ");
                                    user = new User {FName = fName, LName = lName, Department = _display.GetInput()};
                                    AddUser(user);
                                }
                                else
                                {
                                    foreach (var user1 in users)
                                    {
                                        if (inputInt == user1.Id)
                                        {
                                            selectedUser = user1;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                _display.WriteLine("Invalid option.");
                            }

                            return selectedUser;
                        }

                        break;
                    }
                }
            }
        }
    }
}