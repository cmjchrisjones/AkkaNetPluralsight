using Akka.Actor;
using Akka.Event;
using MovieStreaming.Messages;
using System;
using System.Collections.Generic;

namespace MovieStreaming.Actors
{
    public class TrendingMoviesActor : ReceiveActor
    {
        private readonly ILoggingAdapter _logger = Context.GetLogger();

        private readonly ITrendingMovieAnalyzer _trendAnalyzer;

        private readonly Queue<string> _recentlyPlayedMovies;
        private const int NumberOfRecentMoviesToAnalyze = 5;

        public TrendingMoviesActor(ITrendingMovieAnalyzer trendAnalyzer)
        {
            _recentlyPlayedMovies = new Queue<string>(NumberOfRecentMoviesToAnalyze);

            // Tightly Coupled - creates new concrete dependency of SimpleTrendingMovieAnalyzer
            //_trendAnalyzer = new SimpleTrendingMovieAnalyzer();

            // Loosely Coupled
            _trendAnalyzer = trendAnalyzer;

            Receive<IncrementPlayCountMessage>(message => HandleIncrementMessage(message));
        }

        private void HandleIncrementMessage(IncrementPlayCountMessage message)
        {
            var recentlyPlayedMoviesBufferIsFull = _recentlyPlayedMovies.Count == NumberOfRecentMoviesToAnalyze;

            if (recentlyPlayedMoviesBufferIsFull)
            {
                _recentlyPlayedMovies.Dequeue();
            }

            _recentlyPlayedMovies.Enqueue(message.MovieTitle);
        }

        protected override void PreStart()
        {
            _logger.Debug("TrendingMovies Actor PreStart");
        }

        protected override void PostStop()
        {
            _logger.Debug("TrendingMovies Actor PostStop");
        }

        protected override void PreRestart(Exception reason, object message)
        {
            _logger.Debug($"TrendingMovies PreRestart because {reason}.");
            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
            _logger.Debug($"TrendingMovies PostRestart because {reason}.");
            base.PostRestart(reason);
        }
    }
}
