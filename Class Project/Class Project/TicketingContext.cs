using System.Data.Entity;

namespace Database_Functions
{
    class TicketingContext : DbContext
    {
        public DbSet<TicketEntity> Tickets { get; set; }
    }
}