using Akka.Actor;
using MovieStreamingActorSystem.Messages;
using System;

namespace MovieStreamingActorSystem.Actors
{
    public class PlaybackActorThatInheritsReceiveActorAndHandlesMessagesWithOverriddenLifeCycleMethods : ReceiveActor
    {
        public PlaybackActorThatInheritsReceiveActorAndHandlesMessagesWithOverriddenLifeCycleMethods()
        {
            Console.WriteLine($"{this.GetType()} has been created");

            Receive<PlayMovieMessage>(message => HandlePlayMovieMessage(message));
        }

        private void HandlePlayMovieMessage(PlayMovieMessage message)
        {
            ColourConsole.WriteYellowLine($"The movie title is: {message.MovieTitle} and user is {message.UserId}.");
        }

        protected override void PreStart()
        {
            ColourConsole.WriteGreenLine($"{this.GetType()} Prestart");
        }

        protected override void PostStop()
        {
            ColourConsole.WriteRedLine($"{this.GetType()} PostStop");
        }

        protected override void PreRestart(Exception reason, object message)
        {
            ColourConsole.WriteYellowLine($"Restarted because {reason}");

            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
            ColourConsole.WriteYellowLine($"Restarted because {reason}");

            base.PostRestart(reason);
        }
    }
}
