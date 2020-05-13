using Akka.Actor;
using MovieStreamingActorSystem.Actors;
using MovieStreamingActorSystem.Messages;
using System;
using System.Drawing;
using System.Threading;

namespace MovieStreamingActorSystem
{
    public class Program
    {
        private static ActorSystem MovieStreamingActorSystem;

        static void Main(string[] args)
        {
            ColourConsole.WriteGrayLine("Creating MovieStreamingActorSystem");
            MovieStreamingActorSystem = ActorSystem.Create("MovieStreamingActorSystem");

            ColourConsole.WriteGrayLine("Creating Actor supervisory hierarchy");

            // Create our top level actor (Playback Actor)
            MovieStreamingActorSystem.ActorOf(Props.Create<PlaybackActor>(), "Playback");

            do
            {
                ShortPause();

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                ColourConsole.WriteGrayLine("Enter a Command and press enter");

                var command = Console.ReadLine();

                if (command.ToLower().StartsWith("play"))
                {
                    int userId = int.Parse(command.Split(',')[1]);
                    string movieTitle = command.Split(',')[2];

                    var message = new PlayMovieMessage(movieTitle, userId);
                    MovieStreamingActorSystem.ActorSelection("/user/Playback/UserCoordinator").Tell(message);
                }

                if (command.ToLower().StartsWith("stop"))
                {
                    int userId = int.Parse(command.Split(',')[1]);

                    var message = new StopMovieMessage(userId);
                    MovieStreamingActorSystem.ActorSelection("/user/Playback/UserCoordinator").Tell(message);
                }

                if (command.ToLower() == "exit")
                {
                    // Tell the actor system and all child actors to shutdown
                    MovieStreamingActorSystem.Terminate();

                    // Wait for actor system to finish shutting down
                    _ = MovieStreamingActorSystem.WhenTerminated;

                    ColourConsole.WriteGrayLine("Actor system has shut down");
                    Console.ReadKey();
                    Environment.Exit(1);
                }
            }
            while (true);
        }

        static void ShortPause()
        {
            Thread.Sleep(4000);
        }
    }
}
