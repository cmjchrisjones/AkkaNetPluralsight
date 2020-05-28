using Akka.Actor;
using Akka.TestKit.Xunit2;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace ActorModel.Tests
{
    public class DatabaseActorTests : TestKit
    {
        [Fact]
        public void ShouldReadStatsFromDatabase()
        {
            // Arrange
            var statsData = new Dictionary<string, int>
            {
                { "Boolean Lies", 42 },
                { "Codenan the Barbarian", 200 }
            };

            var mockDb = new Mock<IDatabaseGateway>();
            mockDb.Setup(x => x.GetStoredStatistics()).Returns(statsData);

            var actor = ActorOf(Props.Create(() => new DatabaseActor(mockDb.Object)));

            // Act
            actor.Tell(new GetInitialStatisticsMessage());

            // Assert
            var received = ExpectMsg<InitialStatisticsMessage>();
            received.PlayCounts["Codenan the Barbarian"].Should().Be(200);
            received.PlayCounts["Boolean Lies"].Should().Be(42);
        }
    }
}
