using Akka.Actor;
using ReactiveStock.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReactiveStock.ActorModel.Actors
{
    class StockToggleButtonActor : ReceiveActor
    {
        private IActorRef _stocksCoordinatorActorRef;
        private StockToggleButtonViewModel _stockToggleButtonViewModel;
        private string _stockSymbol;

        public StockToggleButtonActor(IActorRef stocksCoordinatorActorRef, StockToggleButtonViewModel stockToggleButtonViewModel, string stockSymbol)
        {
            _stocksCoordinatorActorRef = stocksCoordinatorActorRef;
            _stockToggleButtonViewModel = stockToggleButtonViewModel;
            _stockSymbol = stockSymbol;
        }
    }
}
