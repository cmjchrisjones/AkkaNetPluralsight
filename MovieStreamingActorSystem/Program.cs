using Akka.Actor;
using MovieStreamingActorSystem.Actors;
using MovieStreamingActorSystem.Messages;
using System;

namespace MovieStreamingActorSystem
{
    public class Program
    {
        private static ActorSystem MovieStreamingActorSystem;

        static void Main(string[] args)
        {
            MovieStreamingActorSystem = ActorSystem.Create("MovieStreamingActorSystem");
            Console.WriteLine("Actor System Created");

            // Untyped Actor sending primitive objects

            var playbackActorThatInheritsUntypedActorAndHandlesPrimitiveObjectProps = Props.Create<PlaybackActorThatInheritsUntypedActorAndHandlesPrimitiveObjects>();
            var playbackActorThatInheritsUntypedActorAndHandlesPrimitiveObjectRef = MovieStreamingActorSystem.ActorOf(playbackActorThatInheritsUntypedActorAndHandlesPrimitiveObjectProps, "playbackActorThatHandlesPrimitiveObjectProps");

            // Send Message
            playbackActorThatInheritsUntypedActorAndHandlesPrimitiveObjectRef.Tell("AKKA.NET: The Movie"); // This sends a string to the actor which is handled
            playbackActorThatInheritsUntypedActorAndHandlesPrimitiveObjectRef.Tell(42); // This sends an int to the actor which is handled
            playbackActorThatInheritsUntypedActorAndHandlesPrimitiveObjectRef.Tell('c'); // This sends a char to the actors which is NOT handled, but won't throw an exception

            // Untyped Actor but using the message class
            var playbackActorThatInheritsUntypedActorAndHandlesMessagesProps = Props.Create<PlaybackActorThatInheritsUntypedActorAndHandlesMessages>();
            var playbackActorThatInheritsUntypedActorAndHandlesMessagesRef = MovieStreamingActorSystem.ActorOf(playbackActorThatInheritsUntypedActorAndHandlesMessagesProps, "playbackActorThatInheritsUntypedActorAndHandlesMessagesProps");

            // Send Message
            var message = new PlayMovieMessage("AKKA.NET: The Movie", 42);
            playbackActorThatInheritsUntypedActorAndHandlesMessagesRef.Tell(message);

            // Receive Actor that handles the message class
            var playbackActorThatInheritsReceiveActorAndHandlesMessagesProps = Props.Create<PlaybackActorThatInheritsReceiveActorAndHandlesMessages>();
            var playbackActorThatInheritsReceiveActorAndHandlesMessagesRef = MovieStreamingActorSystem.ActorOf(playbackActorThatInheritsReceiveActorAndHandlesMessagesProps, "playbackActorThatInheritsReceiveActorAndHandlesMessagesProps");

            playbackActorThatInheritsReceiveActorAndHandlesMessagesRef.Tell(message);

            Console.ReadLine();
            MovieStreamingActorSystem.Terminate();
        }
    }
}
