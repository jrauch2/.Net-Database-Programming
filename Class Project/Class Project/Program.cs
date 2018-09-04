using System;
using System.Collections.Generic;
using System.ComponentModel.Design;


namespace Class_Project
{
    internal class Program
    {
        private const string fileName = "support_tickets.csv";
        private static IInput csvIn = new CsvIn(fileName);
        private static IOutput csvOut = new CsvOut(fileName);
        private static TicketFactory ticketFactory = TicketFactory.GetTicketFactory(csvIn.GetMaxId());
        private const string FiftyStarLine = " ************************************************** ";
        private const string Header = " *              Support Ticket System             * ";
        private const string NewTicketHeader = " *                   New Ticket                   * ";
        private const int LineLength = 50;

        public static void Main(string[] args)
        {
            Menu();
        }

        private static void Menu()
        {
            bool correct = false;

            do
            {
                Console.SetWindowSize(52, 30);
                Console.WriteLine(FiftyStarLine);
                Console.WriteLine(Header);
                Console.WriteLine(FiftyStarLine);
                Console.WriteLine(" * 1) New Ticket                                  * ");
                Console.WriteLine(" * 2) Update Ticket                               * ");
                Console.WriteLine(" * 3) Print All Tickets                           * ");
                Console.WriteLine(" * 4) Exit                                        * ");
                Console.WriteLine(FiftyStarLine);
                Console.Write("Select an option: ");

                string input = Console.ReadLine();
                Console.Clear();
                switch (input)
                {
                    case "1":
                        correct = true;
                        NewTicket();
                        break;
                    case "2":
                        correct = true;
                        UpdateTicket();
                        break;
                    case "3":
                        correct = true;
                        PrintAllTickets();
                        break;
                    case "4":
                        correct = true;
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Not a valid option, try again.");
                        break;
                }
            } while (!correct);

        }

        //Ask the user for input to generate a new ticket.
        private static void NewTicket()
        {
            string summary = GetSummaryInput();
            Console.Clear();

            Priority priority= GetPriorityInput();
            Console.Clear();

            string submitter = GetSubmitterInput();
            Console.Clear();
            
            GetCorrectInput(summary, priority, submitter);
        }

        private static void UpdateTicket()
        {
            //TODO
        }

        private static void PrintAllTickets()
        {
            //TODO
        }

        //Ask user for summary.
        private static string GetSummaryInput()
        {
            Console.WriteLine(FiftyStarLine);
            Console.WriteLine(Header);
            Console.WriteLine(NewTicketHeader);
            Console.WriteLine(FiftyStarLine);
            Console.WriteLine(" * Write a summary of the issue (254 Char Max):   * ");
            Console.WriteLine(FiftyStarLine);
            return Console.ReadLine();
        }

        //Ask user for Priority.
        private static Priority GetPriorityInput()
        {
            bool correct = false;
            Priority priority = Priority.LOW;
            do
            {
                correct = false;
                Console.WriteLine(FiftyStarLine);
                Console.WriteLine(Header);
                Console.WriteLine(NewTicketHeader);
                Console.WriteLine(FiftyStarLine);
                Console.WriteLine(" * Set a priority (LOW, MEDIUM, HIGH, SEVERE):    * ");
                Console.WriteLine(FiftyStarLine);
                var priorityInput = Console.ReadLine();
                if (priorityInput.ToUpper().Equals("LOW") || priorityInput.ToUpper().Equals("MEDIUM") || 
                    priorityInput.ToUpper().Equals("HIGH") || priorityInput.ToUpper().Equals("SEVERE"))
                {
                    priority = Conversion.StringToPriority(priorityInput);
                    correct = true;
                }
                else
                {
                    Console.WriteLine("Invalid input. Try again.");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadLine();
                }
                Console.Clear();
            } while (!correct);

            return priority;
        }

        //Ask user for name (submitter).
        private static string GetSubmitterInput()
        {   
            Console.WriteLine(FiftyStarLine);
            Console.WriteLine(Header);
            Console.WriteLine(NewTicketHeader);
            Console.WriteLine(FiftyStarLine);
            Console.WriteLine(" * Enter your name (submitter):                   * ");
            Console.WriteLine(FiftyStarLine);
            return Console.ReadLine();
        }

        //Ask the user to confirm that the information is correct
        private static void GetCorrectInput(string summary, Priority priority, string submitter)
        {
            bool correct = false;

            do
            {
                string s;
                Console.WriteLine(FiftyStarLine);
                Console.WriteLine(Header);
                Console.WriteLine(NewTicketHeader);
                Console.WriteLine(FiftyStarLine);
                s = (" * 1) Summary:".PadRight(LineLength - 2) + "* ");
                Console.WriteLine(s);
                WriteSummary(summary, LineLength);
                s = (" * 2) Priority: " + priority.ToString()).PadRight(LineLength - 2) + "* ";
                Console.WriteLine(s);
                s = (" * 3) Submitter: " + submitter).PadRight(LineLength - 2) + "* ";
                Console.WriteLine(s);
                Console.WriteLine(FiftyStarLine);
                Console.Write("Would you like to make a change?\n(enter 0 to accept): ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "0":
                        correct = true;
                        ticketFactory
                        break;
                    case "1":
                        correct = true;
                        summary = GetSummaryInput();
                        break;
                    case "2":
                        correct = true;
                        priority = GetPriorityInput();
                        break;
                    case "3":
                        correct = true;
                        submitter = GetSubmitterInput();
                        break;
                    default:
                        Console.WriteLine("Invalid input. Try again.");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadLine();
                        break;
                }

                Console.Clear();
            } while (!correct);
        }

        //Write the summary within the formatted area.
        private static void WriteSummary(string summary, int lineLength)
        {
            string[] stringArray = WordWrap.Wrap(summary, lineLength - 4).Split('|');
            for (var i = 0; i < stringArray.Length; i++)
            {
                stringArray[i] = stringArray[i].PadRight(lineLength - 2) + "* ";
                Console.WriteLine(stringArray[i]);
            }
        }
    }
}
