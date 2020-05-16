using Akka.Actor;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using ReactiveStock.ActorModel.Messages;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ReactiveStock.ActorModel.Actors.UI
{
    class LineChartingActor : ReceiveActor
    {
        private readonly PlotModel _chartModel;
        private readonly Dictionary<string, LineSeries> _series;

        public LineChartingActor(PlotModel chartModel)
        {
            _chartModel = chartModel;
            _series = new Dictionary<string, LineSeries>();

            Receive<AddChartSeriesMessage>(m => AddSeriesToChart(m));
            Receive<RemoveChartSeriesMessage>(m => RemoveSeriesFromChart(m));
            Receive<StockPriceMessage>(m => HandleNewStockPrice(m));
        }

        private void AddSeriesToChart(AddChartSeriesMessage message)
        {
            // Sanity check, check if the series does not have the stock symbol
            if (!_series.ContainsKey(message.StockSymbol))
            {
                // because the stock symbol isn't in the chart, lets create the series
                var newLineSeries = new LineSeries
                {
                    StrokeThickness = 2,
                    MarkerSize = 3,
                    MarkerStroke = OxyColors.Black,
                    CanTrackerInterpolatePoints = false,
                    Title = message.StockSymbol
                };

                _series.Add(message.StockSymbol, newLineSeries);
                _chartModel.Series.Add(newLineSeries);

                RefreshChart();
            }
        }

        private void RemoveSeriesFromChart(RemoveChartSeriesMessage message)
        {
            // Sanity check, check if the series does have the stock symbol
            if (_series.ContainsKey(message.StockSymbol))
            {
                // because the stock symbol is in the chart, lets create remove it
                var seriesToRemove = _series[message.StockSymbol];

                _chartModel.Series.Remove(seriesToRemove);
                _series.Remove(message.StockSymbol);

                RefreshChart();
            }
        }
        private void HandleNewStockPrice(StockPriceMessage message)
        {
            if (_series.ContainsKey(message.StockSymbol))
            {
                var series = _series[message.StockSymbol];

                var newDataPoint = new DataPoint(DateTimeAxis.ToDouble(message.Date),
                    LinearAxis.ToDouble(message.StockPrice));

                // Keep the last 10 DataPoints
                if(series.Points.Count > 10)
                {
                    series.Points.RemoveAt(0);
                }

                series.Points.Add(newDataPoint);

                RefreshChart();
            }
        }

        private void RefreshChart()
        {
            _chartModel.InvalidatePlot(true);
        }

    }
}
