using System;

namespace Penrose.Application.Interfaces
{
    public interface ISecurityService
    {
        Guid GetRequestId();
        Guid GetCurrentUserId();
    }
}