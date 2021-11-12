namespace FinTech
{
    using System;
    using System.Linq;
    using System.Reactive.Linq;
    using ExcelDna.Integration;
    using Newtonsoft.Json.Linq;

    public static class DeribitFunctions
    {
        internal static readonly DeribitSocket DeribitSocket = new DeribitSocket();

        [ExcelFunction(
            Name = "DERIBIT.TICKER",
            Description = "Get ticker data from Deribit")]
        public static object GetTicker(
            [ExcelArgument(
                Name = "INSTRUMENT_NAME",
                Description = "Name of instrument on Deribit")]
            string instrumentName,
            
            [ExcelArgument(
                Name = "TICKER_ATTRIBUTES",
                Description = "Comma-separated list of properties to return")]
            string tickerAttributes,
            
            int updateInterval, //TODO: handle non-positive values
            
            bool spillVertically) //TODO: handle
        {
            var observableId = $"{instrumentName}({tickerAttributes})@{updateInterval}/{spillVertically}";
            
            return RxExcel.Observe(
                observableId,
                null,
                () => Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(updateInterval))
                    .Select(async _ =>
                    {
                        var ticker = await DeribitSocket.GetTickerRaw(instrumentName);
                        var result = ticker["result"];
                        var rresult = tickerAttributes.Split(',')
                            .Select(attrName => result.SelectToken(attrName))
                            .Cast<JValue>()
                            .Select(v => v.Value)
                            .ToArray();
                        return rresult;
                    })
                    .Concat()
                );
        }
    }
}