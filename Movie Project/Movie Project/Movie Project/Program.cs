using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NLog;
using NUnit.Framework.Api;

namespace Movie_Project
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();
            logger.Trace("Application starting...");
            //            const string regex = ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)";
            //            var genres = new List<string>();
            //            using (var file = new StreamReader("ml-latest-small\\movies.csv"))
            //            {
            //                var count = 0;
            //                while (!file.EndOfStream)
            //                {
            //                    if (count++ == 0)
            //                    {
            //                        file.ReadLine();
            //                    }
            //                    else
            //                    {
            //                        var line = file.ReadLine();
            //                        if (line == null) continue;
            //                        var movie = Regex.Split(line, regex);
            //                        var genreArray = movie[2].Split('|');
            //
            //                        genres.Sort();
            //
            //                        foreach (var s in genreArray)
            //                        {
            //                            if (!genres.Contains(s))
            //                            {
            //                                genres.Add(s);
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //
            //            using (var fileOut = new StreamWriter("genres.csv"))
            //            {
            //                var count = 0;
            //                for (var i = 0; i < genres.Count(); i++) 
            //                {
            //                    if (count++ == 0)
            //                        fileOut.Write(genres[i]);
            //                    else
            //                    {
            //                        fileOut.Write(',' + genres[i]);
            //                    }
            //                }
            //            }


        }
    }
}
