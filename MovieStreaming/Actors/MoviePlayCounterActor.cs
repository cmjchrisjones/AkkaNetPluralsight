using Akka.Actor;
using Akka.Event;
using MovieStreaming.Exceptions;
using MovieStreaming.Messages;
using System;
using System.Collections.Generic;

namespace MovieStreaming.Actors
{
    public class MoviePlayCounterActor : ReceiveActor
    {
        private readonly Dictionary<string, int> _moviePlayCounts;
        private readonly ILoggingAdapter _logger = Context.GetLogger();


        public MoviePlayCounterActor()
        {
            _moviePlayCounts = new Dictionary<string, int>();

            Receive<IncrementPlayCountMessage>(message => HandleIncrementMessage(message));
        }

        private void HandleIncrementMessage(IncrementPlayCountMessage message)
        {
            if (_moviePlayCounts.ContainsKey(message.MovieTitle))
            {
                _moviePlayCounts[message.MovieTitle]++;
            }
            else
            {
                _moviePlayCounts.Add(message.MovieTitle, 1);
            }

            // Simulated Bugs
            if(message.MovieTitle == "Partial Recoil")
            {
                throw new SimulatedTerribleMovieException("Simulated Exception");
            }

            if(message.MovieTitle == "Partial Recoil 2")
            {
                throw new InvalidOperationException("Simulated Exception");
            }
             
            _logger.Info($"MoviePlayCounterActor {message.MovieTitle} has been watched {_moviePlayCounts[message.MovieTitle]} times");
        }
    }
}