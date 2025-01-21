using DeveloperStore.Sales.Domain.Entities;
using DeveloperStore.Sales.Domain.Interfaces.Repositories;
using DeveloperStore.Sales.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace DeveloperStore.Sales.Infra.Data.Repositories;

public class ItemRepository : IItemRepository
{
    private readonly SalesContext _context;

    public ItemRepository(SalesContext context)
    {
        _context = context;
    }

    public async Task<List<Item>> GetItemsBySaleIdAsync(int saleId)
    {
        return await _context.Items
            .Where(i => i.SalesId == saleId)
            .ToListAsync();
    }

    public async Task AddAsync(Item item)
    {
        await _context.Items.AddAsync(item);
    }

    public async Task UpdateAsync(Item item)
    {
        _context.Items.Update(item);
    }

    public async Task DeleteAsync(int id)
    {
        var item = await _context.Items.FindAsync(id);
        if (item != null)
        {
            _context.Items.Remove(item);
        }
    }
}
