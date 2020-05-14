using Akka.Actor;
using Akka.Event;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieStreaming.Actors
{
    public class PlaybackActor : ReceiveActor
    {
        private readonly ILoggingAdapter _logger = Context.GetLogger();

        public PlaybackActor()
        {
            Context.ActorOf(Props.Create<UserCoordinatorActor>(), "UserCoordinator");
            Context.ActorOf(Props.Create<PlaybackStatisticsActor>(), "PlaybackStatistics");
        } 

        protected override void PreStart()
        {
            _logger.Debug("Playback Actor PreStart");
        }

        protected override void PostStop()
        {
            _logger.Debug("Playback Actor PostStop");
        }

        protected override void PreRestart(Exception reason, object message)
        {
            _logger.Debug($"Playback PreRestart because {reason}.");
            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
            _logger.Debug($"Playback PostRestart because {reason}.");
            base.PostRestart(reason);
        }
    }
}
