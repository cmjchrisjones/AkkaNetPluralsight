using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.Xunit2;
using FluentAssertions;
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
            StatisticsActor actor = new StatisticsActor();

            // Act

            // Assert
            actor.PlayCounts.Should().BeNull();
        }

        [Fact] // Direct Test (bypasses the receive handler on the actor under test)
        public void ShouldSetInitialPlayCounts()
        {
            // Arrange
            StatisticsActor actor = new StatisticsActor();

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
            TestActorRef<StatisticsActor> actor = ActorOfAsTestActorRef<StatisticsActor>();

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
            var statisticsActor = ActorOfAsTestActorRef<StatisticsActor>();

            // Act
            var initialMovieStats = new Dictionary<string, int>();
            initialMovieStats.Add("Codenan the Barbarian", 42);
            statisticsActor.Tell(new InitialStatisticsMessage(new ReadOnlyDictionary<string, int>(initialMovieStats)));

            statisticsActor.Tell("Codenan the Barbarian");

            // Assert
            statisticsActor.UnderlyingActor.PlayCounts["Codenan the Barbarian"].Should().Be(43);
        }

    }
}
