namespace Penrose.Core.Common
{
    public class PagedRequest
    {
        public uint Offset { get; set; }
        public uint Pagesize { get; set; }
        public string OrderBy { get; set; }
        public string SearchBy { get; set; }
        public string Search { get; set; }
    }
}