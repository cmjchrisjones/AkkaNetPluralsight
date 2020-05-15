using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MovieStreaming.Actors
{
    public class SimpleTrendingMovieAnalyzer : ITrendingMovieAnalyzer
    {
        public string CalculateMostPopularMovie(IEnumerable<string> movieTitles)
        {
            var movieCounts = movieTitles.GroupBy(title => title, (key, values) => new { MovieTitle = key, PlayCount = values.Count() });

            return movieCounts.OrderByDescending(x => x.PlayCount).First().MovieTitle;
        }
    }
}
