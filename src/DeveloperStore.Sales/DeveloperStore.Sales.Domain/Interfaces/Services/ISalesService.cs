using DeveloperStore.Sales.Domain.Entities;

namespace DeveloperStore.Sales.Domain.Interfaces.Services;

public interface ISalesService
{
    Task<List<Sale>> GetSalesAsync(int pageNumber, int pageSize);
    Task<Sale> GetSaleByIdAsync(int id);
    Task<Sale> CreateSaleAsync(Sale sale);
    Task UpdateSaleAsync(int id, Sale sale);
    Task DeleteSaleAsync(int id);
}
