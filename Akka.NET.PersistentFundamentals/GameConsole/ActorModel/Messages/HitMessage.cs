using System;
using System.Collections.Generic;
using System.Text;

namespace GameConsole.ActorModel.Messages
{
    class HitMessage
    {
        public int Damage { get; private set; }

        public HitMessage(int damage)
        {
            Damage = damage;
        }
    }
}
