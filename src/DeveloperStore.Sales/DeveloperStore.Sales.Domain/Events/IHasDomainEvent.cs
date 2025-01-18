namespace DeveloperStore.Sales.Domain.Events;

internal interface IHasDomainEvent
{
    public IList<DomainEvent> DomainEvents { get; set; }
}
