using Akka.Actor;
using Akka.Event;
using MovieStreaming.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieStreaming.Actors
{
    public class UserActor : ReceiveActor
    {
        private readonly int _userId;
        private string _currentlyWatching;
        private ILoggingAdapter _logger = Context.GetLogger();

        public UserActor(int userId)
        {
            _userId = userId;
            Stopped();
        }

        private void Playing()
        {
            Receive<PlayMovieMessage>(message =>
            {
                //TODO: log UserActor _userId cannot start playing another movie before stopping an existing one
                _logger.Warning("UserActor {User} cannot start playing another movie before stopping existing one", _userId);
            });

            Receive<StopMovieMessage>(message => StopPlayingCurrentMovie());

            // TODO: log UserActor _userId behaviour has now become playing
            _logger.Info("UserActor {User} behaviour has become playing", _userId);
        }

        private void Stopped()
        {
            Receive<PlayMovieMessage>(message => StartPlayingMovie(message.MovieTitle));
            Receive<StopMovieMessage>(message =>
            {
                _logger.Warning("UserActor {User} cannot stop if nothing is playing", _userId);
            });
            _logger.Info("UserActor {_userId} has now become stopped", _userId);
        }

        private void StartPlayingMovie(string title)
        {
            _currentlyWatching = title;
            _logger.Info("User {User} is currently watching {Title}", _userId, _currentlyWatching);

            Context.ActorSelection("/user/Playback/PlaybackStatistics/MoviePlayCounter")
                .Tell(new IncrementPlayCountMessage(title));

            Context.ActorSelection("/user/Playback/PlaybackStatistics/TrendingMovies")
                .Tell(new IncrementPlayCountMessage(title));

            Become(Playing);
        }

        private void StopPlayingCurrentMovie()
        {
            _logger.Info("UserActor {User} has stopped watching {_currentlyWatching}", _userId, _currentlyWatching);
            _currentlyWatching = null;
            Become(Stopped);
        }

        protected override void PreStart()
        {
            _logger.Debug("User Actor {User} PreStart");
        }

        protected override void PostStop()
        {
            _logger.Debug("User Actor {User} PostStop");
        }

        protected override void PreRestart(Exception reason, object message)
        {
            _logger.Debug("User Actor PreRestart because {reason}");
            base.PreRestart(reason, message);
        }
        protected override void PostRestart(Exception reason)
        {
            _logger.Debug("User Actor {_userId} PostRestart because reason {reason}");
            base.PostRestart(reason);
        }
    }
}