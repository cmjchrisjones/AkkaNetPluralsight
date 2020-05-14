﻿using Akka.Actor;
using Akka.Configuration;
using MovieStreaming.Actors;
using MovieStreaming.Messages;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Linq;

namespace MovieStreaming
{
    class Program
    {
        private static ActorSystem MovieStreamingActorSystem;

        static void Main(string[] args)
        {
            // Solution to read hocon from StackOverflow: https://stackoverflow.com/a/56459986/1798229
            var hocon = XElement.Parse(File.ReadAllText(Directory.GetCurrentDirectory() + "\\hocon.conf"));
            var config = ConfigurationFactory.ParseString(hocon.Descendants("hocon").Single().Value);


            MovieStreamingActorSystem = ActorSystem.Create("MovieStreamingActorSystem", config);
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
            Thread.Sleep(450);
        }
    }
}
