using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Support_Ticket_System
{
    public class TicketResult
    {
        public int? TicketID { get; set; }
        public string Summary { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public int? SubmitterID { get; set; }
        public string SubmitterFirstName { get; set; }
        public string SubmitterLastName { get; set; }
        public string SubmitterDepartment { get; set; }
        public int? SubmitterEnabled { get; set; }
        public int? AssignedID { get; set; }
        public string AssignedFirstName { get; set; }
        public string AssignedLastName { get; set; }
        public string AssignedDepartment { get; set; }
        public int? AssignedEnabled { get; set; }
//        public int? WatchingID { get; set; }
//        public string WatchingFirstName { get; set; }
//        public string WatchingLastName { get; set; }
//        public string WatchingDepartment { get; set; }
//        public int? WatchingEnabled { get; set; }
        public string TicketType { get; set; }
        public int? BugAttrID { get; set; }
        public string Severity { get; set; }
        public int? EnhanceAttrID { get; set; }
        public string Cost { get; set; }
        public string Estimate { get; set; }
        public string Reason { get; set; }
        public string Software { get; set; }
        public int? TaskAttrID { get; set; }
        public string ProjectName { get; set; }
        public string DueDate { get; set; }

        public List<WatchingUser> WatchingUsers { get; set; }
    }
}
