using System.Linq.Expressions;
using DeveloperStore.Sales.Domain.Entities;
using DeveloperStore.Sales.Domain.Interfaces.Repositories;
using DeveloperStore.Sales.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace DeveloperStore.Sales.Infra.Data.Repositories;

public class SaleRepository : ISalesRepository
{
    private readonly SalesContext _context;

    public SaleRepository(SalesContext context)
    {
        _context = context;
    }

    public async Task<(List<Sale> Items, int TotalCount)> GetSalesAsync(int pageNumber,
        int pageSize,
        Dictionary<string, string> filters = null,
        string orderBy = null)
    {
        var query = _context.Sales.AsNoTracking().Include(x => x.Items).AsQueryable();

        if (filters != null)
        {
            foreach (var filter in filters)
            {
                var property = filter.Key;
                var value = filter.Value;

                if (property.StartsWith("_min") || property.StartsWith("_max"))
                {
                    var actualProperty = property.Substring(4);
                    query = ApplyRangeFilter(query, actualProperty, value, property.StartsWith("_min"));
                }
                else
                {
                    query = ApplyEqualityFilter(query, property, value);
                }
            }
        }
        if (!string.IsNullOrEmpty(orderBy))
        {
            query = ApplyOrdering(query, orderBy);
        }

        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<Sale> GetSaleByIdAsync(int id)
    {
        return await _context.Sales
            .Include(s => s.Items)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task AddAsync(Sale sale)
    {
        await _context.Sales.AddAsync(sale);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Sale sale)
    {
        _context.Sales.Update(sale);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var sale = await _context.Sales.FindAsync(id);
        if (sale != null)
        {
            _context.Sales.Remove(sale);
        }
    }

    private IQueryable<T> ApplyRangeFilter<T>(IQueryable<T> query, string property, string value, bool isMin)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var propertyExpression = Expression.Property(parameter, property);
        var valueExpression = Expression.Constant(Convert.ChangeType(value, propertyExpression.Type));

        var comparison = isMin
            ? Expression.GreaterThanOrEqual(propertyExpression, valueExpression)
            : Expression.LessThanOrEqual(propertyExpression, valueExpression);

        var lambda = Expression.Lambda<Func<T, bool>>(comparison, parameter);
        return query.Where(lambda);
    }

    private IQueryable<T> ApplyEqualityFilter<T>(IQueryable<T> query, string property, string value)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var propertyExpression = Expression.Property(parameter, property);
        var valueExpression = Expression.Constant(Convert.ChangeType(value, propertyExpression.Type));

        var equality = Expression.Equal(propertyExpression, valueExpression);
        var lambda = Expression.Lambda<Func<T, bool>>(equality, parameter);
        return query.Where(lambda);
    }

    private IQueryable<T> ApplyOrdering<T>(IQueryable<T> query, string orderBy)
    {
        var orderClauses = orderBy.Split(',');
        foreach (var clause in orderClauses)
        {
            var parts = clause.Trim().Split(' ');
            var property = parts[0];
            var descending = parts.Length > 1 && parts[1].Equals("desc", StringComparison.OrdinalIgnoreCase);

            query = ApplySingleOrdering(query, property, descending);
        }

        return query;
    }

    private IQueryable<T> ApplySingleOrdering<T>(IQueryable<T> query, string property, bool descending)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var propertyExpression = Expression.Property(parameter, property);

        var lambda = Expression.Lambda(propertyExpression, parameter);
        var methodName = descending ? "OrderByDescending" : "OrderBy";

        var resultExpression = Expression.Call(
            typeof(Queryable),
            methodName,
            new Type[] { query.ElementType, propertyExpression.Type },
            query.Expression,
            Expression.Quote(lambda)
        );

        return query.Provider.CreateQuery<T>(resultExpression);
    }
}
