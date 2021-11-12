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
            Name = "Deribit.GetTicker",
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
            
            bool spillVertically) //TODO: handle
        {
            var tickerAttributes = tickerAttributesIn switch
            {
                string s => s.Split(','),
                object[,] ss => ss.Cast<string>().ToArray(),
                _ => throw new ArgumentException()
            };
            
            async Task<object[]> FetchTicker(long _)
            {
                var ticker = await DeribitSocket.GetTickerRaw(instrumentName);
                var result = tickerAttributes
                    .Select(attrName => ticker.ValueByPath("result." + attrName))
                    .ToArray();
                return result;
            }

            var observableId = $"{instrumentName}({string.Join(",", tickerAttributes)})@{updateInterval}/{spillVertically}";

            IObservable<object[]> observableSource;

            if (updateInterval > 0)
            {
                observableSource = Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(updateInterval))
                    .Select(FetchTicker)
                    .Concat();
            }
            else
            {
                observableSource = Observable.FromAsync(async () => await FetchTicker(0));
            }

            return RxExcel.Observe(
                observableId,
                null,
                () => observableSource);
        }
    }
}