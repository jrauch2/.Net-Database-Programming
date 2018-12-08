namespace Support_Ticket_System.Interfaces
{
    public interface IDisplay
    {
        int DisplayHeight { get; set; }
        int DisplayWidth { get; set; }
        string LeftPadding { get; set; }
        string RightPadding { get; set; }
        char SpecialCharacter { get; set; }

        void Write<T>(T message);
        void WriteLine();
        void WriteLine<T>(T message);
        void WriteSpecialLine();
        void SetWindowSize(int displayWidth, int displayHeight);
        string GetInput();
        void Clear();
        string Format<T>(T s);
        void SendWait<T>(T s);
    }
}
