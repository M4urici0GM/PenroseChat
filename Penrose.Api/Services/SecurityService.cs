using System;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using AutoMapper.Internal;
using Microsoft.AspNetCore.Http;
using Penrose.Application.Interfaces;
using Penrose.Core.Structs;

namespace Penrose.Microservices.User.Services
{
    public class SecurityService : ISecurityService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SecurityService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private HttpContext GetHttpContext()
        {
            HttpContext httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
                throw new InvalidCredentialException();

            return httpContext;
        }

        public Guid GetRequestId()
        {
            HttpContext httpContext = GetHttpContext();            
            var requestId =  httpContext.Items.GetOrDefault(HttpRequestHeaderKeys.RequestId);
            if (requestId is null)
                throw new InvalidOperationException("Missing RequestId on Request Items");

            return (Guid) requestId;
        }

        public Guid GetCurrentUserId()
        {
            HttpContext httpContext = GetHttpContext(); 
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