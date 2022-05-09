namespace FinTech
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Linq;
    using System.Threading.Tasks;
    using ExcelDna.Integration;
    using Models;

    public static class DeribitFunctions
    {
        internal static readonly DeribitSocket DeribitSocket = new DeribitSocket();

        private static readonly Dictionary<string, object> Observables = new Dictionary<string, object>();

        private static readonly List<DeribitGetInstrumentsResponse.DeribitInstrumentData> optionCache =
            new List<DeribitGetInstrumentsResponse.DeribitInstrumentData>(); 

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
                        .Select(attrName => ticker.ValueByPath("result." + attrName));

                    return result.ToExcelArray(spillVertically);
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
            Name = "Deribit.ListStrikes",
            Description = "List strike prices around a given ATM price")]
        public static object ListStrikes(
            [ExcelArgument(
                Name = "Expiration",
                Description = "Deribit expiration date")]
            string expiration,
            
            [ExcelArgument(
                Name = "Around ATM",
                Description = "Number of strikes to return around ATM")]
            int aroundAtm,
            
            [ExcelArgument(
                Name = "UpdateInterval",
                Description = "Update interval (in seconds) - 0 to disable")]
            int updateInterval,
            
            [ExcelArgument(
                Name = "SpillVertically",
                Description = "Spill resulting values vertically")]
            bool spillVertically)
        {
            var observableId = $"ListStrikes({expiration}, {aroundAtm}, {updateInterval})";

            return CreatePeriodicObservable(
                observableId,
                updateInterval,
                async () =>
                {
                    if (optionCache.Count == 0)
                    {
                        var instruments = await DeribitSocket.GetInstruments("BTC", "option");
                        
                        lock (optionCache)
                        {
                            optionCache.AddRange(instruments.result);
                        }
                    }

                    var indexPrice = await DeribitSocket.GetIndexPrice("btc_usd");
                    
                    var strikesBelow = optionCache
                        .Where(i =>
                            i.instrument_name.Contains(expiration) &&
                            i.kind == "option" &&
                            i.instrument_name.EndsWith("-C") &&
                            i.strike <= indexPrice)
                        .OrderByDescending(i => i.strike)
                        .Take(aroundAtm)
                        .Reverse()
                        .Select(i => (object)(double)i.strike)
                        .ToArray();

                    if (strikesBelow.Length < aroundAtm)
                    {
                        strikesBelow = Repeat<object>("", aroundAtm - strikesBelow.Length)
                            .Concat(strikesBelow).ToArray();
                    }
                    
                    var strikesAbove = optionCache
                        .Where(i =>
                            i.instrument_name.Contains(expiration) &&
                            i.kind == "option" &&
                            i.instrument_name.EndsWith("-C") &&
                            i.strike > indexPrice)
                        .OrderBy(i => i.strike)
                        .Take(aroundAtm)
                        .Select(i => (object)(double)i.strike)
                        .ToArray();
                    
                    if (strikesAbove.Length < aroundAtm)
                    {
                        strikesAbove = strikesAbove.Concat(Repeat<object>("", aroundAtm - strikesAbove.Length))
                            .ToArray();
                    }
                    
                    return strikesBelow.Concat(strikesAbove).ToExcelArray(spillVertically);
                });
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

        private static IEnumerable<T> Repeat<T>(T value, int count)
        {
            for (var i = 0; i < count; i++)
            {
                yield return value;
            }
        }
    }
}