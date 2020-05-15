using Akka.Actor;
using Akka.DI.Core;
using ReactiveStock.ActorModel.Messages;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ReactiveStock.ActorModel.Actors
{
    class StockActor : ReceiveActor
    {

        private readonly string _stockSymbol;

        // Store a list of subscribing actors
        private readonly HashSet<IActorRef> _subscribers;

        private decimal _stockPrice;

        // Reference for the child actor 'priceLookupChild'
        private readonly IActorRef _priceLookupChild;

        // This will allow us to cancel the schedule when actor is stopped
        private ICancelable _priceRefreshing;

        public StockActor(string stockSymbol)
        {
            _stockSymbol = stockSymbol;
            _subscribers = new HashSet<IActorRef>();


            // Create the PriceLookupChild Actor
            _priceLookupChild = Context.ActorOf(Context.DI().Props<StockPriceLookupActor>());

            // Add the actor to the subscribers when message is received
            Receive<SubscribeToNewStockPricesMessage>(message => _subscribers.Add(message.Subscriber));

            // Remove the actor from the subscribers when message is received
            Receive<UnSubscribeFromNewStockPriceMessage>(message => _subscribers.Remove(message.Subscriber));

            // Delegate any stock price messages received
            Receive<RefreshStockPriceMessage>(message => _priceLookupChild.Tell(message));

            // StockPriceLookupActor tells the sender (this actor) a new stock price message via UpdatedStockPriceMessage
            Receive<UpdatedStockPriceMessage>(message =>
            {
                _stockPrice = message.Price;

                // Construct a new StockPrice Message
                var stockPriceMessage = new StockPriceMessage(_stockSymbol, _stockPrice, message.Date);

                // Send it to all of the subscribed actors
                foreach (var subscribedActor in _subscribers)
                {
                    subscribedActor.Tell(stockPriceMessage);
                }
            });
        }
        protected override void PreStart()
        {
            // Repeatedly send ourself a message to refresh stock price until we get told to cancel
            _priceRefreshing = Context.System.Scheduler.ScheduleTellRepeatedlyCancelable(
                initialDelay: TimeSpan.FromSeconds(1),
                interval: TimeSpan.FromSeconds(1),
                receiver: Self,
                message: new RefreshStockPriceMessage(_stockSymbol),
                sender: Self);
        }

        protected override void PostStop()
        {
            // Make sure we stop the schedule when the actor stops
            _priceRefreshing.Cancel(false);
            base.PostStop();
        }
    }
}
