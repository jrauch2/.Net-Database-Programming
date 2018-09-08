using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
using NUnit.Framework;

namespace Movie_Project
{
    /// <summary>
    /// The <c>Movie</c> class.
    /// A <c>Movie</c> consists of an ID, title, and a <c>List</c> of genres.
    /// </summary>
    // The Movie class.
    // A Movie consists of an ID, title, and a List<MovieGenre>.
    internal class Movie
    {
        private int _id;
        private string _title;
        private List<MovieGenre> _movieGenres;

        public int GetId()
        {
            return _id;
        }

        private void SetId(int id)
        {
            this._id = id;
        }

        public string GetTitle()
        {
            return _title;
        }

        public void SetTitle(string title)
        {
            this._title = title;
        }

        /// <summary>
        /// Checks if the <c>List</c> already contains the genre, then adds it if it does not.
        /// </summary>
        /// <param name="movieGenre"><c>MovieGenre</c> to be added.</param>
        // Checks if the List<MovieGenre> already contains the genre, then adds it if it does not.
        public void AddGenre(MovieGenre movieGenre)
        {
            if (_movieGenres.Contains(movieGenre))
                _movieGenres.Add(movieGenre);
        }
    }
}