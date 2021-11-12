namespace FinTech.Models
{
    public class DeribitGetIndexRequest : DeribitRpcRequest<DeribitGetIndexRequest.DeribitGetIndexParams>
    {
        public DeribitGetIndexRequest(string index_name)
        {
            this.method = "public/get_index_price";
            this.@params = new DeribitGetIndexParams(index_name);
        }
        
        public class DeribitGetIndexParams
        {
            public DeribitGetIndexParams(string index_name)
            {
                this.index_name = index_name;
            }
            
            public string index_name { get; }
        }
    }
}