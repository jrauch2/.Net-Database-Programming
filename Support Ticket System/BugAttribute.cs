//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Support_Ticket_System
{
    using System;
    using System.Collections.Generic;
    
    public partial class BugAttribute
    {
        public int BugAttrID { get; set; }
        public int TicketID { get; set; }
        public string Severity { get; set; }
    
        public virtual Ticket Ticket { get; set; }
    }
}
