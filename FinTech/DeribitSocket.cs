namespace FinTech
{
    using System.Collections.Concurrent;
    using System.Security.Authentication;
    using System.Threading.Tasks;
    using Models;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using WebSocketSharp;
    
    public class DeribitSocket
    {
        private WebSocket _socket;

        private readonly object _requestIdLock = new object();
        private int _nextRequestId = 1;

        private readonly ConcurrentDictionary<long, IResponseHandler> _handlers =
            new ConcurrentDictionary<long, IResponseHandler>();

        public void Start()
        {
            _socket = new WebSocket("wss://www.deribit.com/ws/api/v2");
            _socket.SslConfiguration.EnabledSslProtocols = SslProtocols.Tls12;
            _socket.OnMessage += OnMessageReceived;
            _socket.Connect();
        }

        private void OnMessageReceived(object sender, MessageEventArgs eventArgs)
        {
            var json = (JObject) JsonConvert.DeserializeObject(eventArgs.Data);
            var requestId = json["id"].Value<int>();

            if (_handlers.ContainsKey(requestId))
            {
                _handlers[requestId].SetResult(eventArgs.Data);
                _handlers.TryRemove(requestId, out _);
            }
        }

        public async Task<DeribitGetTickerResponse> GetTicker(string instrumentName)
        {
            var request = new DeribitGetTickerRequest(instrumentName);
            var handler = new ResponseHandlerBase<DeribitGetTickerResponse>();
            await Send(request, handler);
            return await handler.Task;
        }

        private async Task Send(IHasId request, IResponseHandler handler)
        {
            lock (_requestIdLock)
            {
                request.id = _nextRequestId;
                _nextRequestId += 1;
            }

            _handlers[request.id] = handler;
            
            await Send(JsonConvert.SerializeObject(request));
        }

        private async Task Send(string message)
        {
            if (!_socket.IsAlive) return;
            await Task.Run(() => _socket.Send(message));
        }
        
        private interface IResponseHandler
        {
            void SetResult(string payload);
        }
        
        private class ResponseHandlerBase<TRes> : IResponseHandler
        {
            private readonly TaskCompletionSource<TRes> _tcs =
                new TaskCompletionSource<TRes>();
            
            public Task<TRes> Task => _tcs.Task;
            
            public void SetResult(string payload)
            {
                var result = JsonConvert.DeserializeObject(payload, typeof(TRes));
                _tcs.SetResult((TRes) result);
            }
        }
    }
}