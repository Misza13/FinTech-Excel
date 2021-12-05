namespace FinTech
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Linq;
    using System.Threading.Tasks;
    using ExcelDna.Integration;

    public static class DeribitFunctions
    {
        internal static readonly DeribitSocket DeribitSocket = new DeribitSocket();

        private static readonly Dictionary<string, object> Observables = new Dictionary<string, object>();

        [ExcelFunction(
            Name = "Deribit.Ticker",
            Description = "Get ticker data from Deribit")]
        public static object GetTicker(
            [ExcelArgument(
                Name = "InstrumentName",
                Description = "Name of instrument on Deribit")]
            string instrumentName,
            
            [ExcelArgument(
                Name = "TickerAttributes",
                Description = "List of properties to return (Range or comma-separated list)")]
            object tickerAttributesIn,
            
            [ExcelArgument(
                Name = "UpdateInterval",
                Description = "Update interval (in seconds) - 0 to disable")]
            int updateInterval,
            
            [ExcelArgument(
                Name = "SpillVertically",
                Description = "Spill resulting values vertically")]
            bool spillVertically)
        {
            var tickerAttributes = tickerAttributesIn switch
            {
                string s => s.Split(','),
                object[,] ss => ss.Cast<string>().ToArray(),
                _ => throw new ArgumentException()
            };
            
            var observableId = $"GetTicker({instrumentName}, {string.Join(",", tickerAttributes)}, {updateInterval}, {spillVertically})";

            return CreatePeriodicObservable(
                observableId,
                updateInterval,
                async () =>
                {
                    var ticker = await DeribitSocket.GetTickerRaw(instrumentName);
                    var result = tickerAttributes
                        .Select(attrName => ticker.ValueByPath("result." + attrName))
                        .ToArray();

                    if (spillVertically)
                    {
                        return result.To2DArray(tickerAttributes.Length, 1);
                    }
                    else
                    {
                        return result.To2DArray(1, tickerAttributes.Length);
                    }
                });
        }

        [ExcelFunction(
            Name = "Deribit.IndexPrice",
            Description = "Retrieve the current index price value for given index name")]
        public static object GetIndexPrice(
            [ExcelArgument(
                Name = "IndexName",
                Description = "Index identifier, one of: btc_usd, eth_usd, btc_usdt, eth_usdt")]
            string indexName,
            
            [ExcelArgument(
                Name = "UpdateInterval",
                Description = "Update interval (in seconds) - 0 to disable")]
            int updateInterval)
        {
            var observableId = $"GetIndexPrice({indexName}, {updateInterval})";

            return CreatePeriodicObservable(
                observableId,
                updateInterval,
                () => DeribitSocket.GetIndexPrice(indexName));
        }

        [ExcelFunction(
            Name = "Deribit.ShortPutLiquidationPrice",
            Description = "Estimate the liquidation price of a short PUT position")]
        public static object ShortPutLiquidationPrice(
            [ExcelArgument(
                Name = "S",
                Description = "Price of underlying instrument when the option was sold")]
            double S,
            
            [ExcelArgument(
                Name = "K",
                Description = "Option strike price")]
            double K,
            
            [ExcelArgument(
                Name = "T",
                Description = "Time to option expiration (in years)")]
            double T,
            
            [ExcelArgument(
                Name = "sigma",
                Description = "Annual volatility of underlying instrument")]
            double sigma,
            
            [ExcelArgument(
                Name = "r",
                Description = "Risk-free interest rate")]
            double r,
            
            [ExcelArgument(
                Name = "sellPrice",
                Description = "Executed price of the option")]
            double sellPrice,
            
            [ExcelArgument(
                Name = "investmentRatio",
                Description = "Portion of margin that was invested")]
            double investmentRatio)
        {
            var origMarkPrice = OptionsFunctions.BlackScholesEuroPut(
                S, K, T, sigma, r) / S;
                
            Func<double, double> mmRatio = currentS =>
            {
                var markPrice = OptionsFunctions.BlackScholesEuroPut(
                    currentS, K, T, sigma, r) / currentS;

                var maintenanceMargin =
                    Math.Max(0.075, 0.075 * markPrice)
                    + markPrice;
                maintenanceMargin *= investmentRatio;

                var pnl = origMarkPrice - markPrice;

                var marginBalance =
                    0.1 + (sellPrice + pnl) * investmentRatio;

                return maintenanceMargin / marginBalance - 1;
            };

            var result = mmRatio.FindRootByPseudoNewtonMethod(
                S * 0.75,
                10);

            if (double.IsNaN(result))
            {
                return ExcelError.ExcelErrorValue;
            }

            return result;
        }

        private static object CreatePeriodicObservable<TRes>(
            string observableId,
            int updateInterval,
            Func<Task<TRes>> fetcher)
        {
            IObservable<TRes> observableSource;

            if (Observables.ContainsKey(observableId))
            {
                observableSource = (IObservable<TRes>) Observables[observableId];
            }
            else if (updateInterval > 0)
            {
                observableSource = Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(updateInterval))
                    .Select(async _ => await fetcher())
                    .Concat();
                Observables[observableId] = observableSource;
            }
            else
            {
                observableSource = Observable.FromAsync(async () => await fetcher());
                Observables[observableId] = observableSource;
            }

            return RxExcel.Observe(
                observableId,
                null,
                () => observableSource);
        }
    }
}