using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Threading;

namespace ActorModel
{
    public class StatisticsActor : ReceiveActor
    {
        public Dictionary<string, int> PlayCounts { get; set; }

        public StatisticsActor()
        {
            Receive<InitialStatisticsMessage>(m => HandleInitialMessage(m));
            Receive<string>(title => HandleTitleMessage(title));
        }

        public void HandleInitialMessage(InitialStatisticsMessage m)
        {
            PlayCounts = new Dictionary<string, int>(m.PlayCounts);
        }

        public void HandleTitleMessage(string title)
        {
            if (PlayCounts.ContainsKey(title))
            {
                PlayCounts[title]++;
            }
            else
            {
                PlayCounts.Add(title, 1);
            }
        }
    }
}
