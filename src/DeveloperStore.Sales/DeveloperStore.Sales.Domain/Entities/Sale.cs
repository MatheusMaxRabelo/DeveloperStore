using DeveloperStore.Sales.Domain.Events;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeveloperStore.Sales.Domain.Entities;

public class Sale
{
    public int Id { get; set; }
    public DateTime SalesDate { get; set; }
    public int CustomerId { get; set; }
    public decimal TotalAmount { get; set; }
    public string Branch { get; set; } = null!;
    public List<Item> Items { get; set; } = null!;
    public bool IsCancelled { get; set; }
    [NotMapped]
    public IList<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
}

