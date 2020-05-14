using System;
using System.Collections.Generic;
using System.Text;

namespace MovieStreaming.Actors
{
    interface ITrendingMovieAnalyzer
    {
        public SimpleTrendingMovieAnalyzer SimpleTrendingMovieAnalyzer { get; set; }
    }
}
