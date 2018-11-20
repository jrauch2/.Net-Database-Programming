using System;
using System.Windows.Forms;
using Support_Ticket_System.Interfaces;
using Support_Ticket_System.Utility;

namespace Support_Ticket_System
{
    class ConsoleDisplay : IDisplay
    {
        private char _specialCharacter;
        public int DisplayHeight { get; set; }
        public int DisplayWidth { get; set; }
        public string LeftPadding { get; set; }
        public string RightPadding { get; set; }
        public char SpecialCharacter {
            get => _specialCharacter;
            set
            {
                _specialCharacter = value;
                LeftPadding = " " + value + " ";
                RightPadding = value + " ";
            }
        }


        public void Write(string message)
        {
            Console.Write(message);
        }

        public void WriteLine(string message)
        {
            if (message.Length < DisplayWidth - (LeftPadding.Length + RightPadding.Length))
            {
                Console.WriteLine(Format(message));
            }
            else
            {
                var stringArray = WordWrap.Wrap(message, DisplayWidth - (LeftPadding.Length + RightPadding.Length))
                    .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var s in stringArray)
                {
                    Console.WriteLine(Format(s));
                }
            }
        }

        public void WriteSpecialLine()
        {
            for (var i = 0; i < DisplayWidth; i++)
            {
                if (i == 0 || i == DisplayWidth - 1)
                {
                    Console.Write(" ");
                }
                else
                {
                    Console.Write(SpecialCharacter);
                }
            }
            Console.WriteLine();
        }

        public void SetWindowSize(int displayWidth, int displayHeight)
        {
            DisplayWidth = displayWidth;
            DisplayHeight = displayHeight;
            Console.SetWindowSize(displayWidth, displayHeight);
        }

        public string GetInput()
        {
            return Console.ReadLine();
        }

        public void Clear()
        {
            Console.Clear();
        }

        public string Format(string s)
        {
            return LeftPadding + s.PadRight(DisplayWidth - (LeftPadding.Length + RightPadding.Length)) + RightPadding;
        }

        public void SendWait(string s)
        {
            SendKeys.SendWait(s);
        }
    }
}
