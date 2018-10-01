using System;
using NLog;

namespace Support_Ticket_System
{
    internal static class Program
    {
        // Logger for the Program class.
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        // Display for the program.
        private static IDisplay _consoleDisplay;
        // Support System Application instance.
        private static SupportTicketSystem _supportTicketSystem;

        public static void Main()
        {
            Logger.Trace("Starting application...");
            try
            {
                _consoleDisplay = new ConsoleDisplay();
                Logger.Trace("Starting the Support Ticket System...");
                _supportTicketSystem = new SupportTicketSystem(_consoleDisplay);
                _supportTicketSystem.Start();
                Logger.Trace("...Support Ticket System Started.");
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }
    }
}
