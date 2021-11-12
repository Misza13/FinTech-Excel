namespace FinTech.Models
{
    public abstract class DeribitRpcRequest<TParams> : IHasId
    {
        public string jsonrpc => "2.0";

        public long id { get; set; }

        public string method { get; protected set; }

        public TParams @params { get; protected set; }
    }
}