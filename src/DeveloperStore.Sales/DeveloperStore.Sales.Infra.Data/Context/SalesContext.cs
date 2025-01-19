using DeveloperStore.Sales.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeveloperStore.Sales.Infra.Data.Context;

public class SalesContext : DbContext
{
    #region Ctor
    public SalesContext() { }

    public SalesContext(DbContextOptions<SalesContext> options) : base(options) { }
    #endregion

    #region Properties
    public DbSet<Domain.Entities.Sales> Sales { get; set; }
    public DbSet<Item> Items { get; set; }
    #endregion
}
