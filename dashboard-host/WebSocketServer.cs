using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace dashboard_host
{
    public class WebSocketServer
    {
        private HttpListener _httpListener;
        private ConcurrentBag<WebSocket> _clients = new ConcurrentBag<WebSocket>();

        public WebSocketServer(string prefix)
        {
            _httpListener = new HttpListener();
            _httpListener.Prefixes.Add(prefix);
        }

        public async Task StartAsync(CancellationToken cancellationToken = default)
        {
            _httpListener.Start();
            Console.WriteLine("Server started");

            while (!cancellationToken.IsCancellationRequested)
            {
                var httpContext = await _httpListener.GetContextAsync();
                if (httpContext.Request.IsWebSocketRequest)
                {
                    var webSocketContext = await httpContext.AcceptWebSocketAsync(null);
                    _clients.Add(webSocketContext.WebSocket);
                    Console.WriteLine("Client connected");
                    _ = Task.Run(() => HandleClientAsync(webSocketContext.WebSocket, cancellationToken));
                }
                else
                {
                    httpContext.Response.StatusCode = 400;
                    httpContext.Response.Close();
                }
            }
        }

        private async Task HandleClientAsync(WebSocket webSocket, CancellationToken cancellationToken)
        {
            var buffer = new byte[1024];
            try
            {
                while (webSocket.State == WebSocketState.Open && !cancellationToken.IsCancellationRequested)
                {
                    var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);
                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", cancellationToken);
                    }
                    else if (result.MessageType == WebSocketMessageType.Text)
                    {
                        var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        Console.WriteLine($"Received: {message}");
                    }
                }
            }
            catch (WebSocketException ex)
            {
                Console.WriteLine($"WebSocket error: {ex.Message}");
            }
            finally
            {
                _clients.TryTake(out webSocket);
                webSocket.Dispose();
                Console.WriteLine("Client disconnected");
            }
        }

        public async Task BroadcastMessageAsync(string message, CancellationToken cancellationToken = default)
        {
            var messageBuffer = Encoding.UTF8.GetBytes(message);

            foreach (var client in _clients)
            {
                if (client.State == WebSocketState.Open)
                {
                    await client.SendAsync(new ArraySegment<byte>(messageBuffer), WebSocketMessageType.Text, true, cancellationToken);
                }
            }
        }
    }
}
