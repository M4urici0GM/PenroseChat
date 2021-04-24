using System.Collections.Generic;

namespace Penrose.Core.Common
{
    public class PagedResult<TEntity> where TEntity : class
    {
        public uint Offset { get; set; }
        public uint Pagesize { get; set; }
        public int Count { get; set; }
        public IEnumerable<TEntity> Records { get; set; }
    }
}