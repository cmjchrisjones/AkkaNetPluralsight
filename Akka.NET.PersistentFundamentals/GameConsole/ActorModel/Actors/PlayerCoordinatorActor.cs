using Akka.Actor;
using GameConsole.ActorModel.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameConsole.ActorModel.Actors
{
    class PlayerCoordinatorActor : ReceiveActor
    {
        private const int DefaultStartingHealth = 100;

        public PlayerCoordinatorActor()
        {
            Receive<CreatePlayerMessage>(m =>
            {
                DisplayHelper.WriteLine($"PlayerCoordinatorActor received CreatePlayerMessage for {m.PlayerName}");

                Context.ActorOf(Props.Create(() => new PlayerActor(m.PlayerName, DefaultStartingHealth)), m.PlayerName);
            });
        }
    }
}
