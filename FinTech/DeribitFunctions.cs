namespace FinTech
{
    using System;
    using System.Linq;
    using System.Reactive.Linq;
    using System.Threading.Tasks;
    using ExcelDna.Integration;

    public static class DeribitFunctions
    {
        internal static readonly DeribitSocket DeribitSocket = new DeribitSocket();

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

        private static object CreatePeriodicObservable<TRes>(
            string observableId,
            int updateInterval,
            Func<Task<TRes>> fetcher)
        {
            IObservable<TRes> observableSource;

            if (updateInterval > 0)
            {
                observableSource = Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(updateInterval))
                    .Select(async _ => await fetcher())
                    .Concat();
            }
            else
            {
                observableSource = Observable.FromAsync(async () => await fetcher());
            }

            return RxExcel.Observe(
                observableId,
                null,
                () => observableSource);
        }
    }
}