using Akka.Actor;
using System;
using System.Threading;

namespace ActorModel
{
    public class UserActor : ReceiveActor
    {
        public string CurrentlyPlaying { get; set; }

        private readonly IActorRef _stats;

        public UserActor(IActorRef stats)
        {
            _stats = stats;

            Receive<PlayMovieMessage>(m => {
                CurrentlyPlaying = m.MovieTitle;
                Sender.Tell(new NowPlayingMessage(CurrentlyPlaying));
                _stats.Tell(m.MovieTitle);
            });
        }
    }
}
