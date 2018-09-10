using System.Collections.Generic;
using System.Linq;

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

        public Movie(int id, string title, List<string> movieGenres)
        {
            SetId(id);
            SetTitle(title);
            SetMovieGenres(movieGenres);
        }

        public int GetId()
        {
            return _id;
        }

        private void SetId(int id)
        {
            _id = id;
        }

        public string GetTitle()
        {
            return _title;
        }

        public void SetTitle(string title)
        {
            _title = title;
        }

        public List<string> GetMovieGenres()
        {
            return _movieGenres;
        }

        private void SetMovieGenres(List<string> genres)
        {
            for (var i = 0; i < genres.Count; i++)
            {
                genres[i] = genres[i].FirstCharToUpper();
            }
            _movieGenres = genres;
        }
        
        /// <summary>
        /// Checks if the <c>List</c> already contains the genre, then adds it if it does not.
        /// </summary>
        /// <param name="movieGenre">genre to be added.</param>
        // Checks if the List<string> already contains the genre, then adds it if it does not.
        public void AddGenre(string movieGenre)
        {
            if (_movieGenres.Contains(movieGenre.FirstCharToUpper()))
                _movieGenres.Add(movieGenre.FirstCharToUpper());
        }

        public void RemoveGenre(string genre)
        {
            //TODO
        }

        public override string ToString()
        {
            return _id.ToString() + "," +  _title + "," + GenresToString();
        }

        private string GenresToString()
        {
            if (_movieGenres.Any())
            {
                var s = "";
                for (var i = 0; i < _movieGenres.Count; i++)
                {
                    if (i == 0)
                    {
                        s += _movieGenres[i];
                    }
                    else
                    {
                        s += '|' + _movieGenres[i];
                    }
                }

                return s;
            }
            else
            {
                return "(no genres listed)";
            }
        }
    }
}