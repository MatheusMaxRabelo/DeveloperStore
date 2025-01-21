using DeveloperStore.Sales.Domain.Events;

namespace DeveloperStore.Sales.Application.Interfaces;

public interface IDomainEventService
{
    Task PublishAsync(IEnumerable<DomainEvent> domainEvents);
    Task PublishAsync(DomainEvent domainEvent);
}
