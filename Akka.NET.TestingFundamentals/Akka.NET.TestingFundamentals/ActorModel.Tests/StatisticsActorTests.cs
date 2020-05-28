using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.TestActors;
using Akka.TestKit.Xunit2;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xunit;

namespace ActorModel.Tests
{
    public class StatisticsActorTests : TestKit
    {
        [Fact]
        public void ShouldHaveInitialPlayCountsValue()
        {
            // Arrange
            StatisticsActor actor = new StatisticsActor(null);

            // Act

            // Assert
            actor.PlayCounts.Should().BeNull();
        }

        [Fact] // Direct Test (bypasses the receive handler on the actor under test)
        public void ShouldSetInitialPlayCounts()
        {
            // Arrange
            StatisticsActor actor = new StatisticsActor(null);

            // Act
            var initalMovieStats = new Dictionary<string, int>();
            initalMovieStats.Add("Codenan the Barbarian", 42);

            actor.HandleInitialMessage(new InitialStatisticsMessage(new ReadOnlyDictionary<string, int>(initalMovieStats)));

            // Assert
            actor.PlayCounts["Codenan the Barbarian"].Should().Be(42);
        }

        [Fact] // Unit test using the ActorOfTestActorRef
        public void ShouldReceiveInitialStatisticsMessage()
        {
            // Arrange
            TestActorRef<StatisticsActor> actor = ActorOfAsTestActorRef(() => new StatisticsActor(ActorOf(BlackHoleActor.Props)));

            // Act
            var initalMovieStats = new Dictionary<string, int>();
            initalMovieStats.Add("Codenan the Barbarian", 42);

            actor.Tell(new InitialStatisticsMessage(new ReadOnlyDictionary<string, int>(initalMovieStats)));

            // Assert
            actor.UnderlyingActor.PlayCounts["Codenan the Barbarian"].Should().Be(42);
        }

        // INTEGRATION TESTS

        [Fact]
        public void ShouldUpdatePlayCounts()
        {
            // Arrange
            var statisticsActor = ActorOfAsTestActorRef(() => new StatisticsActor(ActorOf(BlackHoleActor.Props)));

            // Act
            var initialMovieStats = new Dictionary<string, int>();
            initialMovieStats.Add("Codenan the Barbarian", 42);
            statisticsActor.Tell(new InitialStatisticsMessage(new ReadOnlyDictionary<string, int>(initialMovieStats)));

            statisticsActor.Tell("Codenan the Barbarian");

            // Assert
            statisticsActor.UnderlyingActor.PlayCounts["Codenan the Barbarian"].Should().Be(43);
        }

        [Fact]
        public void ShouldGetInitialStatsFromDatabase()
        {
            // Arrange
            var mockDatabaseActor = ActorOfAsTestActorRef<MockDatabaseActor>();

            var actor = ActorOfAsTestActorRef(() => new StatisticsActor(mockDatabaseActor));

            // Act

            // Assert
            actor.UnderlyingActor.PlayCounts["Codenan the Barbarian"].Should().Be(42);
        }

        [Fact]
        public void ShouldGetInitialStatsFromDatabaseUsingTestProbesInsteadOfHandWrittenMockActors()
        {
            // Arrange
            var mockDatabaseActor = CreateTestProbe();
            var messageHandler = new DelegateAutoPilot((sender, message) =>
            {
                if (message is GetInitialStatisticsMessage)
                {
                    var stats = new Dictionary<string, int>
                {
                    {"Codenan the Barbarian", 42 }
                };

                    sender.Tell(new InitialStatisticsMessage(new ReadOnlyDictionary<string, int>(stats)));
                }
                return AutoPilot.KeepRunning;
            });

            mockDatabaseActor.SetAutoPilot(messageHandler);

            var actor = ActorOfAsTestActorRef(() => new StatisticsActor(mockDatabaseActor));

            // Act

            // Assert
            actor.UnderlyingActor.PlayCounts["Codenan the Barbarian"].Should().Be(42);
        }

        [Fact]
        public void ShouldAskDatabaseForInitialStats()
        {
            // Arrange
            var mockDb = CreateTestProbe();
            var actor = ActorOf(() => new StatisticsActor(mockDb));

            // Act

            // Assert
            mockDb.ExpectMsg<GetInitialStatisticsMessage>();
        }
    }
}
