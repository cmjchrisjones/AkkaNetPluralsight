using System;
using System.Collections.Generic;
using System.Text;

namespace MovieStreaming.Messages
{
    public class PlayMovieMessage
    {
        public int UserId { get; private set; }

        public string MovieTitle { get; set; }

        public PlayMovieMessage(string movieTitle, int userId)
        {
            UserId = userId;
            MovieTitle = movieTitle;
        }
    }
}
