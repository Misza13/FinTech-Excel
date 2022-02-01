namespace FinTech
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using ExcelDna.Integration;
    using RestSharp;

    public static class GreeksLiveFunctions
    {
        [ExcelFunction(
            Name = "GreeksLive.GetIVHistory",
            Description = "Get ATM Implied Volatility history")]
        public static object GetIVHistory(
            [ExcelArgument(
                Name = "currency",
                Description = "Instrument currency")]
            string currency)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var restClient = new RestClient("https://api.greeks.live/api/v1");

            var ivHistoryRequest = new RestRequest("/iv_history", Method.Post);
            ivHistoryRequest.AddBody(new { currency = "BTC" });
            
            var response = restClient.ExecuteAsync<IVHistoryResponse>(ivHistoryRequest).Result;
            var data = response.Data.data;

            var result = new object[data.Count, 4];
            
            for (var i = 0; i < data.Count; i++)
            {
                result[i, 0] = DateTimeOffset.FromUnixTimeMilliseconds(data[i].created_at).DateTime.ToOADate();
                result[i, 1] = data[i].month1;
                result[i, 2] = data[i].month3;
                result[i, 3] = data[i].month6;
            }

            return result;
        }
        
        private class IVHistoryResponse
        {
            public int code { get; set; }
            public List<IVHistoryData> data { get; set; }
            public string msg { get; set; }
        }
        
        private class IVHistoryData
        {
            public long created_at { get; set; }
            public decimal month1 { get; set; }
            public decimal month3 { get; set; }
            public decimal month6 { get; set; }
        }
    }
}