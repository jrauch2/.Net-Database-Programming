using System;

namespace Support_Ticket_System
{
    class TaskTicket : Ticket
    {
        
        public string ProjectName { get; set; }
        public DateTime DueDate { get; set; }


        public override void DisplayTicket()
        {
            throw new NotImplementedException();
        }
    }
}
