using DeveloperStore.Sales.Domain.Events;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeveloperStore.Sales.Domain.Entities;
public class Item
{
    public int Id { get; set; }
    public int ItemId { get; set; }
    public int SalesId { get; set; }
    public Sale Sale { get; set; }
    public int Quantity { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Discount { get; set; } = 0m;
    public bool IsCancelled { get; set; }
    [NotMapped]
    public IList<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
}