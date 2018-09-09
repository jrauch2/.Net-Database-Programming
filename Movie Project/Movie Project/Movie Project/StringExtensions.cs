using System;
using System.Linq;

namespace Movie_Project
{
    /// <summary>
    /// The <c>StringExtensions</c> class.
    /// Add methods here to extend the <c>String</c> class.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Make the first character in a string uppercase.
        /// </summary>
        /// <param name="input">The string to capitalize.</param>
        /// <returns>A capitalized string.</returns>
        // Make the first character in a string uppercase.
        public static string FirstCharToUpper(this string input)
        {
            switch (input)
            {
                case null:
                    throw new ArgumentNullException(nameof(input));
                case "":
                    throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default:
                    return input.First().ToString().ToUpper() + input.Substring(1);
            }
        }
    }
}