using System;
using System.Net;

namespace Penrose.Core.Generics
{
    public class ApiResponse
    {
        public string Message { get; set; }
        public HttpStatusCode Status { get; set; }
        public Guid RequestId { get; set; }
        public object Data { get; set; }
    }
}