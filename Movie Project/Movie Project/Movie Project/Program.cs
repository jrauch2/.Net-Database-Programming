namespace Movie_Project
{
    internal class Program
    {
        private static readonly MovieProject MovieProjectInstance = MovieProject.GetMovieProjectInstance();
        public static void Main(string[] args)
        {
            MovieProjectInstance.Menu();
        }
    }
}
