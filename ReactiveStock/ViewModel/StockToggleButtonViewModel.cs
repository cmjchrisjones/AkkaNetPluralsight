using Akka.Actor;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ReactiveStock.ActorModel;
using ReactiveStock.ActorModel.Actors;
using ReactiveStock.ActorModel.Messages;
using System;
using System.Windows.Input;

namespace ReactiveStock.ViewModel
{
    public class StockToggleButtonViewModel : ViewModelBase
    {
        // Bound to the UI buttons content property
        private string _buttonText;

        // Stock Symbol
        public string StockSymbol { get; set; }

        // Command to execute when button is clicked
        public ICommand ToggleCommand { get; set; }

        // IActorRef to point at the StockToggleButtonActor thats associated with this view model and the stock symbol
        public IActorRef StockToggleButtonActorRef { get; private set; }

        // Wrap button text field 
        public string ButtonText
        {
            get
            {
                return _buttonText;
            }
            set
            {
                Set(() => ButtonText, ref _buttonText, value);
            }
        }

        public StockToggleButtonViewModel(IActorRef stocksCoordinatorActorRef, string stockSymbol)
        {
            StockSymbol = stockSymbol;

            // Create the StockToggleButtonActor thats associated with this view model which will be the bridge between the StockToggleButtonViewModel and our actor model
            StockToggleButtonActorRef = ActorSystemReferance.ActorSystem.ActorOf(
                Props.Create(()=> 
                new StockToggleButtonActor(stocksCoordinatorActorRef, this, stockSymbol)));

            // When user clicks the button, this command executes and tells the StockToggleButtonActor to flip the toggle (causes it to be watched or unwatched)
            ToggleCommand = new RelayCommand(() => StockToggleButtonActorRef.Tell(new FlipToggleMessage()));

            // When we create an instance of StockToggleButtonViewModel, initial text should be set to off
            UpdateButtonTextToOff();
        }

        public void UpdateButtonTextToOff()
        {
            ButtonText = ConstructButtonText(false);
        }


        public void UpdateButtonTextToOn()
        {
            ButtonText = ConstructButtonText(true);
        }

        private string ConstructButtonText(Boolean isToggledOn)
        {
            return $"{StockSymbol} {(isToggledOn ? "(on)" : "(off)")}";
        }
    }
}
