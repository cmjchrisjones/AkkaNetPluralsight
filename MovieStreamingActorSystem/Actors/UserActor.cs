using Akka.Actor;
using MovieStreamingActorSystem.Messages;
using System;

namespace MovieStreamingActorSystem.Actors
{
    public class UserActor : ReceiveActor
    {
        // Add State
        private string _currentlyWatching;

        public UserActor()
        {
            Console.WriteLine("Creating a UserActor");

            Receive<PlayMovieMessage>(m => HandlePlayMovieMessage(m));
            Receive<StopMovieMessage>(m => HandleStopMovieMessage());

        }

        private void HandlePlayMovieMessage(PlayMovieMessage message)
        {
            if(_currentlyWatching != null)
            {
                // Currently watching a movie
                ColourConsole.WriteRedLine("ERROR: Cannot start playing another movie before stopping existing one!");
            }
            else
            {
                StartPlayingMovie(message.MovieTitle);
            }
        }

        private void StartPlayingMovie(string title)
        {
            _currentlyWatching = title;

            ColourConsole.WriteYellowLine($"User is currently watching {_currentlyWatching}");
        }

        private void HandleStopMovieMessage()
        {
            if(_currentlyWatching == null)
            {
                // can't stop watching a movie if one isn't being watched!
                ColourConsole.WriteRedLine("ERROR: Can't stop playing if nothing is playing!");
            }
            else
            {
                StopCurrentMovie();
            }
        }

        private void StopCurrentMovie()
        {
            ColourConsole.WriteYellowLine($"Stopping movie {_currentlyWatching}");
            _currentlyWatching = null;
        }


        protected override void PreStart()
        {
            ColourConsole.WriteGreenLine("UserActor PreStart");
        }

        protected override void PostStop()
        {
            ColourConsole.WriteGreenLine("UserActor PostStop");
        }

        protected override void PreRestart(Exception reason, object message)
        {
            ColourConsole.WriteRedLine($"UserActor PreRestart because {reason}.");
            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
            ColourConsole.WriteRedLine($"UserActor PostRestart because reason {reason}.");
            base.PostRestart(reason);
        }
    }
}
