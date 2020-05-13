using Akka.Actor;
using MovieStreamingActorSystem.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieStreamingActorSystem.Actors
{
    public class UserCoordinatorActor : ReceiveActor
    {
        private readonly Dictionary<int, IActorRef> _users;

        public UserCoordinatorActor()
        {
            _users = new Dictionary<int, IActorRef>();

            Receive<PlayMovieMessage>(message =>
            {
                CreateChildIfNotExists(message.UserId);
                IActorRef childActorRef = _users[message.UserId];
                childActorRef.Tell(message);
            });

            Receive<StopMovieMessage>(message =>
            {
                CreateChildIfNotExists(message.UserId);
                IActorRef childActorRef = _users[message.UserId];
                childActorRef.Tell(message);
            });
        }

        private void CreateChildIfNotExists(int userId)
        {
            if (!_users.ContainsKey(userId))
            {
                IActorRef newChildActorRef = Context.ActorOf(Props.Create(() => new UserActor(userId)), "User" + userId);
                _users.Add(userId, newChildActorRef);

                ColourConsole.WriteCyanLine($"UserCoordinatorActor create a new child UserActor for {userId} (Total users: {_users.Count})");
            }
        }

        protected override void PreStart()
        {
            ColourConsole.WriteCyanLine("UserCoordinatorActor Actor PreStart");
        }

        protected override void PostStop()
        {
            ColourConsole.WriteCyanLine("UserCoordinatorActor Actor PostStop");
        }

        protected override void PreRestart(Exception reason, object message)
        {
            ColourConsole.WriteCyanLine($"UserCoordinatorActor Prerestart because {reason}.");
            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
            ColourConsole.WriteCyanLine($"UserCoordinatorActor Postrestart because {reason}.");
            base.PostRestart(reason);
        }
    }
}
