using System;
using System.Collections.Generic;
using System.Text;

namespace MovieStreamingActorSystem.Messages
{
    public class PlayMovieMessage
    {
        public PlayMovieMessage(string movieTitle, int userId)
        {
            MovieTitle = movieTitle;
            UserId = userId;
        }

        public string MovieTitle { get; private set; }

        public int UserId { get; private set; }
    }
}
