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
    // A Movie consists of an ID, title, and a List<string> of genres.
    internal class Movie
    {
        private int _id;
        private string _title;
        private List<string> _movieGenres;

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

        public void SetMovieGenres(string genreString)
        {
            _movieGenres = new List<string>();
            if (genreString != null)
            {
                //TODO
            }
        }

        /// <summary>
        /// Checks if the <c>List</c> already contains the genre, then adds it if it does not.
        /// </summary>
        /// <param name="movieGenre">genre to be added.</param>
        // Checks if the List<string> already contains the genre, then adds it if it does not.
        public void AddGenre(string movieGenre)
        {
            if (_movieGenres.Contains(movieGenre))
                _movieGenres.Add(movieGenre);
        }
    }
}