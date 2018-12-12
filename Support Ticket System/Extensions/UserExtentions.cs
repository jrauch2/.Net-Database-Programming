using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Windows.Forms;
using Support_Ticket_System.Interfaces;

namespace Support_Ticket_System
{
    public partial class User
    {
        public override string ToString()
        {
            return this.FirstName + " " + this.LastName;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj == this) return true;
            if (obj.GetType() != this.GetType()) return false;

            var u = (User)obj;

            return UserID == u.UserID;
        }

        public override int GetHashCode()
        {
            return UserID;
        }

        public void Print(IDisplay display)
        {
            display.WriteLine("User ID: " + UserID);
            display.WriteLine(FirstName + " " + LastName);
            display.WriteLine("Department: " + Department);
            display.WriteLine("Enabled: " + (Enabled == 1 ? "Yes" : "No"));
            display.WriteSpecialLine();
        }
    }
}
