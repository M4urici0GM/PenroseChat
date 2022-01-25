using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Penrose.Application.Extensions;
using Penrose.Core.Generics;

namespace Penrose.Microservices.User.Extensions
{
  public static class HttpContextExtensions
  {
    public static Task WriteCustomResponse(
        this HttpContext context,
        HttpStatusCode statusCode,
        Guid requestId,
        Exception exception,
        IHostEnvironment hostEnvironment,
        object response = null)
    {
      string stackTrace = !hostEnvironment.IsDevelopment()
        ? null
        : exception?.StackTrace;

      string message = (statusCode == HttpStatusCode.InternalServerError && !hostEnvironment.IsDevelopment())
          ? "Internal Server Error."
          : exception?.Message;

      ApiResponse<object> apiResponse = new ApiResponse<object>
      {
        Message = message,
        Status = statusCode,
        StackTrace = stackTrace, 
        RequestId = requestId,
        Data = response
      };

      string responseContent = apiResponse.ToJson();

      context.Response.ContentType = "application/json";
      context.Response.StatusCode = (int)statusCode;
      return context.Response.WriteAsync(responseContent);
    }
  }
}