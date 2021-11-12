namespace FinTech.Models
{
    public class DeribitGetTickerResponse: DeribitRpcResponse<DeribitGetTickerResponse.DeribitGetTickerResult>
    {
        public class DeribitGetTickerResult
        {
            public decimal underlying_price { get; set; }

            public decimal mark_price { get; set; }
            
            public decimal mark_iv { get; set; }
        }
    }
}