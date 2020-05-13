using Akka.Actor;
using MovieStreamingActorSystem.Messages;
using System;

namespace MovieStreamingActorSystem.Actors
{
    public class UserActorRefactoredToSwitchableBehavioursRefactoredToSwitchableBehaviours : ReceiveActor
    {
        // Add State
        private string _currentlyWatching;

        public UserActorRefactoredToSwitchableBehavioursRefactoredToSwitchableBehaviours()
        {
            Console.WriteLine("Creating a UserActorRefactoredToSwitchableBehaviours");
            ColourConsole.WriteCyanLine("Setting inital behaviour to stopped");

            // Initial Behaviour
            Stopped();
        }

        private void Playing()
        {
            Receive<PlayMovieMessage>(message => ColourConsole.WriteRedLine("ERROR: Cannot start playing another movie before stopping the existing one!"));
            Receive<StopMovieMessage>(message => StopPlayingCurrentMovie());

            ColourConsole.WriteCyanLine("UserActorRefactoredToSwitchableBehaviours has now become Playing");
        }

        private void Stopped()
        {
            Receive<PlayMovieMessage>(message => StartPlayingMovie(message.MovieTitle));
            Receive<StopMovieMessage>(message => ColourConsole.WriteRedLine("ERROR: cannot stop if nothing is playing"));

            ColourConsole.WriteCyanLine("UserActorRefactoredToSwitchableBehaviours has now become stopped");
        }

        private void StartPlayingMovie(string title)
        {
            _currentlyWatching = title;

            ColourConsole.WriteYellowLine($"User is currently watching {_currentlyWatching}");

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
            ColourConsole.WriteGreenLine("UserActorRefactoredToSwitchableBehaviours PreStart");
        }

        protected override void PostStop()
        {
            ColourConsole.WriteGreenLine("UserActorRefactoredToSwitchableBehaviours PostStop");
        }

        protected override void PreRestart(Exception reason, object message)
        {
            ColourConsole.WriteRedLine($"UserActorRefactoredToSwitchableBehaviours PreRestart because {reason}.");
            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
            ColourConsole.WriteRedLine($"UserActorRefactoredToSwitchableBehaviours PostRestart because reason {reason}.");
            base.PostRestart(reason);
        }
    }
}
