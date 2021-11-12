namespace FinTech
{
    using System;
    using System.Linq;
    using System.Reactive.Linq;
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
            
            int updateInterval, //TODO: handle non-positive values
            
            bool spillVertically) //TODO: handle
        {
            var tickerAttributes = tickerAttributesIn switch
            {
                string s => s.Split(','),
                object[,] ss => ss.Cast<string>().ToArray(),
                _ => throw new ArgumentException()
            };
            
            var observableId = $"{instrumentName}({string.Join(",", tickerAttributes)})@{updateInterval}/{spillVertically}";
            
            return RxExcel.Observe(
                observableId,
                null,
                () => Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(updateInterval))
                    .Select(async _ =>
                    {
                        var ticker = await DeribitSocket.GetTickerRaw(instrumentName);
                        var result = tickerAttributes
                            .Select(attrName => ticker.ValueByPath("result." + attrName))
                            .ToArray();
                        return result;
                    })
                    .Concat()
                );
        }
    }
}