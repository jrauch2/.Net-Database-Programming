using System.Collections.Generic;

namespace Movie_Project
{/// <summary>
    /// The <c>IInput</c> interface.
    /// provides signatures for methods to query stored tickets.
    /// </summary>
    internal interface IInput
    {
        /// <summary>
        /// Get a <c>List</c> of all stored movies. 
        /// </summary>
        /// <returns>A <c>List</c> of all stored tickets.</returns>
        List<Movie> GetStoredMovies();
        /// <summary>
        /// Gets the highest ID stored.
        /// Used to generate new tickets without reusing IDs.
        /// </summary>
        /// <returns>
        /// The highest ID as an <c>int</c>.
        /// </returns>
        int GetMaxId();
        /// <summary>
        /// Get a <c>Ticket</c> by ID.
        /// </summary>
        /// <param name="id">The id of the desired <c>Ticket</c> as an <c>int</c>.</param>
        /// <returns>The desired <c>Ticket</c>, if found.</returns>
        Movie FindId(int id);
    }
}