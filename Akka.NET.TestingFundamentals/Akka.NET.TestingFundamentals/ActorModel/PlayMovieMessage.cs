using System;
using System.Collections.Generic;
using System.Text;

namespace ActorModel
{
    public class PlayMovieMessage
    {
        public string MovieTitle { get; private set; }

        public PlayMovieMessage(string movieTitle)
        {
            MovieTitle = movieTitle;
        }
    }
}
