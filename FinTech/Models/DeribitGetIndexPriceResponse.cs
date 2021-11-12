namespace FinTech.Models
{
    public class DeribitGetIndexPriceResponse : DeribitRpcResponse<DeribitGetIndexPriceResponse.DeribitGetIndexPriceResult>
    {
        public class DeribitGetIndexPriceResult
        {
            public decimal estimated_delivery_price { get; set; }

            public decimal index_price { get; set; }
        }
    }
}