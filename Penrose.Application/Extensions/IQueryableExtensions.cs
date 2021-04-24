using System;
using System.Linq;
using System.Linq.Expressions;
using Penrose.Core.Common;

namespace Penrose.Application.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> query, PagedRequest pagedRequest)
        {
            return query
                .Take(pagedRequest.Pagesize == 0 ? 20 : (int) pagedRequest.Pagesize)
                .Skip((int)pagedRequest.Offset);
        }
        
        public static IQueryable<T> ApplyOrdering<T>(this IQueryable<T> query, Expression<Func<T, string>> keySelector, string orderBy)
        {
            return orderBy.StartsWith("-")
                ? query.OrderByDescending(keySelector)
                : query.OrderBy(keySelector);
        }
    }
}