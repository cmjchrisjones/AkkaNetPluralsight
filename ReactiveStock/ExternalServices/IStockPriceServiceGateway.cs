using System;
using System.Collections.Generic;
using System.Text;

namespace ReactiveStock.ExternalServices
{
    interface IStockPriceServiceGateway
    {
        decimal GetLatestPrice(string stockSymbol);
    }
}
