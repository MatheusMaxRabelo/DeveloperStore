using DeveloperStore.Sales.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeveloperStore.Sales.Infra.Data.EntityConfig;

public class ItemConfiguration : IEntityTypeConfiguration<Item>
{
    private const string TABLE_NAME = "Items";
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.ToTable(TABLE_NAME);

        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id)
            .ValueGeneratedOnAdd();

        builder.HasOne(i => i.Sale)
            .WithMany(s => s.Items)
            .HasForeignKey(i => i.SalesId)
            .OnDelete(DeleteBehavior.Cascade);

        
        builder.Property(i => i.ItemId)
            .IsRequired();

        builder.Property(i => i.Quantity)
            .IsRequired()
            .HasDefaultValue(1);

        builder.Property(i => i.TotalAmount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(i => i.UnitPrice)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(i => i.Discount)
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0m);

        builder.Property(i => i.IsCancelled)
            .IsRequired();

        builder.HasIndex(i => new { i.ItemId, i.SalesId })
            .IsUnique()
            .HasDatabaseName("IX_Item_Sales_CompositeUnique");

        builder.Ignore(i => i.DomainEvents);
    }
}
