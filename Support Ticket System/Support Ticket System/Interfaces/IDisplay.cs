namespace Support_Ticket_System.Interfaces
{
    internal interface IDisplay
    {
        int DisplayHeight { get; set; }
        int DisplayWidth { get; set; }
        string LeftPadding { get; set; }
        string RightPadding { get; set; }
        char SpecialCharacter { get; set; }

        void Write(string message);
        void WriteLine(string message);
        void WriteSpecialLine();
        void SetWindowSize(int displayWidth, int displayHeight);
        string GetInput();
        void Clear();
        string Format(string s);
    }
}
