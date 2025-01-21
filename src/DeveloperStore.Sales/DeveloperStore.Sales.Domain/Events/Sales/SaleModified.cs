using DeveloperStore.Sales.Domain.Entities;

namespace DeveloperStore.Sales.Domain.Events.Sales;

public class SaleModified : DomainEvent
{
    public Sale Sale { get; set; }

    public SaleModified(Sale sale)
    {
        Sale = sale;
    }
}
