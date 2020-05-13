using System;
using System.Collections.Generic;
using System.Text;

namespace MovieStreamingActorSystem.Messages
{
    public class IncrementPlayCountMessage
    {

        public string MovieTitle { get; private set; }

        public IncrementPlayCountMessage(string movieTitle)
        {
            MovieTitle = movieTitle;
        }
    }
}
