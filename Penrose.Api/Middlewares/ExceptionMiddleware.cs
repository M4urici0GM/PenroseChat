using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Penrose.Application.Extensions;
using Penrose.Core.Exceptions;
using Penrose.Core.Generics;
using Penrose.Core.Structs;
using Penrose.Microservices.User.Extensions;

namespace Penrose.Microservices.User.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _requestDelegate;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly Guid _requestId;

        public ExceptionMiddleware(RequestDelegate requestDelegate, IHostEnvironment hostEnvironment)
        {
            _requestDelegate = requestDelegate;
            _hostEnvironment = hostEnvironment;
            _requestId = Guid.NewGuid();
        }

        public async Task Invoke(HttpContext httpContext, IHostEnvironment hostEnvironment)
        {
            try
            {
                httpContext.Items.Add(HttpRequestHeaderKeys.RequestId, _requestId);
                await _requestDelegate(httpContext);
            }
            catch (ConcurrencyException ex)
            {
                await httpContext.WriteCustomResponse(HttpStatusCode.Conflict, _requestId, ex, hostEnvironment);
            }
            catch (EntityNotFoundException ex)
            {
                await httpContext.WriteCustomResponse(HttpStatusCode.NotFound, _requestId, ex, hostEnvironment);
            }
            catch (EntityValidationException ex)
            {
                object validationErrors = ex.ValidationErrors.Select(x => new
                {
                    name = x.PropertyName,
                    message = x.ErrorMessage,
                });

                await httpContext.WriteCustomResponse(
                    HttpStatusCode.BadRequest,
                    _requestId,
                    ex,
                    hostEnvironment,
                    validationErrors);
            }
            catch (Exception ex)
            {
                await httpContext.WriteCustomResponse(
                    HttpStatusCode.InternalServerError,
                    _requestId,
                    ex,
                    hostEnvironment);
            }
        }

    }
}