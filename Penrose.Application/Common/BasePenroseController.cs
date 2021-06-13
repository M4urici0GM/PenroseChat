using System.Net;
using Microsoft.AspNetCore.Mvc;
using Penrose.Application.Interfaces;
using Penrose.Core.Generics;

namespace Penrose.Application.Common
{
    public class BasePenroseController : ControllerBase
    {
        protected readonly ISecurityService SecurityService;
        
        public BasePenroseController(ISecurityService securityService)
        {
            SecurityService = securityService;
        }

        public OkObjectResult Ok<T>(T value) where T : class
        {
            return Ok(null, value);
        }

        public CreatedResult Created<T>(T value) where T : class
        {   
            return Created(null, value);
        }

        public OkObjectResult Ok<T>(string message, T value) where T : class
        {
            return base.Ok(new ApiResponse<T>
            {
                Message = message,
                Data = value,
                Status = HttpStatusCode.OK,
                RequestId = SecurityService.GetRequestId(),
            });
        }
        
        public CreatedResult Created<T>(string message, T value) where T : class
        {
            return base.Created("", new ApiResponse<T>
            {
                Message = message,
                Data = value,
                Status = HttpStatusCode.Created,
                RequestId = SecurityService.GetRequestId(),
            });
        }
        
        public ObjectResult Forbidden<T>(string message, T value) where T : class
        {
            return new(new ApiResponse<T>
            {
                Message = message,
                Data = value,
                Status = HttpStatusCode.Forbidden,
                RequestId = SecurityService.GetRequestId(),
            })
            {
                StatusCode = (int) HttpStatusCode.Forbidden,
            };
        }
    }
}