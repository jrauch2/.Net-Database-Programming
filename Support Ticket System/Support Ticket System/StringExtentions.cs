using System;
using System.Collections.Generic;
using System.Linq;

namespace Support_Ticket_System
{
    /// <summary>
    /// The <c>StringExtensions</c> class.
    /// Add methods here to extend the <c>String</c> class.
    /// </summary>
    public static class StringExtensions
    {
        public static int ToInt(this string s)
        {
            int.TryParse(s, out var i);
            return i;
        }

        public static double ToDouble(this string s)
        {
            double.TryParse(s, out var d);
            return d;
        }

        /// <summary>
        /// Parse a <c>string</c> to a <c>Status</c>.
        /// </summary>
        /// <param name="statusString">The <c>string</c> to be parsed.</param>
        /// <returns>A <c>Status</c>.</returns>
        public static Status ToStatus(this string statusString)
        {
            Enum.TryParse<Status>(statusString, true, out var status);
            return status;
        }

        /// <summary>
        /// Parse a <c>string</c> to a <c>Priority</c>.
        /// </summary>
        /// <param name="priorityString">The <c>string</c> to be parsed.</param>
        /// <returns>A <c>Priority</c>.</returns>
        public static Priority ToPriority(this string priorityString)
        {
            Enum.TryParse<Priority>(priorityString, true, out var priority);
            return priority;
        }

        public static Severity ToSeverity(this string severityString)
        {
            Enum.TryParse<Severity>(severityString, true, out var severity);
            return severity;
        }

        public static DateTime ToDateTime(this string dateTimeString)
        {
            DateTime.TryParse(dateTimeString, out var dateTime);
            return dateTime;
        }

        public static List<string> ToStringList(this string s)
        {
            var list = new List<string>();

            var sa = s.Split();

            if (!sa.Any())
            {
                return list;
            }
            else
            {
                for (var i = 0; i < sa.Length; i++)
                {
                    sa[i] = sa[i].Trim();
                    list.Add(sa[i]);
                }

                return list;
            }
        }

    }
}
