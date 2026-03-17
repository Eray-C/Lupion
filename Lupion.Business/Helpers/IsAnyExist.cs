using Lupion.Business.Interfaces;
using Lupion.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Lupion.Business.Helpers;

public class IsAnyExist(DBContext context)
{

    public async Task<bool> BeUnique<T, TRequest>(TRequest request, string value, Expression<Func<T, string>> propertySelector, CancellationToken cancellationToken)
        where T : class
        where TRequest : IHasId
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var property = (propertySelector.Body as MemberExpression)?.Member.Name;

        if (string.IsNullOrEmpty(property))
            throw new InvalidOperationException("Property name cannot be determined.");

        var propertyExpression = Expression.Property(parameter, property);
        var valueExpression = Expression.Constant(value);
        var equality = Expression.Equal(propertyExpression, valueExpression);

        var idProperty = Expression.Property(parameter, "Id");

        Expression predicate;

        if (request.Id == null || request.Id == default(int))
        {
            predicate = equality;
        }
        else
        {
            var idConstant = Expression.Constant(request.Id, idProperty.Type);
            var idNotEqual = Expression.NotEqual(idProperty, idConstant);
            predicate = Expression.AndAlso(equality, idNotEqual);
        }

        var lambda = Expression.Lambda<Func<T, bool>>(predicate, parameter);

        return !await IsAnyValueExistsAsync(lambda);
    }
    public async Task<bool> IsAnyValueExistsAsync<T>(Expression<Func<T, bool>> predicate) where T : class
    {
        return await context.Set<T>().AnyAsync(predicate);
    }
}
