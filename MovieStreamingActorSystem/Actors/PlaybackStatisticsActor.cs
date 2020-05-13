using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieStreamingActorSystem.Actors
{
    public class PlaybackStatisticsActor : ReceiveActor
    {

        public PlaybackStatisticsActor()
        {
            Context.ActorOf(Props.Create<MoviePlayCounterActor>(), "MoviePlayCounter");
        }

        protected override void PreStart()
        {
            ColourConsole.WriteWhiteLine("PlaybackStatisticsActor Actor PreStart");
        }

        protected override void PostStop()
        {
            ColourConsole.WriteWhiteLine("PlaybackStatisticsActor Actor PostStop");
        }

        protected override void PreRestart(Exception reason, object message)
        {
            ColourConsole.WriteWhiteLine($"PlaybackStatisticsActor Prerestart because {reason}.");
            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
            ColourConsole.WriteWhiteLine($"PlaybackStatisticsActor Postrestart because {reason}.");
            base.PostRestart(reason);
        }
    }
}
