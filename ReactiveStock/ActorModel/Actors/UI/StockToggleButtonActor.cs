using Akka.Actor;
using ReactiveStock.ActorModel.Messages;
using ReactiveStock.ViewModel;
using System;

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

            // Behaviours

            // Initial Behaviour
            ToggledOff();
        }

        private void ToggledOff()
        {
            Receive<FlipToggleMessage>(m =>
            {
                // Start watching a stock
                _stocksCoordinatorActorRef.Tell(new WatchStockMessage(_stockSymbol));


                // Change the button text to on
                _stockToggleButtonViewModel.UpdateButtonTextToOn();

                // Change the state to ToggledOn
                Become(ToggledOn);
            });
        }


        private void ToggledOn()
        {
            Receive<FlipToggleMessage>(m =>
            {
                // Stop watching a stock
                _stocksCoordinatorActorRef.Tell(new UnWatchStockMessage(_stockSymbol));

                // Change the button text to off
                _stockToggleButtonViewModel.UpdateButtonTextToOff();

                // Change the state to ToggledOff
                Become(ToggledOff);
            });
        }
    }
}
