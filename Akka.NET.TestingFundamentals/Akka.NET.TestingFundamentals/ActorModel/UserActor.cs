using Akka.Actor;
using System;
using System.Threading;

namespace ActorModel
{
    public class UserActor : ReceiveActor
    {
        public string CurrentlyPlaying { get; set; }

        public UserActor()
        {
            Receive<PlayMovieMessage>(m => {
                CurrentlyPlaying = m.MovieTitle;
                Thread.Sleep(4000);
                Sender.Tell(new NowPlayingMessage(CurrentlyPlaying));
            });
        }
    }
}
