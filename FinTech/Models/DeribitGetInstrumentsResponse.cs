namespace FinTech.Models
{
    public class DeribitGetInstrumentsResponse : DeribitRpcResponse<DeribitGetInstrumentsResponse.DeribitInstrumentData[]>
    {
        public class DeribitInstrumentData
        {
            public string base_currency { get; set; }
            public decimal block_trade_commission { get; set; }
            public decimal contract_size { get; set; }
            public string counter_currency { get; set; }
            public long creation_timestamp { get; set; }
            public long expiration_timestamp { get; set; }
            public string future_type { get; set; }
            public int instrument_id { get; set; }
            public string instrument_name { get; set; }
            public bool is_active { get; set; }
            public string kind { get; set; }
            public decimal leverage { get; set; }
            public decimal maker_commission { get; set; }
            public decimal min_trade_amount { get; set; }
            public string option_type { get; set; }
            public string price_index { get; set; }
            public string quote_currency { get; set; }
            public string settlement_currency { get; set; }
            public string settlement_period { get; set; }
            public decimal strike { get; set; }
            public decimal taker_commission { get; set; }
            public decimal tick_size { get; set; }
        }
    }
}