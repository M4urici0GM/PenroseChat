namespace Penrose.Core.Interfaces.Common
{
    public interface IPagedRequest
    {
        uint Offset { get; set; }
        uint PageSize { get; set; }
    }
}