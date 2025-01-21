namespace DeveloperStore.Sales.Domain.Events.Sales;

public class SaleCancelled : DomainEvent
{
    public int Id { get; set; }

    public SaleCancelled(int id)
    {
        Id = id;
    }
}
