using DeveloperStore.Sales.Domain.Entities;

namespace DeveloperStore.Sales.Domain.Interfaces.Repositories;

public interface ISalesRepository
{
    Task<List<Sale>> GetSalesAsync(int pageNumber, int pageSize);
    Task<Sale> GetSaleByIdAsync(int id);
    Task AddAsync(Sale sale);
    Task UpdateAsync(Sale sale);
    Task DeleteAsync(int id);
}
