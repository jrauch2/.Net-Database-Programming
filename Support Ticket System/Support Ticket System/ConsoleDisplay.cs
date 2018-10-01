using System;

namespace Support_Ticket_System
{
    class ConsoleDisplay : IDisplay
    {
        public int DisplayHeight { get; set; }
        public int DisplayWidth { get; set; }
        public string LeftPadding { get; set; }
        public string RightPadding { get; set; }

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
    }
}
