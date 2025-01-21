using DeveloperStore.Sales.Domain.Entities;

namespace DeveloperStore.Sales.Domain.Interfaces.Repositories;

public interface ISalesRepository
{
    Task<(List<Sale> Items, int TotalCount)> GetSalesAsync(int pageNumber, int pageSize,
        Dictionary<string, string> filters = null,
        string orderBy = null);
    Task<Sale> GetSaleByIdAsync(int id);
    Task AddAsync(Sale sale);
    Task UpdateAsync(Sale sale);
    Task DeleteAsync(int id);
}
