namespace FinTech.Models
{
    public class DeribitGetInstrumentsRequest : DeribitRpcRequest<DeribitGetInstrumentsRequest.DeribitGetInstrumentsParams>
    {
        public DeribitGetInstrumentsRequest(string currency, string kind)
        {
            this.method = "public/get_instruments";
            this.@params = new DeribitGetInstrumentsParams
            {
                currency = currency,
                kind = kind,
                expired = false
            };
        }
        
        public class DeribitGetInstrumentsParams
        {
            public string currency { get; set; }
            public string kind { get; set; }
            public bool expired { get; set; }
        }
    }
}