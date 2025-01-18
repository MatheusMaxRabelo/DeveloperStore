using DeveloperStore.Sales.Domain.Events;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeveloperStore.Sales.Domain.Entities;
public class Item
{
    public int Id { get; set; }
    public int ItemId { get; set; }
    public int SalesId { get; set; }
    public int Quantity { get; set; }
    public double TotalAmount { get; set; }
    public double Price { get; set; }
    public double DiscountAmount { get; set; } = 0d;
    public string DiscountPercent { get; set; } = string.Empty;
    public bool IsCancelled { get; set; }
    [NotMapped]
    public IList<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
}