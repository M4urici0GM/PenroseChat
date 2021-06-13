using Penrose.Core.Interfaces.Common;

namespace Penrose.Application.Common
{
    public class PagedRequest : IPagedRequest
    {
        public uint Offset { get; set; }
        public uint PageSize { get; set; }
        public string OrderBy { get; set; }
        public string SearchBy { get; set; }
        public string Search { get; set; }
    }
}