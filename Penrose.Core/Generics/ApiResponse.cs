using System;
using System.Net;

namespace Penrose.Core.Generics
{
    public class ApiResponse<T> where T : class
    {
        public string Message { get; init; }
        public string StackTrace { get; init; }
        public HttpStatusCode Status { get; init; }
        public Guid RequestId { get; init; }
        public T Data { get; init; }
    }
}