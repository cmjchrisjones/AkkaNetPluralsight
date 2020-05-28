using Akka.Actor;
using Akka.TestKit.TestActors;
using Akka.TestKit.Xunit2;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xunit;

namespace ActorModel.Tests
{
    public class IntegrationTests : TestKit
    {
        [Fact]
        public void UserShouldUpdatePlayCounts()
        {
            // Arrange
            var stats = ActorOfAsTestActorRef(() => new StatisticsActor(ActorOf(BlackHoleActor.Props)));

            // Act
            var initialMovieStats = new Dictionary<string, int>();
            initialMovieStats.Add("Codenan the Barbarian", 42);

            stats.Tell(new InitialStatisticsMessage(new ReadOnlyDictionary<string, int>(initialMovieStats)));

            var userActor = ActorOfAsTestActorRef<UserActor>(Props.Create(() => new UserActor(stats)));

            userActor.Tell(new PlayMovieMessage("Codenan the Barbarian"));

            // Assert
            stats.UnderlyingActor.PlayCounts["Codenan the Barbarian"].Should().Be(43);
        }
    }
}
