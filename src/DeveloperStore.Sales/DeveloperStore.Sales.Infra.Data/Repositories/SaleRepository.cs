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

    public async Task<List<Sale>> GetSalesAsync(int pageNumber, int pageSize)
    {
        var query = _context.Sales.AsQueryable();

        var count = await query.CountAsync();

        var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

        return items;
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
    }

    public async Task DeleteAsync(int id)
    {
        var sale = await _context.Sales.FindAsync(id);
        if (sale != null)
        {
            _context.Sales.Remove(sale);
        }
    }
}
