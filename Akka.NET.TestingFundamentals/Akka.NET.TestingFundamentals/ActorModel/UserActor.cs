using Akka.Actor;
using Akka.Event;
using System;
using System.Threading;

namespace ActorModel
{
    public class UserActor : ReceiveActor
    {
        public string CurrentlyPlaying { get; set; }

        private readonly IActorRef _stats;

        private readonly ILoggingAdapter _logger = Context.GetLogger();

        public UserActor(IActorRef stats)
        {
            _stats = stats;

            Receive<PlayMovieMessage>(m => {

                if(m.MovieTitle == "Null Terminator")
                {
                    throw new NotSupportedException();
                }

                _logger.Info($"Started playing {m.MovieTitle}");
                CurrentlyPlaying = m.MovieTitle;
                _logger.Info($"Replying to sender");
                Sender.Tell(new NowPlayingMessage(CurrentlyPlaying));
                _stats.Tell(m.MovieTitle);

                Context.ActorSelection("/user/audit").Tell(m);
                Context.System.EventStream.Publish(new NowPlayingMessage(m.MovieTitle));
                
            });
        }
    }
}
