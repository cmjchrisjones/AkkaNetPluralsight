using Akka.Actor;
using System.Collections.ObjectModel;

namespace ActorModel
{
    public class DatabaseActor : ReceiveActor
    {
        private IDatabaseGateway _databaseGateway;

        public DatabaseActor(IDatabaseGateway databaseGateway)
        {
            _databaseGateway = databaseGateway;


            Receive<GetInitialStatisticsMessage>(m =>
            {
                var storedStats = _databaseGateway.GetStoredStatistics();

                Sender.Tell(new InitialStatisticsMessage(new ReadOnlyDictionary<string, int>(storedStats)));
            });
        }
    }
}
