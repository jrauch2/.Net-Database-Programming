using System.Collections.Generic;

namespace Class_Project
{
    internal interface IOutput
    {
        /// <summary>
        /// Write out <c>List</c> of <c>Ticket</c> objects to storage medium.
        /// </summary>
        /// <param name="tickets">A <c>List</c> of <c>Ticket</c> objects</param>
        void WriteAll(List<Ticket> tickets);
    }
}