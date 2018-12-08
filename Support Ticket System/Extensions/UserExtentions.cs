using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Support_Ticket_System
{
    public partial class User
    {
        public override string ToString()
        {
            return this.FirstName + " " + this.LastName;
        }
//
//        public override bool Equals(object obj)
//        {
//            if (obj == this)
//                return true;
//
//            if (obj?.GetType() != typeof(User))
//                return false;
//
//            var u = (User) obj;
//
//            return u.UserID == UserID;
//        }
//
//        public override int GetHashCode()
//        {
//            return UserID;
//        }
    }
}
