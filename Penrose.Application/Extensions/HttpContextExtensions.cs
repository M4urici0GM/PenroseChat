using System;
using Microsoft.AspNetCore.Http;

namespace Penrose.Application.Extensions
{
    public static class HttpContextExtensions
    {
        public static Guid GetRequestId(this HttpContext httpContext)
        {
            var hasRequestId = httpContext.Items.TryGetValue("RequestId", out var requestId);
            if (!hasRequestId)
                throw new InvalidOperationException("Missing RequestId on Request Items");

            return (Guid) requestId;
        }
    }
}