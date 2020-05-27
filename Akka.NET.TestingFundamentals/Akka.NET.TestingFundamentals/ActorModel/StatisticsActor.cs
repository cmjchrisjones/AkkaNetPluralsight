using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Threading;

namespace ActorModel
{
    public class StatisticsActor : ReceiveActor
    {
        public Dictionary<string, int> PlayCounts {get;set;}

        public StatisticsActor()
        {
            Receive<InitialStatisticsMessage>(m => HandleInitialMessage(m));
        }

        public void HandleInitialMessage(InitialStatisticsMessage m)
        {
            PlayCounts = new Dictionary<string, int>(m.PlayCounts);
        }
    }
}
