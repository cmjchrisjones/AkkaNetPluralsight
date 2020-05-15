using System;
using System.Collections.Generic;
using System.Text;

namespace ReactiveStock.ActorModel.ExternalServices
{
    interface IStockPriceServiceGateway
    {
        decimal GetLatestPrice(string stockSymbol);
    }
}
