using System;
using System.Collections.Generic;
using System.Text;

namespace GameConsole.ActorModel.Messages
{
    class CreatePlayerMessage
    {
        public CreatePlayerMessage(string playerName)
        {
            PlayerName = playerName;
        }

        public string PlayerName { get; private set; }

    }
}
