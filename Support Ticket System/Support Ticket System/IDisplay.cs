namespace Support_Ticket_System
{
    internal interface IDisplay
    {
        int DisplayHeight { get; set; }
        int DisplayWidth { get; set; }
        string LeftPadding { get; set; }
        string RightPadding { get; set; }

        void Write(string message);
        void WriteLine(string message);
        void SetWindowSize(int displayWidth, int displayHeight);
        string GetInput();
        void Clear();
        string Format(string s);
    }
}
