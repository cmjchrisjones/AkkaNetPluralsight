using System.Collections.Generic;

namespace ActorModel
{
    public interface IDatabaseGateway
    {
        Dictionary<string, int> GetStoredStatistics();
    }
}
