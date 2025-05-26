using System.Linq.Expressions;
using System.Reflection;

namespace Infra.Data.Data
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> OrderByDynamic<T>(
            this IQueryable<T> query,
            string propertyName,
            bool descending)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                return query;

            // Verifica se a propriedade existe no tipo T
            var property = typeof(T).GetProperty(propertyName,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (property == null)
                return query; // Retorna query sem ordenação se a propriedade não existir

            var parameter = Expression.Parameter(typeof(T), "x");
            var propertyAccess = Expression.Property(parameter, property);
            var lambda = Expression.Lambda(propertyAccess, parameter);

            var methodName = descending ? "OrderByDescending" : "OrderBy";
            var resultExp = Expression.Call(
                typeof(Queryable),
                methodName,
                new[] { typeof(T), property.PropertyType },
                query.Expression,
                Expression.Quote(lambda));

            return query.Provider.CreateQuery<T>(resultExp);
        }
    }
}