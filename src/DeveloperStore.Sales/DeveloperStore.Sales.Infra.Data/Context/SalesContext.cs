using DeveloperStore.Sales.Domain.Entities;
using DeveloperStore.Sales.Infra.Data.EntityConfig;
using Microsoft.EntityFrameworkCore;

namespace DeveloperStore.Sales.Infra.Data.Context;

public class SalesContext : DbContext
{
    #region Ctor
    public SalesContext() { }

    public SalesContext(DbContextOptions<SalesContext> options) : base(options) { }
    #endregion

    #region Properties
    public DbSet<Sale> Sales { get; set; }
    public DbSet<Item> Items { get; set; }
    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new SaleConfiguration().Configure(modelBuilder.Entity<Sale>());
        new ItemConfiguration().Configure(modelBuilder.Entity<Item>());

        base.OnModelCreating(modelBuilder);
    }
}
