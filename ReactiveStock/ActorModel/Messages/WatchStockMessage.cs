using System;
using System.Collections.Generic;
using System.Text;

namespace ReactiveStock.ActorModel.Messages
{
    class WatchStockMessage
    {
        public string StockSymbol { get; private set; }

        public WatchStockMessage(string stockSymbol)
        {
            StockSymbol = stockSymbol;
        }
    }
}
