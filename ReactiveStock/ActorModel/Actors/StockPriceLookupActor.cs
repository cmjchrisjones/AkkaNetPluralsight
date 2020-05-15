using Akka.Actor;
using ReactiveStock.ActorModel.ExternalServices;
using ReactiveStock.ActorModel.Messages;
using System;

namespace ReactiveStock.ActorModel.Actors
{
    class StockPriceLookupActor : ReceiveActor
    {
        private readonly IStockPriceServiceGateway stockPriceServiceGateway;

        public StockPriceLookupActor(IStockPriceServiceGateway stockPriceServiceGateway)
        {
            this.stockPriceServiceGateway = stockPriceServiceGateway;

            Receive<RefreshStockPriceMessage>(message => LookupStockPrice(message));

        }

        private void LookupStockPrice(RefreshStockPriceMessage message)
        {
            var latestPrice = stockPriceServiceGateway.GetLatestPrice(message.StockSymbol);
            Sender.Tell(new UpdatedStockPriceMessage(latestPrice, DateTime.Now));
        }
    }
}
