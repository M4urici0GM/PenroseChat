using System;
using System.Net;

namespace Penrose.Core.Generics
{
    public class ApiResponse<T> where T : class
    {
        public string Message { get; set; }
        public HttpStatusCode Status { get; set; }
        public Guid RequestId { get; set; }
        public T Data { get; set; }
    }
}