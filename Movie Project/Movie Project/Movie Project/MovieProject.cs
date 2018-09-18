using System;
using System.Collections.Generic;
using NLog;


namespace Movie_Project
{
    internal sealed class MovieProject
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static readonly MovieProject Instance = new MovieProject();
        private const string FileName = "ml-latest-small\\movies.csv";
        private static readonly IInput CsvInputInstance = CsvInput.GetCsvInputInstance(FileName);
        private static readonly IOutput CsvOutput = new CsvOutput(FileName);
        private static readonly MovieFactory MovieFactoryInstance = MovieFactory.GetMovieFactoryInstance();
        private static readonly GenreTracker GenreTrackerInstance = GenreTracker.GetGenreTrackerInstance();
        private static int _lastUsedId = CsvInputInstance.GetMaxId();
        private const int ConsoleWidth = 52;
        private const int ConsoleHeight = 30;

        private const string FiftyStarLine = " ************************************************** ";
        private const string Header = " *                  Movie Program                 * ";
        private const string NewTicketHeader = " *                   New  Movie                   * ";
        private const string InputEmptyMessage = "User input cannot be empty.";
        private const string InputNullMessage = "User input is null in method {0}";
        private const string InvalidOptionMessage = "Not a valid option, try again.";
        private const string MovieExistsMessage = "Cannot add movie. Movie already exists.";
        private const string ContinueMessage = "Press any key to continue...";
        private const string ViewMovieHeader = " *               View Movie Details               * ";
        private const string MovieNotFoundMessage = "That Movie was not found.";
        private const string InputNotInt = "The input was not a numeric ID.";
        private const string MovieAddedSuccessMessage = "Movie added successfully.";


        private MovieProject()
        {

        }

        public static MovieProject GetMovieProjectInstance()
        {
            return Instance;
        }

        public void Menu()
        {
            Console.SetWindowSize(ConsoleWidth, ConsoleHeight);
            var correct = false;
            
            do
            {
                Console.Clear();
                Console.WriteLine(FiftyStarLine);
                Console.WriteLine(Header);
                Console.WriteLine(FiftyStarLine);
                Console.WriteLine(" * 1) Add Movie".PadRight(ConsoleWidth - 2) + "* ");
                Console.WriteLine(" * 2) View Movie Details".PadRight(ConsoleWidth - 2) + "* ");
                Console.WriteLine(" * 3) Print All Movies".PadRight(ConsoleWidth - 2) + "* ");
                Console.WriteLine(" * 4) View All Movies By Genre".PadRight(ConsoleWidth - 2) + "* ");
                Console.WriteLine(" * 5) Exit".PadRight(ConsoleWidth - 2) + "* ");
                Console.WriteLine(FiftyStarLine);
                Console.Write("Select an option: ");
                var input = Console.ReadLine();
                Console.Clear();
                switch (input)
                {
                    case "1":
                        NewMovieBuilder();
                        break;
                    case "2":
                        ViewMovieDetails();
                        break;
                    case "3":
                        DisplayAllMovies();
                        break;
                    case "4":
                        DisplayAllMoviesByGenre();
                        break;
                    case "5":
                        correct = true;
                        CloseProgram();
                        break;
                    default:
                        Console.WriteLine(InvalidOptionMessage);
                        Logger.Warn(InvalidOptionMessage);
                        Console.WriteLine(ContinueMessage);
                        Console.ReadLine();
                        break;
                }
            } while (!correct);
        }

        private void NewMovieBuilder()
        {
            Console.Clear();
            var title = GetTitleInput();
            Console.Clear();
            var genres = GetGenreInput();
            Console.Clear();
            VerifyInput(title, genres);
        }

        private string GetTitleInput()
        {
            var good = false;
            string input;
            do
            {
                Console.Clear();
                Console.WriteLine(FiftyStarLine);
                Console.WriteLine(Header);
                Console.WriteLine(NewTicketHeader);
                Console.WriteLine(FiftyStarLine);
                Console.WriteLine(" * Movie Title (YEAR)[Toy Story (1995)]".PadRight(ConsoleWidth - 2) + "* ");
                Console.WriteLine(" Type \"cancel\" to cancel:".PadRight(ConsoleWidth - 2) + "* ");
                Console.WriteLine(FiftyStarLine);
                input = Console.ReadLine();
                switch (input)
                {
                    case null:
                        Logger.Error(InputNullMessage, nameof(GetTitleInput));
                        break;
                    case "":
                        Logger.Error(InputEmptyMessage);
                        Console.WriteLine(InputEmptyMessage);
                        Console.WriteLine(ContinueMessage);
                        Console.ReadLine();
                        break;
                    case "cancel":
                        Menu();
                        break;
                    default:
                        good = true;
                        break;
                }
            } while (!good);

            return input;
        }

        private bool MovieTitleExists(string title)
        {
            return CsvInputInstance.FindMovieByTitle(title, out _);
        }

        private List<string> GetGenreInput()
        {
            var list = new List<string>();
            Console.WriteLine(FiftyStarLine);
            Console.WriteLine(Header);
            Console.WriteLine(NewTicketHeader);
            Console.WriteLine(FiftyStarLine);
            Console.WriteLine(" * For genre examples, enter 'g'".PadRight(ConsoleWidth - 2) + "* ");
            Console.WriteLine(" * Enter genres, comma separated:".PadRight(ConsoleWidth - 2) + "* ");
            Console.WriteLine(FiftyStarLine);
            var input = Console.ReadLine();

            if (input != null)
            {
                if (input.Equals(""))
                {
                    return list;
                }
                else if (input == "g")
                {
                    PrintInBoundaries(GenreTrackerInstance.ToFormattedString());
                    input = Console.ReadLine();
                }
            }
            else
            {
                Logger.Error(InputNullMessage, nameof(GetTitleInput));
            }

            if (input != null)
            {
                if (input.Equals("")) return list;
                else
                {
                    var sa = input.Split(',');
                    foreach (var s in sa)
                    {
                        var g = s.Trim();
                        if (!GenreTrackerInstance.Contains(g.FirstCharToUpper()))
                        {
                            NotKnownGenre(g);
                        }
                        else
                        {
                            list.Add(g.FirstCharToUpper());
                        }
                    }
                }
            }
            else
            {
                Logger.Error(InputNullMessage, nameof(GetTitleInput));
            }

            return list;
        }

        private void NotKnownGenre(string genre)
        {
            var done = false;
            do
            {
                Console.WriteLine("{0} is not a known genre.");
                Console.Write("Would you like to add it (Y/N)?: ");
                var boolInput = Console.ReadLine();
                switch (boolInput)
                {
                    case null:
                        Logger.Error(InputNullMessage, nameof(GetTitleInput));
                        break;
                    case "":
                        Logger.Error(InputEmptyMessage);
                        Console.WriteLine(InputEmptyMessage);
                        Console.WriteLine(ContinueMessage);
                        Console.ReadLine();
                        break;
                    default:
                        {
                            if (boolInput.ToUpper().Equals("Y"))
                            {
                                GenreTrackerInstance.AddGenre(genre.FirstCharToUpper());
                                done = true;
                            }
                            else if (boolInput.ToUpper().Equals("N"))
                            {
                                done = true;
                            }
                            else
                            {
                                Console.WriteLine(InvalidOptionMessage);
                                Logger.Warn(InvalidOptionMessage);
                                Console.WriteLine(ContinueMessage);
                                Console.ReadLine();
                            }
                            break;
                        }
                }
            } while (!done);
        }

        private void VerifyInput(string title, List<string> genres)
        {
            var correct = false;

            do
            {
                Console.WriteLine(FiftyStarLine);
                Console.WriteLine(Header);
                Console.WriteLine(NewTicketHeader);
                Console.WriteLine(FiftyStarLine);
                var s = " * 1) Title: " + title.PadRight(ConsoleWidth - 2) + "* ";
                Console.WriteLine(s);
                s = (" * 2) Genres: ").PadRight(ConsoleWidth - 2) + "* ";
                Console.WriteLine(s);
                Console.WriteLine(genres.ToFormattedString());
                Console.WriteLine(FiftyStarLine);
                Console.Write("Would you like to make a change?\n(enter 0 to accept): ");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "0":
                        correct = true;
                        if (!MovieTitleExists(title))
                        {
                            var addedMovie = MovieFactoryInstance.NewMovie(++_lastUsedId, title, genres);
                            CsvOutput.AddMovie(addedMovie);
                            Logger.Trace(MovieAddedSuccessMessage);
                            Console.WriteLine(MovieAddedSuccessMessage);
                            DisplayMovie(addedMovie);
                            Console.WriteLine(ContinueMessage);
                            Console.ReadLine();
                        }
                        else
                        {
                            Console.WriteLine(MovieExistsMessage);
                            Logger.Error(MovieExistsMessage);
                            Console.WriteLine(ContinueMessage);
                            Console.ReadLine();
                        }
                        break;
                    case "1":
                        Console.Clear();
                        title = GetTitleInput();
                        break;
                    case "2":
                        Console.Clear();
                        genres = GetGenreInput();
                        break;
                    default:
                        Console.WriteLine(InvalidOptionMessage);
                        Console.WriteLine(ContinueMessage);
                        Console.ReadLine();
                        break;
                }
                Console.Clear();
            } while (!correct);
        }

        private void ViewMovieDetails()
        {
            Console.Clear();
            Console.WriteLine(FiftyStarLine);
            Console.WriteLine(Header);
            Console.WriteLine(ViewMovieHeader);
            Console.WriteLine(FiftyStarLine);
            Console.WriteLine(" * 1) Get Movie by ID".PadRight(ConsoleWidth - 2) + "* ");
            Console.WriteLine(" * 2) Get Movie by title [Movie Title (YEAR)]".PadRight(ConsoleWidth - 2) + "* ");
            Console.WriteLine(" * 3) Main Menu");
            var input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    Console.Clear();
                    FindMovieById();
                    break;
                case "2":
                    Console.Clear();
                    FindMovieByTitle();
                    break;
                case "3":
                    Console.Clear();
                    break;
                default:
                    Console.WriteLine(InvalidOptionMessage);
                    Logger.Warn(InvalidOptionMessage);
                    Console.WriteLine(ContinueMessage);
                    Console.ReadLine();
                    break;
            }
        }

        private void FindMovieByTitle()
        {
            Console.WriteLine(FiftyStarLine);
            Console.WriteLine(Header);
            Console.WriteLine(ViewMovieHeader);
            Console.WriteLine(FiftyStarLine);
            Console.WriteLine(" *Enter the title of the movie".PadRight(ConsoleWidth - 2) + "* ");
            Console.WriteLine(" *including the year [Movie Title (YEAR)]:".PadRight(ConsoleWidth - 2) + "* ");
            Console.WriteLine(FiftyStarLine);
            var searchTitle = Console.ReadLine();
            if (CsvInputInstance.FindMovieByTitle(searchTitle, out var foundMovie))
            {
                DisplayMovie(foundMovie);
                Console.WriteLine(ContinueMessage);
                Console.ReadLine();
            }
            else
            {
                Logger.Warn(MovieNotFoundMessage);
                Console.WriteLine(ContinueMessage);
                Console.ReadLine();
            }
        }

        private void DisplayMovie(Movie movie)
        {
            if (movie is null)
            {
                var ane = new ArgumentNullException();
                Logger.Error(ane.Message, ane.Source, ane.StackTrace);
            }
            else
            {
                Console.WriteLine(FiftyStarLine);
                Console.WriteLine(" * Id:".PadRight(ConsoleWidth - 2) + "* ");
                Console.WriteLine((" * " + movie.GetId()).PadRight(ConsoleWidth - 2) + "* ");
                Console.WriteLine(" *".PadRight(ConsoleWidth - 2) + "* ");
                Console.WriteLine(" * Title:".PadRight(ConsoleWidth - 2) + "* ");
                PrintInBoundaries(movie.GetTitle());
                Console.WriteLine(" * Genres:".PadRight(ConsoleWidth - 2) + "* ");
                PrintInBoundaries(movie.GetMovieGenres().ToFormattedString());
                Console.WriteLine(FiftyStarLine);
            }
        }

        private void FindMovieById()
        {
            Console.WriteLine(FiftyStarLine);
            Console.WriteLine(Header);
            Console.WriteLine(ViewMovieHeader);
            Console.WriteLine(FiftyStarLine);
            Console.WriteLine(" *Enter the id of the movie:".PadRight(ConsoleWidth - 2) + "* ");
            Console.WriteLine(FiftyStarLine);
            var searchString = Console.ReadLine();
            if (int.TryParse(searchString, out var searchInt))
            {
                if (CsvInputInstance.FindMovieById(searchInt, out var foundMovie))
                {
                    DisplayMovie(foundMovie);
                    Console.WriteLine(ContinueMessage);
                    Console.ReadLine();
                }
                else
                {
                    Logger.Warn(MovieNotFoundMessage + " ID searched: {ID}", searchInt);
                    Console.WriteLine(MovieNotFoundMessage);
                }
            }
            else
            {
                Logger.Error(InputNotInt);
                Console.WriteLine(InputNotInt);
                Console.WriteLine(ContinueMessage);
                Console.ReadLine();
            }
        }

        private void PrintInBoundaries(string s)
        {
            var sa = WordWrap.Wrap(s, ConsoleWidth - 6).Split('|');
            for (var i = 0; i < sa.Length; i++)
            {
                sa[i] = " * " + sa[i].Replace("\"", "").PadRight(ConsoleWidth - 6) + " * ";
                Console.WriteLine(sa[i]);
            }
        }

        private void DisplayAllMovies()
        {
            foreach (var movie in CsvInputInstance.GetAllMovies())
            {
                DisplayMovie(movie);
            }
            Console.WriteLine(ContinueMessage);
            Console.ReadLine();
        }

        private void DisplayAllMoviesByGenre()
        {
            Console.WriteLine(FiftyStarLine);
            Console.WriteLine(Header);
            Console.WriteLine(FiftyStarLine);
            Console.WriteLine(" * Select Genre:".PadRight(ConsoleWidth - 2) + "* ");
            string[] genres = GenreTrackerInstance.GetGenres();
            IDictionary<int, string> dict = new Dictionary<int, string>();
            for (int i = 0; i < genres.Length; i++)
            {
//                Console.WriteLine(" * {0}) {1}".PadRight(ConsoleWidth - 2) + "* ", i, genres[i]);
                Console.WriteLine((" * " + i + ") " + genres[i]).PadRight(ConsoleWidth - 2) + "* ");
                dict.Add(i,genres[i]);
            }

            var input = Console.ReadLine();
            if (int.TryParse(input, out var key))
            {
                if (dict.TryGetValue(key, out var selectedGenre))
                {
                    foreach (var movie in CsvInputInstance.GetAllMovies())
                    {
                        if (movie.GetMovieGenres().Contains(selectedGenre))
                        {
                            DisplayMovie(movie);
                        }
                    }
                    Console.WriteLine(ContinueMessage);
                    Console.ReadLine();
                }
                else
                {
                    var fe = new FormatException();
                    Logger.Warn(fe.Message);
                    Console.WriteLine(InvalidOptionMessage);
                    Console.WriteLine(ContinueMessage);
                    Console.ReadLine();
                }
            }
            else
            {
                var fe = new FormatException();
                Logger.Warn(fe.Message);
                Console.WriteLine(InputNotInt);
                Console.WriteLine(ContinueMessage);
                Console.ReadLine();
            }
            
        }

        private void CloseProgram()
        {
            Environment.Exit(0);
        }
    }
}