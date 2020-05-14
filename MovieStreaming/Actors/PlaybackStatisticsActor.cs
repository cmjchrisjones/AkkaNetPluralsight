using Akka.Actor;
using Akka.Event;
using MovieStreaming.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieStreaming.Actors
{
    public class PlaybackStatisticsActor : ReceiveActor
    {
        private readonly ILoggingAdapter _logger = Context.GetLogger();
        public PlaybackStatisticsActor()
        {
            Context.ActorOf(Props.Create<MoviePlayCounterActor>(), "MoviePlayCounter");
            Context.ActorOf(Props.Create<TrendingMoviesActor>(), "TrendingMovies");
        }

        protected override SupervisorStrategy SupervisorStrategy()
        {
            return new OneForOneStrategy(exception =>
            {
                if (exception is ActorInitializationException)
                {
                    // TODO: log PlaybackStatisticsActor supervisor strategy stopped
                    _logger.Error(exception, "PlaybackStatisticsActor supervisor strategy stopping child due to ActorInitializationException");
                    return Directive.Stop;
                }

                if (exception is SimulatedTerribleMovieException)
                {
                    var terribleMovieEx = (SimulatedTerribleMovieException)exception;

                    // TODO: log supervisor strategy resuming due to terrible movie exception
                    _logger.Warning($"PlaybackStatisticsActor supervisor strategy resuming child due to terrible movie {terribleMovieEx.MovieTitle}");
                    return Directive.Resume;
                }

                _logger.Error(exception, "PlaybackStatisticsActor supervisor strategy stopping child due to ActorInitializationException");
                return Directive.Restart;
            });
        }

        protected override void PreStart()
        {
           _logger.Debug("PlaybackStatisticsActor Actor PreStart");
        }

        protected override void PostStop()
        {
            _logger.Debug("PlaybackStatisticsActor Actor PostStop");
        }

        protected override void PreRestart(Exception reason, object message)
        {
            _logger.Debug($"PlaybackStatisticsActor Prerestart because {reason}.");
            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
            _logger.Debug($"PlaybackStatisticsActor Postrestart because {reason}.");
            base.PostRestart(reason);
        }
    }
}
