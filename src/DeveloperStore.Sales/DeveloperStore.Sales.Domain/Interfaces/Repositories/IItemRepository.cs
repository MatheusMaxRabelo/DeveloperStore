using DeveloperStore.Sales.Domain.Entities;

namespace DeveloperStore.Sales.Domain.Interfaces.Repositories;

public interface IItemRepository
{
    Task<List<Item>> GetItemsBySaleIdAsync(int saleId);
    Task AddAsync(Item item);
    Task UpdateAsync(Item item);
    Task DeleteAsync(int id);
}
