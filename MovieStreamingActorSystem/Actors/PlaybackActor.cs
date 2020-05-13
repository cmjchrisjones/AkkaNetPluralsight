using Akka.Actor;
using MovieStreamingActorSystem.Messages;
using System;

namespace MovieStreamingActorSystem.Actors
{
    public class PlaybackActor : ReceiveActor
    {
        public PlaybackActor()
        {
            Context.ActorOf(Props.Create<UserCoordinatorActor>(), "UserCoordinator");
            Context.ActorOf(Props.Create<PlaybackStatisticsActor>(), "PlaybackStatistics");
        }

        protected override void PreStart()
        {
            ColourConsole.WriteGreenLine("Playback Actor PreStart");
        }

        protected override void PostStop()
        {
            ColourConsole.WriteGreenLine("Playback Actor PostStop");
        }

        protected override void PreRestart(Exception reason, object message)
        {
            ColourConsole.WriteGreenLine($"Playback Prerestart because {reason}.");
            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
            ColourConsole.WriteGreenLine($"Playback Postrestart because {reason}.");
            base.PostRestart(reason);
        }
    }
}
