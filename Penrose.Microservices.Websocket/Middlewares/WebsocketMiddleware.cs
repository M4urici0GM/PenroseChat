using System;
using System.Net;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Penrose.Microservices.Websocket.Producers.Messages;
using Penrose.Microservices.Websocket.Services;

namespace Penrose.Microservices.Websocket.Middlewares
{
    public class WebsocketMiddleware
    {
        private readonly RequestDelegate _requestDelegate;
        
        public WebsocketMiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }
        
        public async Task Invoke(
            HttpContext httpContext,
            IMediator mediator,
            IWebsocketManager websocketManager,
            CancellationToken cancellationToken)
        {
            var webSockets = httpContext.WebSockets;
            var request = httpContext.Request;
            if (request.Path != "/ws" || !webSockets.IsWebSocketRequest)
            {
                await WriteResponse(httpContext, HttpStatusCode.NotFound, "Not Found!");
                return;
            }

            var websocketConnection = await webSockets.AcceptWebSocketAsync();
            WebsocketConnection websocketIdentifier = websocketManager.InsertConnection(websocketConnection);
            while (true)
            {
                byte[] buffer = new byte[1024 * 4];
                WebSocketReceiveResult received = await websocketConnection.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);
                if (received.Count == 0)
                    continue;

                if (!received.EndOfMessage)
                    throw new InvalidOperationException("Streaming not supported yet!");

                await mediator.Send(new MessageReceivedProduceRequest()
                {
                    MessageBuffer = buffer,
                    WebsocketIdentifier = websocketIdentifier,
                }, cancellationToken);
            }
        }

        private async Task WriteResponse(HttpContext httpContext, HttpStatusCode status, string response)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int) status;
            await httpContext.Response.WriteAsync(response);
        }
    }
}