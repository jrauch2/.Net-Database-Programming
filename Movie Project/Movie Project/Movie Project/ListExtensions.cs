using System.Collections.Generic;

namespace Movie_Project
{
    public static class ListExtensions
    {
        public static string ToDelimitedString(this List<string> list, char delimiter)
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

        public static string ToFormattedString(this List<string> list)
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
    }
}