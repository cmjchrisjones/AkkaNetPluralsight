using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ActorModel.Tests
{
    class MockDatabaseActor : ReceiveActor

    {
        public MockDatabaseActor()
        {
            Receive<GetInitialStatisticsMessage>(
                message =>
                {
                    var stats = new Dictionary<string, int>
                    {
                        { "Codenan the Barbarian", 42 }
                    };
                    Sender.Tell(new InitialStatisticsMessage(new ReadOnlyDictionary<string, int>(stats)));
                });
        }
    }
}
