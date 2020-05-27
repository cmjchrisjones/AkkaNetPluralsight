using Akka.TestKit;
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
            TestActorRef<UserActor> userActor = ActorOfAsTestActorRef<UserActor>();

            // Act

            // Assert
            userActor.UnderlyingActor.CurrentlyPlaying.Should().BeNull();
        }
    }
}
