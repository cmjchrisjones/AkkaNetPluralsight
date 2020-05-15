using System;
using System.Collections.Generic;

namespace MovieStreaming.Actors
{
    public interface ITrendingMovieAnalyzer
    {
        string CalculateMostPopularMovie(IEnumerable<string> movieTitles);
    }
}
