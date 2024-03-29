﻿

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Penrose.Core.Interfaces.Common;

namespace Penrose.Application.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> query, IPagedRequest pagedRequest)
        {
            return query
                .Take(pagedRequest.PageSize == 0 ? 20 : (int) pagedRequest.PageSize)
                .Skip((int)pagedRequest.Offset);
        }
        
        public static IQueryable<T> ApplyOrdering<T>(this IQueryable<T> query, Expression<Func<T, object>> keySelector, string orderBy)
        {
            return orderBy.StartsWith("-")
                ? query.OrderByDescending(keySelector)
                : query.OrderBy(keySelector);
        }

        public static IQueryable<T> ApplyOrdering<T>(
            this IQueryable<T> query,
            string orderBy,
            Dictionary<string, Expression<Func<T, object>>> orderings)
        {
            bool hasOrdering = orderings.TryGetValue(orderBy, out Expression<Func<T, object>> value);
            return !hasOrdering
                ? query
                : query.ApplyOrdering(value, orderBy);
        }
    }
}