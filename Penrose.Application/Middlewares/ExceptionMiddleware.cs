using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
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
        private readonly IHostEnvironment _hostEnvironment;

        public ExceptionMiddleware(RequestDelegate requestDelegate, IHostEnvironment hostEnvironment)
        {
            _requestDelegate = requestDelegate;
            _hostEnvironment = hostEnvironment;
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
                await Response(
                    httpContext,
                    HttpStatusCode.BadRequest,
                    ex,
                    ex.ValidationErrors.Select(x => new
                    {
                        name = x.PropertyName,
                        message = x.ErrorMessage,
                    }));
            }
            catch (Exception ex)
            {
                await Response(httpContext, HttpStatusCode.InternalServerError, ex);
            }
        }

        private Task Response(HttpContext context, HttpStatusCode statusCode, Exception exception,
            object response = null)
        {
            string message = exception.Message;
            Guid requestId = context.GetRequestId();

            if (!_hostEnvironment.IsDevelopment())
                message = "Internal Server Error";
            
            ApiResponse<object> apiResponse = new ApiResponse<object>
            {
                Message = message,
                Status = statusCode,
                RequestId = requestId,
                Data = response
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) statusCode;
            return context.Response.WriteAsync(apiResponse.ToJson());
        }
    }
}