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
        private static readonly MovieFactory MovieFactoryInstance = MovieFactory.GetMovieFactoryInstance();
        private static readonly GenreTracker GenreTrackerInstance = GenreTracker.GetGenreTrackerInstance();
        private static int _lastUsedId = CsvInputInstance.GetMaxId();
        private const int ConsoleWidth = 52;
        private const int ConsoleHeight = 30;

        private const string FiftyTwoStarLine = " ************************************************** ";
        private const string Header = " *                  Movie Program                 * ";
        private const string NewTicketHeader = " *                   New  Movie                   * ";
        private const string InputEmptyMessage = "User input cannot be empty.";
        private const string InputNullMessage = "User input is null in method {0}";
        private const string InvalidOptionMessage = "Not a valid option, try again.";
        private const string MovieExistsMessage = "Cannot add movie. Movie already exists.";
        private const string ContinueMessage = "Press any key to continue...";
        private const string ViewMovieHeader = " *               View Movie Details               * ";


        private MovieProject()
        {

        }

        public static MovieProject GetMovieProjectInstance()
        {
            return Instance;
        }

        public void Menu()
        {
            var correct = false;
            
            do
            {
                Console.SetWindowSize(ConsoleWidth, ConsoleHeight);
                Console.WriteLine(FiftyTwoStarLine);
                Console.WriteLine(Header);
                Console.WriteLine(FiftyTwoStarLine);
                Console.WriteLine(" * 1) Add Movie".PadRight(ConsoleWidth - 2) + "* ");
                Console.WriteLine(" * 2) View Movie Details".PadRight(ConsoleWidth - 2) + "* ");
                Console.WriteLine(" * 3) Print All Movies".PadRight(ConsoleWidth - 2) + "* ");
                Console.WriteLine(" * 4) Exit".PadRight(ConsoleWidth - 2) + "* ");
                Console.WriteLine(FiftyTwoStarLine);
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
                        
                        break;
                    case "4":
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

        private void ViewMovieDetails()
        {
            var correct = false;
            do
            {
                Console.Clear();
                Console.WriteLine(FiftyTwoStarLine);
                Console.WriteLine(Header);
                Console.WriteLine(ViewMovieHeader);
                Console.WriteLine(FiftyTwoStarLine);
                Console.WriteLine(" * 1) Get Movie by ID.".PadRight(ConsoleWidth - 2) + "* ");
                Console.WriteLine(" * 2) Get Movie by title. [Movie Title (YEAR)]".PadRight(ConsoleWidth - 2) + "* ");
                var input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        GetMovieById();
                        break;
                    case "2":
                        break;
                    default:
                        break;
                }
            } while (!correct);
        }

        private void GetMovieById()
        {
            Console.Clear();
            Console.WriteLine(FiftyTwoStarLine);
            Console.WriteLine(Header);
            Console.WriteLine(ViewMovieHeader);
            Console.WriteLine(FiftyTwoStarLine);
            
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

        private void VerifyInput(string title, List<string> genres)
        {
            var correct = false;

            do
            {
                Console.WriteLine(FiftyTwoStarLine);
                Console.WriteLine(Header);
                Console.WriteLine(NewTicketHeader);
                Console.WriteLine(FiftyTwoStarLine);
                var s = " * 1) Title: " + title.PadRight(ConsoleWidth - 2) + "* ";
                Console.WriteLine(s);
                s = (" * 2) Genres: ").PadRight(ConsoleWidth - 2) + "* ";
                Console.WriteLine(s);
                Console.WriteLine(genres.ToFormattedString());
                Console.WriteLine(FiftyTwoStarLine);
                Console.Write("Would you like to make a change?\n(enter 0 to accept): ");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "0":
                        correct = true;
                        if (!MovieExists(title))
                        {
                            CsvInputInstance.AddMovie(MovieFactoryInstance.NewMovie(++_lastUsedId, title, genres));
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

        private bool MovieExists(string title)
        {
            return CsvInputInstance.FindMovieByTitle(title, out _);
        }

        private string GetTitleInput()
        {
            var good = false;
            string input;
            do
            {
                Console.Clear();
                Console.WriteLine(FiftyTwoStarLine);
                Console.WriteLine(Header);
                Console.WriteLine(NewTicketHeader);
                Console.WriteLine(FiftyTwoStarLine);
                Console.WriteLine(" * Movie Title (YEAR)[Toy Story (1995)]:".PadRight(ConsoleWidth - 2) + "* ");
                Console.WriteLine(FiftyTwoStarLine);
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
                    default:
                        good = true;
                        break;
                }
            } while (!good);

            return input;
        }

        private List<string> GetGenreInput()
        {
            var list = new List<string>();
            Console.WriteLine(FiftyTwoStarLine);
            Console.WriteLine(Header);
            Console.WriteLine(NewTicketHeader);
            Console.WriteLine(FiftyTwoStarLine);
            Console.WriteLine(" * For genre examples, enter 'g'".PadRight(ConsoleWidth - 2) + "* ");
            Console.WriteLine(" * Enter genres, comma separated:".PadRight(ConsoleWidth - 2) + "* ");
            Console.WriteLine(FiftyTwoStarLine);
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

        private void PrintInBoundaries(string s)
        {
            var sa = WordWrap.Wrap(s, ConsoleWidth - 6).Split('|');
            for (var i = 0; i < sa.Length; i++)
            {
                sa[i] = sa[i].PadRight(ConsoleWidth - 2) + "* ";
                Console.WriteLine(sa[i]);
            }
        }

        private void CloseProgram()
        {
            Environment.Exit(0);
        }
    }
}
