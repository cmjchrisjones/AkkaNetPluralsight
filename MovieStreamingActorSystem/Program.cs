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

            // Some additional messages
            var partialRecall = new PlayMovieMessage("Partial Recall", 99);
            var booleanLies = new PlayMovieMessage("Boolean Lies", 77);
            var codenanTheDestroyer = new PlayMovieMessage("Codenan the Destroyer", 1);

            // Actor with overridden life cycle methods

            var playbackActorThatInheritsReceiveActorAndHandlesMessagesWithOverriddenLifeCycleMethodsProps = Props.Create<PlaybackActorThatInheritsReceiveActorAndHandlesMessagesWithOverriddenLifeCycleMethods>();
            var playbackActorThatInheritsReceiveActorAndHandlesMessagesWithOverriddenLifeCycleMethodsRef = MovieStreamingActorSystem.ActorOf(playbackActorThatInheritsReceiveActorAndHandlesMessagesWithOverriddenLifeCycleMethodsProps, "playbackActorThatInheritsReceiveActorAndHandlesMessagesWithOverriddenLifeCycleMethodsProps");

            playbackActorThatInheritsReceiveActorAndHandlesMessagesWithOverriddenLifeCycleMethodsRef.Tell(message);
            playbackActorThatInheritsReceiveActorAndHandlesMessagesWithOverriddenLifeCycleMethodsRef.Tell(partialRecall);
            playbackActorThatInheritsReceiveActorAndHandlesMessagesWithOverriddenLifeCycleMethodsRef.Tell(booleanLies);
            playbackActorThatInheritsReceiveActorAndHandlesMessagesWithOverriddenLifeCycleMethodsRef.Tell(codenanTheDestroyer);

            playbackActorThatInheritsReceiveActorAndHandlesMessagesWithOverriddenLifeCycleMethodsRef.Tell(PoisonPill.Instance);

            // Behaviours

            // Switching to a new explicitly specified behaviour
            //   - Use Become() method
            //     Existing configured behaviour not remembered

            //   - Use Behaviour Stack (contains 2 methods)
            //   - BecomeStacked() - Switches to new behaviour and pushes existing behaviour down the behaviour stack
            //   - UnbecomeStacked() - pops the current behaviour off the stack and the previously pushed behaviour is restored

            var userActorProps = Props.Create<UserActor>();
            var userActorRef = MovieStreamingActorSystem.ActorOf(userActorProps, "userActor");
            
            Console.ReadKey();
            Console.WriteLine("Sending a PlayMovie Message for Codenan the Destroyer");
            userActorRef.Tell(codenanTheDestroyer);

            Console.ReadKey();
            Console.WriteLine("Sending a PlayMovie Message for BooleanLies");
            userActorRef.Tell(booleanLies);

            Console.ReadKey();
            Console.WriteLine("Sending a StopMovie Message");
            userActorRef.Tell(new StopMovieMessage());

            Console.ReadKey();
            Console.WriteLine("Sending a StopMovie Message");
            userActorRef.Tell(new StopMovieMessage());

            // Behaviour:  Refactored to switchable behaviours
            Console.WriteLine();
            Console.WriteLine("Refactorted UserActor to Switchable Behaviours");
            Console.WriteLine();
            var userActorRefactoredToSwitchableBehavioursProps = Props.Create<UserActorRefactoredToSwitchableBehavioursRefactoredToSwitchableBehaviours>();
            var userActorRefactoredToSwitchableBehavioursRef = MovieStreamingActorSystem.ActorOf(userActorRefactoredToSwitchableBehavioursProps, "userActorRefactoredToSwitchableBehaviours");

            Console.ReadKey();
            Console.WriteLine("Sending a PlayMovie Message for Codenan the Destroyer");
            userActorRefactoredToSwitchableBehavioursRef.Tell(codenanTheDestroyer);

            Console.ReadKey();
            Console.WriteLine("Sending a PlayMovie Message for BooleanLies");
            userActorRefactoredToSwitchableBehavioursRef.Tell(booleanLies);

            Console.ReadKey();
            Console.WriteLine("Sending a StopMovie Message");
            userActorRefactoredToSwitchableBehavioursRef.Tell(new StopMovieMessage());

            Console.ReadKey();
            Console.WriteLine("Sending a StopMovie Message");
            userActorRefactoredToSwitchableBehavioursRef.Tell(new StopMovieMessage());


            Console.ReadLine();

            // Tell the actor system and all child actors to shutdown
            MovieStreamingActorSystem.Terminate();

            // Wait for actor system to finish shutting down
            _ = MovieStreamingActorSystem.WhenTerminated;

            Console.WriteLine("Actor system has shut down");

            Console.ReadLine();

        }
    }
}
