using System;
using System.Collections.Generic;
using System.Text;

namespace MovieStreaming.Exceptions
{
    public class SimulatedTerribleMovieException : Exception
    {
        public SimulatedTerribleMovieException(string message) : base(message)
        {
            MovieTitle = message;
        }

        public string MovieTitle { get; set; }
    }
}
