using System;
using System.Collections.Generic;
using System.Text;

namespace ReactiveStock.ActorModel.Messages
{
    class StockPriceMessage
    {
        public string StockSymbol;
        public decimal StockPrice;
        public DateTime Date;

        public StockPriceMessage(string stockSymbol, decimal stockPrice, DateTime date)
        {
            StockSymbol = stockSymbol;
            StockPrice = stockPrice;
            Date = date;
        }
    }
}
