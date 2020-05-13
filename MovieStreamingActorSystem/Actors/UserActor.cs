using Akka.Actor;
using MovieStreamingActorSystem.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieStreamingActorSystem.Actors
{
    public class UserActor : ReceiveActor
    {
        // Add State
        private readonly int _userId;
        private string _currentlyWatching;

        public UserActor(int userId)
        {
            _userId = userId;

            // Initial Behaviour
            Stopped();
        }

        private void Playing()
        {
            Receive<PlayMovieMessage>(message => ColourConsole.WriteRedLine("ERROR: Cannot start playing another movie before stopping the existing one!"));
            Receive<StopMovieMessage>(message => StopPlayingCurrentMovie());

            ColourConsole.WriteCyanLine("UserActor has now become Playing");
        }

        private void Stopped()
        {
            Receive<PlayMovieMessage>(message => StartPlayingMovie(message.MovieTitle));
            Receive<StopMovieMessage>(message => ColourConsole.WriteRedLine("ERROR: cannot stop if nothing is playing"));

            ColourConsole.WriteCyanLine("UserActor has now become stopped");
        }

        private void StartPlayingMovie(string title)
        {
            _currentlyWatching = title;

            ColourConsole.WriteYellowLine($"User is currently watching {_currentlyWatching}");

            Context.ActorSelection("/user/Playback/PlaybackStatistics/MoviePlayCounter").Tell(new IncrementPlayCountMessage(title));


            // Set the current state of the actor to playing
            Become(Playing);
        }

        private void StopPlayingCurrentMovie()
        {
            ColourConsole.WriteYellowLine($"Stopping movie {_currentlyWatching}");
            _currentlyWatching = null;
            Become(Stopped);
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