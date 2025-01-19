using DeveloperStore.Sales.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeveloperStore.Sales.Infra.Data.EntityConfig;

public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    private const string TABLE_NAME = "Sales";
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.ToTable(TABLE_NAME);

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id)
            .ValueGeneratedOnAdd();

        builder.Property(s => s.SalesDate)
            .IsRequired();

        builder.Property(s => s.CustomerId)
            .IsRequired();

        builder.Property(s => s.TotalAmount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(s => s.Branch)
            .IsRequired();

        builder.Property(s => s.IsCancelled)
            .IsRequired();

        builder.HasMany(s => s.Items)
            .WithOne(i => i.Sale)
            .HasForeignKey(i => i.SalesId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(s => s.DomainEvents);
    }
}
