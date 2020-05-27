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


    }
}
