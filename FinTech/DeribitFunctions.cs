namespace FinTech
{
    using System;
    using System.Reactive.Linq;
    using ExcelDna.Integration;

    public static class DeribitFunctions
    {
        internal static readonly DeribitSocket DeribitSocket = new DeribitSocket();

        [ExcelFunction(
            Name = "DERIBIT.TICKER",
            Description = "Get ticker data from Deribit")]
        public static object GetTicker(
            [ExcelArgument(Name = "INSTRUMENT_NAME")]
            string instrumentName,
            string tickerAttributes,
            int updateInterval,
            bool spillVertically)
        {
            var observableId = $"{instrumentName}({tickerAttributes})@{updateInterval}/{spillVertically}";
            
            return RxExcel.Observe(
                observableId,
                null,
                () => Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(updateInterval))
                    .Select(async _ =>
                    {
                        var ticker = await DeribitSocket.GetTicker(instrumentName);
                        return new object[]
                        {
                            ticker.result.underlying_price,
                            ticker.result.mark_price,
                            ticker.result.mark_iv
                        };
                    })
                    .Concat()
                );
        }
    }
}