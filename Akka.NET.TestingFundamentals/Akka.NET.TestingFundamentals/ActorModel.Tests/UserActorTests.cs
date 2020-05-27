using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.TestActors;
using Akka.TestKit.Xunit2;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ActorModel.Tests
{
    public class UserActorTests : TestKit
    {
        [Fact]
        public void ShouldHaveInitialState()
        {
            // Arrange
            TestActorRef<UserActor> userActor = ActorOfAsTestActorRef<UserActor>(
                Props.Create(()=> new UserActor(ActorOf(BlackHoleActor.Props))));

            // Act

            // Assert
            userActor.UnderlyingActor.CurrentlyPlaying.Should().BeNull();
        }

        [Fact]
        public void ShouldUpdateCurrentlyPlayingState()
        {
            // Arrange
            var userActor = ActorOfAsTestActorRef<UserActor>(
                Props.Create(() => new UserActor(ActorOf(BlackHoleActor.Props))));

            // Act
            userActor.Tell(new PlayMovieMessage("Codenan the Barbarian"));

            // Assert
            userActor.UnderlyingActor.CurrentlyPlaying.Should().Be("Codenan the Barbarian");
        }

        [Fact]
        public void ShouldPlayMovie()
        {
            // Arrange
            var actor = ActorOfAsTestActorRef<UserActor>(
                Props.Create(()=> new UserActor(ActorOf(BlackHoleActor.Props))));

            // Act
            actor.Tell(new PlayMovieMessage("Codenan the Barbarian"));

            // Assert
            var message = ExpectMsg<NowPlayingMessage>(TimeSpan.FromSeconds(5));
            message.CurrentlyPlaying.Should().Be("Codenan the Barbarian");
        }
    }
}
