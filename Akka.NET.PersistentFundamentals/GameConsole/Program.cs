using Akka.Actor;
using GameConsole.ActorModel.Actors;
using GameConsole.ActorModel.Messages;
using System;
using System.Threading;
using static System.Console;

namespace GameConsole
{
    class Program
    {
        private static ActorSystem System { get; set; }

        private static IActorRef PlayerCoordinator { get; set; }

        static void Main(string[] args)
        {
            System = ActorSystem.Create("Game");

            PlayerCoordinator = System.ActorOf<PlayerCoordinatorActor>("PlayerCoordinator");

            ForegroundColor = ConsoleColor.White;

            DisplayInstructions();

            while (true)
            {
                Thread.Sleep(2000); // Ensure console colour is set back to white
                ForegroundColor = ConsoleColor.White;

                var action = ReadLine();

                var playerName = action.Split(' ')[0];

                if (action.Contains("create"))
                {
                    CreatePlayer(playerName);
                }
                else if (action.Contains("hit"))
                {
                    var damage = int.Parse(action.Split(' ')[2]);
                    HitPlayer(playerName, damage);
                }
                else if (action.Contains("display"))
                {
                    DisplayPlayer(playerName);
                }
                else if (action.Contains("error"))
                {
                    ErrorPlayer(playerName);
                }
                else
                {
                    WriteLine("Unknown Command");
                }
            }
        }

        private static void ErrorPlayer(string playerName)
        {
            System.ActorSelection($"/user/PlayerCoordinator/{playerName}").Tell(new CauseErrorMessage());
        }

        private static void DisplayPlayer(string playerName)
        {
            System.ActorSelection($"/user/PlayerCoordinator/{playerName}").Tell(new DisplayStatusMessage());
        }

        private static void HitPlayer(string playerName, int damage)
        {
            System.ActorSelection($"/user/PlayerCoordinator/{playerName}").Tell(new HitMessage(damage));
        }

        private static void CreatePlayer(string playerName)
        {
            PlayerCoordinator.Tell(new CreatePlayerMessage(playerName));
        }

        private static void DisplayInstructions()
        {
            Thread.Sleep(2000);
            ForegroundColor = ConsoleColor.White;

            WriteLine("Available Commands:");
            WriteLine("<playername> create");
            WriteLine("<playername> hit");
            WriteLine("<playername> display");
            WriteLine("<playername> error");
        }
    }
}
