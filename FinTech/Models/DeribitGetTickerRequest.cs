namespace FinTech.Models
{
    public class DeribitGetTickerRequest : DeribitRpcRequest<DeribitGetTickerRequest.DeribitGetTickerParams>
    {
        public DeribitGetTickerRequest(string instrument_name)
        {
            this.method = "public/ticker";
            this.@params = new DeribitGetTickerParams(instrument_name);
        }
        
        public class DeribitGetTickerParams
        {
            public DeribitGetTickerParams(string instrument_name)
            {
                this.instrument_name = instrument_name;
            }
            
            public string instrument_name { get; }
        }
    }
}