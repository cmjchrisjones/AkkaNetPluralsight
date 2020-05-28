using Akka.Actor;
using GameConsole.ActorModel.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameConsole.ActorModel.Actors
{
    class PlayerActor : ReceiveActor
    {
        private readonly string _playerName;
        private int _health;

        public PlayerActor(string playerName, int defaultStartingHealth)
        {
            _playerName = playerName;
            _health = defaultStartingHealth;

            DisplayHelper.WriteLine($"{_playerName} has been created");

            Receive<HitMessage>(message => HitPlayer(message));
            Receive<DisplayStatusMessage>(message => DisplayPlayerStatus());
            Receive<CauseErrorMessage>(message => SimulateError());
        }

        private void HitPlayer(HitMessage message)
        {
            DisplayHelper.WriteLine($"{_playerName} received HitMessage");
            _health -= message.Damage;
        }

        private void DisplayPlayerStatus()
        {
            DisplayHelper.WriteLine($"{_playerName} recieved DisplayStatusMessage");
            Console.WriteLine($"{_playerName} has {_health} health");
        }

        private void SimulateError()
        {
            DisplayHelper.WriteLine($"{_playerName} received CauseErrorMessage");
            throw new ApplicationException($"Simulated exception in player {_playerName}");
        }
    }
}
