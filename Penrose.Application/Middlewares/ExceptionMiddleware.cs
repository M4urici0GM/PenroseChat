using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Penrose.Application.Extensions;
using Penrose.Core.Exceptions;
using Penrose.Core.Generics;
using Penrose.Core.Structs;

namespace Penrose.Application.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _requestDelegate;

        public ExceptionMiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                httpContext.Items.Add(HttpRequestHeaderKeys.RequestId, Guid.NewGuid());
                await _requestDelegate(httpContext);
            }
            catch (ConcurrencyException ex)
            {
                await Response(httpContext, HttpStatusCode.Conflict, ex);
            }
            catch (EntityValidationException ex)
            {
                await Response(httpContext, HttpStatusCode.BadRequest, ex);
            }
            catch (Exception ex)
            {
                await Response(httpContext, HttpStatusCode.InternalServerError, ex);
            }
        }

        private static Task Response(HttpContext context, HttpStatusCode statusCode, Exception exception,
            object response = null)
        {
            var requestId = context.GetRequestId();
            var responseContent = JsonConvert.SerializeObject(new ApiResponse<object>
            {
                Message = exception.Message,
                Status = statusCode,
                RequestId = requestId,
                Data = response
            });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) statusCode;
            return context.Response.WriteAsync(responseContent);
        }
    }
}