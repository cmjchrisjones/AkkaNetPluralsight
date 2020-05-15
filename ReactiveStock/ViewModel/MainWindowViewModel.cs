using Akka.Actor;
using GalaSoft.MvvmLight;
using OxyPlot;
using OxyPlot.Axes;
using ReactiveStock.ActorModel;
using ReactiveStock.ActorModel.Actors;
using ReactiveStock.ActorModel.Actors.UI;
using System;
using System.Collections.Generic;

namespace ReactiveStock.ViewModel
{
   public class MainWindowViewModel : ViewModelBase
    {
        private IActorRef _chartingActorRef;
        private IActorRef _stocksCoordinatorActorRef;
        private PlotModel _plotModel;

        public Dictionary<string, StockToggleButtonViewModel> StockButtonViewModels { get; set; }

        public PlotModel PlotModel
        {
            get
            {
                return _plotModel;
            }
            set
            {
                Set(() => PlotModel, ref _plotModel, value);
            }
        }

        public MainWindowViewModel()
        {
            SetupChartModel();
            InitializeActors();
            CreateStockButtonViewModels();
        }

        private void SetupChartModel()
        {
            _plotModel = new PlotModel
            {
                LegendTitle = "Legend",
                LegendOrientation = LegendOrientation.Horizontal,
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.TopRight,
                LegendBackground = OxyColor.FromAColor(200, OxyColors.White),
                LegendBorder = OxyColors.Black
            };

            var stockDateTimeAxis = new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                Title = "Date",
                StringFormat = "HH:mm:ss"
            };

            _plotModel.Axes.Add(stockDateTimeAxis);

            var stockPriceAxis = new LinearAxis
            {
                Minimum = 0,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                Title = "Price"
            };

            _plotModel.Axes.Add(stockPriceAxis);
        }

        private void InitializeActors()
        {
            // Create a new line charting actor and add it to the actor system
            _chartingActorRef = ActorSystemReferance.ActorSystem.ActorOf(Props.Create(() => new LineChartingActor(PlotModel)));

            // Create the stocks coordinating actor in the actor system
            _stocksCoordinatorActorRef = ActorSystemReferance.ActorSystem.ActorOf(Props.Create(() => new StocksCoordinatorActor(_chartingActorRef)), "StocksCoordinator");
        }

        private void CreateStockButtonViewModels()
        {
            StockButtonViewModels = new Dictionary<string, StockToggleButtonViewModel>();

            CreateStockButtonViewModel("AAAA");
            CreateStockButtonViewModel("BBBB");
            CreateStockButtonViewModel("CCCC");
            CreateStockButtonViewModel("DDDD");
        }

        private void CreateStockButtonViewModel(string stockSymbol)
        {
            var newViewModel = new StockToggleButtonViewModel(_stocksCoordinatorActorRef, stockSymbol);
            StockButtonViewModels.Add(stockSymbol, newViewModel);
        }
    }
}
