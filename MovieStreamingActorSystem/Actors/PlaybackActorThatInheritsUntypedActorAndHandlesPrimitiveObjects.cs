using Akka.Actor;
using System;

namespace MovieStreamingActorSystem.Actors
{
    public class PlaybackActorThatInheritsUntypedActorAndHandlesPrimitiveObjects : UntypedActor
    {
        public PlaybackActorThatInheritsUntypedActorAndHandlesPrimitiveObjects()
        {
            Console.WriteLine($"{this.GetType()} has been created");
        }
        protected override void OnReceive(object message)
        {
            if (message is string)
            {
                Console.WriteLine($"The movie title is: {message}");
            }
            else if (message is int)
            {
                Console.WriteLine($"The use id is: {message}");
            }
            else
            {
                Unhandled(message);
            }
        }
    }
}
