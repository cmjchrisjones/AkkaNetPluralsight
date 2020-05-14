using Akka.Actor;
using Akka.Event;
using MovieStreaming.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieStreaming.Actors
{
    public class UserCoordinatorActor : ReceiveActor
    {
        private readonly Dictionary<int, IActorRef> _users;
        private readonly ILoggingAdapter _logger = Context.GetLogger();

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

                _logger.Info($"UserCoordinatorActor create a new child UserActor for {userId} (Total users: {_users.Count})");
            }
        }

        protected override void PreStart()
        {
            _logger.Debug("UserCoordinator Actor PreStart");
        }

        protected override void PostStop()
        {
            _logger.Debug("UserCoordinator Actor PostStop");
        }

        protected override void PreRestart(Exception reason, object message)
        {
            _logger.Debug($"UserCoordinator Actor PreRestart because {reason}.");
            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
            _logger.Debug($"UserCoordinator Actor PostRestart because {reason}.");
            base.PostRestart(reason);
        }
    }
}
