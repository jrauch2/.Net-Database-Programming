using System.ComponentModel.DataAnnotations.Schema;

namespace Database_Functions
{
    [Table("Tickets")]
    public class TicketEntity
    {
        public int TicketId { get; set; }
        public string Summary { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public string Submitter { get; set; }
        public string Assigned { get; set; }
        public string Watching { get; set; }
    }
}