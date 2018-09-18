using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Support_Ticket_System
{
    [Table("Tickets")]
    public class TicketEntity
    {
        [Key]
        public int TicketId { get; set; }
        public string Summary { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public string Submitter { get; set; }
        public string Assigned { get; set; }
        public string Watching { get; set; }

        public override string ToString()
        {
            return TicketId.ToString() + ",\"" + Summary + "\"," + Status + "," + Priority + "," + Submitter + "," + Assigned + "," + Watching;
        }
    }
}