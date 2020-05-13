using Akka.Actor;
using MovieStreamingActorSystem.Messages;
using System;

namespace MovieStreamingActorSystem.Actors
{
    public class PlaybackActorThatInheritsReceiveActorAndHandlesMessages : ReceiveActor
    {
        public PlaybackActorThatInheritsReceiveActorAndHandlesMessages()
        {
            Console.WriteLine($"{this.GetType()} has been created");

            Receive<PlayMovieMessage>(message => HandlePlayMovieMessage(message));
        }

        private void HandlePlayMovieMessage(PlayMovieMessage message)
        {
            Console.WriteLine($"The movie title is: {message.MovieTitle} and user is {message.UserId}.");
        }
    }
}
