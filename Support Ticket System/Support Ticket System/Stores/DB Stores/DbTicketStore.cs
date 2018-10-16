using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Support_Ticket_System.Interfaces;
using Support_Ticket_System.Tickets;
using Support_Ticket_System.Utility;

namespace Support_Ticket_System.Stores.DB_Stores
{
    internal class DbTicketStore : IStore
    {
        private IDisplay _display;
        private const string InvalidTicketTypeMessage = "Invalid ticket type. Check for a ticket type and try again.";

        public DbTicketStore(ref IDisplay display)
        {
            _display = display;
        }

        public List<Ticket> GetAllTickets()
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
                    cmd.CommandText = "SELECT t.TicketID, t.Summary, t.Status, t.Priority, t.Submitter, su.FirstName as SubmitterFName, su.LastName as SubmitterLName, su.Department as SubmitterDepartment, su.Enabled as SubmitterEnabled, t.Assigned, au.FirstName as AssignedFName, au.LastName as AssignedLName, au.Department as AssignedDepartment, au.Enabled as AssignedEnabled, t.TicketTypeID, tt.Description, u.*, ba.BugAttrID, ba.Severity, ea.EnhanceAttrID, ea.Software, ea.Cost, ea.Reason, ea.Estimate, ta.TaskAttrID, ta.ProjectName, ta.DueDate" +
                                      "FROM Tickets t join Users su on su.UserID = t.Submitter" +
                                      "join Users au on au.UserID = t.Assigned" +
                                      "join TicketType tt on tt.TicketTypeID = t.TicketTypeID" +
                                      "join WatchingUsers wu on wu.TicketID = t.TicketID" +
                                      "join Users u on u.UserID = wu.UserID" +
                                      "left join bugattributes ba on t.TicketID = ba.TicketID" +
                                      "left join enhancementattributes ea on t.TicketID = ea.TicketID" +
                                      "left join taskattributes ta on t.TicketID = ta.TicketID";
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
                                submitter.Id = submitterId;
                                submitter.FName = dr["SubmitterFName"].ToString();
                                submitter.LName = dr["SubmitterLName"].ToString();
                                submitter.Department = dr["SubmitterDepartment"].ToString();
                                int.TryParse(dr["SubmitterEnabled"].ToString(), out var submitterEnabled);
                                submitter.Enabled = submitterEnabled;

                                var assigned = new User();
                                int.TryParse(dr["Assigned"].ToString(), out var assignedId);
                                assigned.Id = assignedId;
                                assigned.FName = dr["AssignedFName"].ToString();
                                assigned.LName = dr["AssignedLName"].ToString();
                                assigned.Department = dr["AssignedDepartment"].ToString();
                                int.TryParse(dr["AssignedEnabled"].ToString(), out var assignedEnabled);
                                assigned.Enabled = assignedEnabled;

                                var watcher = new User();
                                int.TryParse(dr["UserID"].ToString(), out var watcherId);
                                watcher.Id = watcherId;
                                watcher.FName = dr["FirstName"].ToString();
                                watcher.LName = dr["LastName"].ToString();
                                watcher.Department = dr["Department"].ToString();
                                int.TryParse(dr["Enabled"].ToString(), out var watcherEnabled);
                                watcher.Enabled = watcherEnabled;

                                watching.Add(watcher);

                                switch (type)
                                {
                                    case "Bug":
                                        record = new Bug(id, dr["Summary"].ToString(),
                                            dr["Status"].ToString().ToStatus(), dr["Priority"].ToString().ToPriority(),
                                            submitter, assigned, watching, dr["Severity"].ToString().ToSeverity(),
                                            ref _display);
                                        break;
                                    case "Enhancement":
                                        record = new Enhancement(id, dr["Summary"].ToString(),
                                            dr["Status"].ToString().ToStatus(), dr["Priority"].ToString().ToPriority(),
                                            submitter, assigned, watching, dr["Software"].ToString(),
                                            dr["Cost"].ToString(), dr["Reason"].ToString(), dr["Estimate"].ToString(),
                                            ref _display);
                                        break;
                                    case "Task":
                                        record = new Task(id, dr["Summary"].ToString(),
                                            dr["Status"].ToString().ToStatus(), dr["Priority"].ToString().ToPriority(),
                                            submitter, assigned, watching, dr["ProjectName"].ToString(),
                                            dr["DueDate"].ToString(), ref _display);
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
                    }
                }
            }
            // Read from our command and populate our list of objects
            return list;
        }

        public List<Ticket> Search()
        {
            throw new NotImplementedException();
        }

        public int GetMaxId()
        {
            throw new NotImplementedException();
        }

        public bool FindId(int id, out Ticket t)
        {
            throw new NotImplementedException();
        }

        public void AddTicket(Ticket ticket)
        {
            throw new NotImplementedException();
        }
    }
}
