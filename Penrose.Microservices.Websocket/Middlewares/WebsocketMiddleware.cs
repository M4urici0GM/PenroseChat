using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
        
        public async Task Invoke(HttpContext httpContext, IWebsocketManager websocketManager)
        {
            var webSockets = httpContext.WebSockets;
            var request = httpContext.Request;
            var response = httpContext.Response;

            if (request.Path != "/ws" || !webSockets.IsWebSocketRequest)
            {
                await WriteResponse(httpContext, HttpStatusCode.NotFound, "Not Found!");
                return;
            }

            var websocketConnection = await webSockets.AcceptWebSocketAsync();
            websocketManager.InsertConnection(websocketConnection);
        }

        private async Task WriteResponse(HttpContext httpContext, HttpStatusCode status, string response)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int) status;
            await httpContext.Response.WriteAsync(response);
        }
    }
}