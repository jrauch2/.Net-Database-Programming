using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Support_Ticket_System
{
    public class User
    {
        public int Id { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Department { get; set; }
        public int Enabled { get; set; }
    }
}
