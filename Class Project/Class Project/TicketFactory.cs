namespace Class_Project
{
    sealed class TicketFactory
    {
        private static readonly TicketFactory _instance = new TicketFactory();

        private TicketFactory()
        {

        }

        public static TicketFactory GetTicketFactory()
        {
            return _instance;
        }
    }
}