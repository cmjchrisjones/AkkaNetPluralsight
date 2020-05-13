using Akka.Actor;
using MovieStreamingActorSystem.Messages;
using System;

namespace MovieStreamingActorSystem.Actors
{
    public class PlaybackActorThatInheritsUntypedActorAndHandlesMessages : UntypedActor
    {
        public PlaybackActorThatInheritsUntypedActorAndHandlesMessages()
        {
            Console.WriteLine($"{this.GetType()} has been created");
        }
        protected override void OnReceive(object message)
        {
            if(message is PlayMovieMessage)
            {
                var m = message as PlayMovieMessage;
                Console.WriteLine($"The movie title is: {m.MovieTitle} and user is {m.UserId}.");
            }
        }
    }
}
