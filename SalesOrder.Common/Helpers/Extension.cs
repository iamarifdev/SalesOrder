using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace SalesOrder.Common.Helpers;

public static class Extension
{
    public static bool IsEmpty(this int? value)
    {
        return value == null;
    }

    public static bool IsEmpty<T>(this T value)
    {
        return value == null;
    }

    public static bool IsNotEmpty(this string value)
    {
        return !string.IsNullOrEmpty(value);
    }

    public static bool IsNotEmpty(this int? value)
    {
        return value != null;
    }

    public static bool IsNotEmpty(this Enum value)
    {
        return value != null;
    }

    public static bool IsNotEmpty(this DateTime? value)
    {
        return value != null;
    }

    public static bool IsNotEmpty<T>(this T value)
    {
        return !IsEmpty(value);
    }

    public static IOrderedQueryable<T> OrderByField<T>(
        this IQueryable<T> source,
        string sortField,
        string order = "asc")
    {
        var lambda = GetOrderByLambda<T>(sortField);
        return order == "asc"
            ? source.OrderBy(lambda)
            : source.OrderByDescending(lambda);
    }

    private static Expression<Func<T, object>> GetOrderByLambda<T>(string sortBy)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var member = Expression.PropertyOrField(parameter, sortBy);
        var body = Expression.Convert(member, typeof(object));
        return Expression.Lambda<Func<T, object>>(body, parameter);
    }

    public static async Task PartialUpdate<TEntity>(this DbContext context, TEntity entity,
        params Expression<Func<TEntity, object>>[] propertiesToUpdate)
        where TEntity : class
    {
        context.Set<TEntity>().Attach(entity);

        foreach (var property in propertiesToUpdate)
        {
            var propertyName = ((MemberExpression)property.Body).Member.Name;
            context.Entry(entity).Property(propertyName).IsModified = true;
        }

        await context.SaveChangesAsync();
    }
}