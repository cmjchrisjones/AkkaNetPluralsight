using Akka.Actor;
using ReactiveStock.ActorModel.Messages;
using System;
using System.Collections.Generic;

namespace ReactiveStock.ActorModel.Actors
{
    class StocksCoordinatorActor : ReceiveActor
    {
        private readonly IActorRef _chartingActor;
        private readonly Dictionary<string, IActorRef> _stockActors;

        public StocksCoordinatorActor(IActorRef chartingActor)
        {
            _chartingActor = chartingActor;

            _stockActors = new Dictionary<string, IActorRef>();

            Receive<WatchStockMessage>(message => WatchStock(message));
            Receive<UnWatchStockMessage>(message => UnWatchStock(message));
        }

        private void WatchStock(WatchStockMessage message)
        {
            // Check in the dictionary to see if the stock is already being watched
            bool childActorNeedsCreating = !_stockActors.ContainsKey(message.StockSymbol);

            // If the stock is not being watched, create a child actor
            if (childActorNeedsCreating)
            {
                // create the child actor with the name StockActor_{stock} EG StockActor_AAPL
                IActorRef newChildActor = Context.ActorOf(Props.Create(() => new StockActor(message.StockSymbol)), $"StockActor_{message.StockSymbol}");

                // Add the child into the dictionary of stock actors
                _stockActors.Add(message.StockSymbol, newChildActor);
            }

            // Tell the charting actor to add a new line to the chart
            _chartingActor.Tell(new AddChartSeriesMessage(message.StockSymbol));

            // Subscribe to new stock prices - pub/sub in action, signing up the charting actor to receive new stock price messages
            _stockActors[message.StockSymbol].Tell(new SubscribeToNewStockPricesMessage(_chartingActor));
        }

        private void UnWatchStock(UnWatchStockMessage message)
        {
            // Check to see if the stock is currently being watched
            if (!_stockActors.ContainsKey(message.StockSymbol))
            {
                // if its not being watched, just return
                return;
            }

            // Remove the stock from our chart
            _chartingActor.Tell(new RemoveChartSeriesMessage(message.StockSymbol));

            // Unsubscribe from getting any new stock prices
            _stockActors[message.StockSymbol].Tell(new UnSubscribeFromNewStockPriceMessage(_chartingActor));
        }
    }
}
