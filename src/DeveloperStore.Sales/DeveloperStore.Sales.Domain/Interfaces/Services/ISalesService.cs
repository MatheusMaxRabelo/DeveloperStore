using DeveloperStore.Sales.Domain.Entities;
using DeveloperStore.Sales.Domain.Models;

namespace DeveloperStore.Sales.Domain.Interfaces.Services;

public interface ISalesService
{
    Task<List<SalesModel>> GetSalesAsync(Dictionary<string, string> filter);
    Task<SalesModel> GetSaleByIdAsync(int id);
    Task<SalesModel> CreateSaleAsync(Sale sale);
    Task UpdateSaleAsync(int id, Sale sale);
    Task DeleteSaleAsync(int id);
}
