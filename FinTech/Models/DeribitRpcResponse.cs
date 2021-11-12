namespace FinTech.Models
{
    public abstract class DeribitRpcResponse<TResult> : IHasId
    {
        public string jsonrpc { get; set; }
        
        public long id { get; set; }

        public TResult result { get; set; }

        public long usIn { get; set; }
        
        public long usOut { get; set; }

        public long usDiff { get; set; }
        
        public bool testnet { get; set; }
    }
}