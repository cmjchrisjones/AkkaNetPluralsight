using System;

namespace ReactiveStock.ActorModel.Messages
{
    class RefreshStockPriceMessage
    {
        public string StockSymbol { get; private set; }

        public RefreshStockPriceMessage(string stockSymbol)
        {
            StockSymbol = stockSymbol;
        }
    }
}
