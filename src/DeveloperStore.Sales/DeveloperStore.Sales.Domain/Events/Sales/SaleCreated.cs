using DeveloperStore.Sales.Domain.Entities;

namespace DeveloperStore.Sales.Domain.Events.Sales;

public class SaleCreated : DomainEvent
{
    public Sale Sale { get; set; }

    public SaleCreated(Sale sale)
    {
        Sale = sale;
    }
}
