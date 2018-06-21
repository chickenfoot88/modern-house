using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Core.DataAccess.Interfaces;
using Core.DataAccess.Params;

namespace Core.DataAccess.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Paging<T>(this IQueryable<T> query, StoreLoadParams loadParams)
        {
            if (loadParams == null)
            {
                return query;
            }
            if (loadParams.Length == 0)
            {
                return query;
            }
            var resultQuery = query;
            if (loadParams.Start != 0)
            {
                resultQuery = resultQuery.Skip(loadParams.Start);
            }

            return resultQuery.Take(loadParams.Length);
        }

        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> expression)
        {
            if (condition)
            {
                return query.Where(expression);
            }
            return query;
        }

        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> query, bool condition, Func<T, bool> predicate)
        {
            if (condition)
            {
                return query.Where(predicate);
            }
            return query;
        }

        public static IQueryable<T> WhereIfElse<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> trueExpression, Expression<Func<T, bool>> falseExpression)
        {
            if (condition)
            {
                return query.Where(trueExpression);
            }
            else
            {
                return query.Where(falseExpression);
            }
        }
    }
}
