using Akka.Actor;
using System;

namespace ActorModel
{
    public class UserActor : ReceiveActor
    {
        public string CurrentlyPlaying { get; set; }

        public UserActor()
        {
            Receive<PlayMovieMessage>(m => {
                CurrentlyPlaying = m.MovieTitle;
            });
        }
    }
}
