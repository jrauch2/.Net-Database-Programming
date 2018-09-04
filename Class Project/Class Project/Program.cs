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
        private static 
        private const string FiftyStarLine = "**************************************************";

        public static void Main(string[] args)
        {
            Menu();
        }

        public static void Menu()
        {
            Console.WriteLine(FiftyStarLine);
            Console.WriteLine("*              Support Ticket System             *");
            Console.WriteLine(FiftyStarLine);
            Console.WriteLine("* 1) New Ticket                                  *");
            Console.WriteLine("* 2) Update Ticket                               *");
            Console.WriteLine("* 3) Print All Tickets                           *");
            Console.WriteLine("* 4) Exit                                        *");
            Console.WriteLine(FiftyStarLine);
            Console.Write("Select an option: ");

            string input = Console.ReadLine();
            Console.Clear();
            switch (input)
            {
                case "1":
                    NewTicket();
                    break;
                case "2":
                    UpdateTicket();
                    break;
                case "3":
                    PrintAllTickets();
                    break;
                case "4":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Not a valid option, try again.");
                    break;
            }
        }

        public static void NewTicket()
        {
            //TODO
        }

        public static void UpdateTicket()
        {
            //TODO
        }

        public static void PrintAllTickets()
        {
            //TODO
        }
    }
}
