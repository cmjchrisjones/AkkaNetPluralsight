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
                Props.Create(() => new UserActor(ActorOf(BlackHoleActor.Props))));

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
                Props.Create(() => new UserActor(ActorOf(BlackHoleActor.Props))));

            // Act
            actor.Tell(new PlayMovieMessage("Codenan the Barbarian"));

            // Assert
            var message = ExpectMsg<NowPlayingMessage>(TimeSpan.FromSeconds(5));
            message.CurrentlyPlaying.Should().Be("Codenan the Barbarian");
        }

        [Fact]
        public void ShouldLogPlayMovie()
        {
            // Arrange
            var actor = ActorOf(Props.Create(() => new UserActor(ActorOf(BlackHoleActor.Props))));

            // Assert
            EventFilter.Info("Started playing Boolean Lies")
                .And
                .Info("Replying to sender")
                .Expect(2, () =>
            actor.Tell(new PlayMovieMessage("Boolean Lies")));
        }

        [Fact]
        public void ShouldSendToDeadLetters()
        {
            // Arrange
            var actor = ActorOf(Props.Create(() => new UserActor(ActorOf(BlackHoleActor.Props))));

            EventFilter.DeadLetter<PlayMovieMessage>(
                message => message.MovieTitle == "Boolean Lies")
                .ExpectOne(() => actor.Tell(new PlayMovieMessage("Boolean Lies")));
            // Act

            // Assert

        }

        [Fact]
        public void ShouldErrorOnUnknownMovie()
        {
            // Arrange
            var actor = ActorOf(Props.Create(() => new UserActor(ActorOf(BlackHoleActor.Props))));

            EventFilter.Exception<NotSupportedException>()
                .ExpectOne(() => actor.Tell(new PlayMovieMessage("Null Terminator")));
            // Act

            // Assert
        }

        [Fact]
        public void ShouldPublishPlayingMovie()
        {
            // Arrange
            var actor = ActorOf(Props.Create(() => new UserActor(ActorOf(BlackHoleActor.Props))));

            var subscriber = CreateTestProbe();

            // Act
            Sys.EventStream.Subscribe(subscriber, typeof(NowPlayingMessage));
            actor.Tell(new PlayMovieMessage("Codenan the Barbarian"));

            // Assert
            subscriber.ExpectMsg<NowPlayingMessage>(m => m.CurrentlyPlaying == "Codenan the Barbarian");
        }


        [Fact]
        public void ShouldTerminate()
        {
            // Arrange
            var actor = ActorOf(Props.Create(() => new UserActor(ActorOf(BlackHoleActor.Props))));

            Watch(actor);

            // Assert
            actor.Tell(PoisonPill.Instance);

            // Assert
            ExpectTerminated(actor);
        }
    }
}
