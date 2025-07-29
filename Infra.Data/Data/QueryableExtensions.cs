using System.Linq.Expressions;
using System.Reflection;
using System.Collections.Concurrent;

namespace Infra.Data.Data
{
    public static class QueryableExtensions
    {
        // Cache para melhorar performance de ordenação dinâmica
        private static readonly ConcurrentDictionary<string, LambdaExpression> _orderExpressions = new();

        /// <summary>
        /// Strongly-typed ordering method - PREFERRED
        /// </summary>
        public static IOrderedQueryable<T> OrderBy<T, TKey>(
            this IQueryable<T> query,
            Expression<Func<T, TKey>> keySelector,
            bool descending = false)
        {
            return descending 
                ? query.OrderByDescending(keySelector)
                : query.OrderBy(keySelector);
        }

        /// <summary>
        /// Dynamic ordering with caching and improved safety
        /// </summary>
        public static IQueryable<T> OrderByDynamic<T>(
            this IQueryable<T> query,
            string propertyName,
            bool descending = false)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                return query;

            var cacheKey = $"{typeof(T).FullName}.{propertyName}";
            
            // Try to get from cache first
            if (!_orderExpressions.TryGetValue(cacheKey, out var cachedExpression))
            {
                // Create and cache the expression
                cachedExpression = CreateOrderExpression<T>(propertyName);
                if (cachedExpression != null)
                {
                    _orderExpressions.TryAdd(cacheKey, cachedExpression);
                }
            }

            if (cachedExpression == null)
                return query; // Property not found, return original query

            return ApplyOrderExpression(query, cachedExpression, descending);
        }

        /// <summary>
        /// Common ordering patterns for entities
        /// </summary>
        public static IQueryable<T> OrderByCommonPatterns<T>(
            this IQueryable<T> query,
            string orderBy,
            bool descending = false)
        {
            // Handle common property names across entities
            var normalizedProperty = orderBy?.ToLower() switch
            {
                "id" => "Id",
                "nome" => "Nome",
                "ativo" => "Ativo",
                "datacriacao" or "criado" => "CreatedAt",
                "datamodificacao" or "modificado" => "UpdatedAt",
                "descricao" => "Descricao",
                _ => orderBy
            };

            return OrderByDynamic(query, normalizedProperty, descending);
        }

        /// <summary>
        /// Multiple field ordering
        /// </summary>
        public static IQueryable<T> OrderByFields<T>(
            this IQueryable<T> query,
            params (string PropertyName, bool Descending)[] orderFields)
        {
            if (orderFields == null || orderFields.Length == 0)
                return query;

            IOrderedQueryable<T>? orderedQuery = null;

            foreach (var (propertyName, descending) in orderFields)
            {
                if (orderedQuery == null)
                {
                    // First ordering
                    var tempQuery = OrderByDynamic(query, propertyName, descending);
                    orderedQuery = tempQuery as IOrderedQueryable<T>;
                }
                else
                {
                    // Subsequent orderings (ThenBy)
                    orderedQuery = ApplyThenBy(orderedQuery, propertyName, descending);
                }
            }

            return orderedQuery ?? query;
        }

        #region Private Implementation

        private static LambdaExpression? CreateOrderExpression<T>(string propertyName)
        {
            try
            {
                var type = typeof(T);
                var property = type.GetProperty(propertyName, 
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (property == null)
                    return null;

                var parameter = Expression.Parameter(type, "x");
                var propertyAccess = Expression.Property(parameter, property);
                
                return Expression.Lambda(propertyAccess, parameter);
            }
            catch
            {
                return null; // Safely handle any reflection errors
            }
        }

        private static IQueryable<T> ApplyOrderExpression<T>(
            IQueryable<T> query, 
            LambdaExpression orderExpression, 
            bool descending)
        {
            try
            {
                var methodName = descending ? "OrderByDescending" : "OrderBy";
                var resultExp = Expression.Call(
                    typeof(Queryable),
                    methodName,
                    new[] { typeof(T), orderExpression.ReturnType },
                    query.Expression,
                    Expression.Quote(orderExpression));

                return query.Provider.CreateQuery<T>(resultExp);
            }
            catch
            {
                return query; // Return original query if ordering fails
            }
        }

        private static IOrderedQueryable<T> ApplyThenBy<T>(
            IOrderedQueryable<T> query,
            string propertyName,
            bool descending)
        {
            var cacheKey = $"{typeof(T).FullName}.{propertyName}";
            
            if (!_orderExpressions.TryGetValue(cacheKey, out var expression))
                return query;

            try
            {
                var methodName = descending ? "ThenByDescending" : "ThenBy";
                var resultExp = Expression.Call(
                    typeof(Queryable),
                    methodName,
                    new[] { typeof(T), expression.ReturnType },
                    query.Expression,
                    Expression.Quote(expression));

                return (IOrderedQueryable<T>)query.Provider.CreateQuery<T>(resultExp);
            }
            catch
            {
                return query;
            }
        }

        #endregion
    }
}