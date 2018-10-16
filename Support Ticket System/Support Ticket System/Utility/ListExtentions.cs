using System.Collections.Generic;

namespace Support_Ticket_System.Utility
{
    public static class ListExtensions
    {
        public static string ToDelimitedString(this IEnumerable<string> list, char delimiter)
        {
            var count = 0;
            var s = "";
            foreach (var v in list)
            {
                if (count++ == 0)
                {
                    s += v;
                }
                else
                {
                    s += delimiter + v;
                }
            }

            return s;
        }

        public static string ToFormattedString(this IEnumerable<string> list)
        {
            var count = 0;
            var s = "";

            foreach (var sl in list)
            {
                if (count++ == 0)
                {
                    s += sl;
                }
                else
                {
                    s += ", " + sl;
                }
            }

            return s;
        }

        public static string ToDelimitedString(this IEnumerable<User> list, char delimiter)
        {
            var count = 0;
            var s = "";

            foreach (var user in list)
            {
                if (count++ == 0)
                {
                    s += '\"' + user.Id + ',' + user.FName + ',' + user.LName + ',' + user.Department + ',' + user.Enabled + '\"';
                }
                else
                {
                    s += delimiter + '\"' + user.Id + ',' + user.FName + ',' + user.LName + ',' + user.Department + ',' + user.Enabled + '\"';
                }
            }

            return s;
        }

        public static string ToFormattedString(this IEnumerable<User> list)
        {
            var count = 0;
            var s = "";

            foreach (var user in list)
            {
                if (count++ == 0)
                {
                    s += user.FName + " " + user.LName;
                }
                else
                {
                    s += ", " + user.FName + " " + user.LName;
                }
            }

            return s;
        }
    }
}