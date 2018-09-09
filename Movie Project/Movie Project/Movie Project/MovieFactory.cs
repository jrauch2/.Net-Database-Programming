namespace Movie_Project
{
    /// <summary>
    /// The <c>MovieFactory</c> Class.
    /// Follows a singleton pattern.
    /// Builds all <c>Movie</c> objects.
    /// </summary>
    // The MovieFactory class.
    // Follows a singleton pattern.
    // Builds all Movie objects.
    internal sealed class MovieFactory
    {
        private static readonly MovieFactory Instance = new MovieFactory();

        /// <summary>
        /// Private constructor.
        /// To access <c>MovieFactory</c>, call <c>GetMovieFactoryInstance()</c>.
        /// </summary>
        // Private constructor.
        // To access MovieFactory, call GetMovieFactoryInstance().
        private MovieFactory()
        {

        }

        /// <summary>
        /// Get the <c>MovieFactory</c> instance.
        /// </summary>
        /// <returns>THe <c>MovieFactory</c> instance</returns>
        // Get the MovieFactory instance.
        public static MovieFactory GetMovieFactoryInstance()
        {
            return Instance;
        }
    }
}