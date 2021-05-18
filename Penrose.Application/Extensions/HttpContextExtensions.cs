using System;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Penrose.Core.Structs;

namespace Penrose.Application.Extensions
{
    public static class HttpContextExtensions
    {
        public static Guid GetRequestId(this HttpContext httpContext)
        {
            var hasRequestId = httpContext.Items.TryGetValue(HttpRequestHeaderKeys.RequestId, out var requestId);
            if (!hasRequestId)
                throw new InvalidOperationException("Missing RequestId on Request Items");

            return (Guid) requestId;
        }

        public static Guid GetUserId(this IHttpContextAccessor httpContextAccessor)
        {
            return GetUserId(httpContextAccessor.HttpContext);
        }
        
        public static Guid GetUserId(this HttpContext httpContext)
        {
            var identity = httpContext?.User.Identity;
            if (identity == null)
                throw new InvalidCredentialException("Missing user identity!");
            
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.Claims.FirstOrDefault(x => x.Type == PenroseJwtTokenClaimNames.UserId);
            
            if (claim == null)
                throw new InvalidCredentialException("Missing user claim!");

            bool hasUserId = Guid.TryParse(claim.Value, out Guid userId);
            if (!hasUserId)
                throw new InvalidCredentialException("Missing user id!");

            return userId;
        }
    }
}