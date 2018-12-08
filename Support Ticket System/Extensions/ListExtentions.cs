using System.Collections.Generic;

namespace Support_Ticket_System.Utility
{
    public static class ListExtensions
    {
        public static string ToDelimitedString<T>(this IEnumerable<T> list, char delimiter)
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
                    s += delimiter.ToString() + v;
                }
            }

            return s;
        }

        public static string ToFormattedString<T>(this IEnumerable<T> list)
        {
            var count = 0;
            var s = "";

            foreach (var sl in list)
            {
                if (count++ == 0)
                {
                    s += sl.ToString();
                }
                else
                {
                    s += ", " + sl;
                }
            }

            return s;
        }
    }
}