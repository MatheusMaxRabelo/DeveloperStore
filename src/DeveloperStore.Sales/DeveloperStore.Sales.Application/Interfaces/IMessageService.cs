using DeveloperStore.Sales.Domain.Events;

namespace DeveloperStore.Sales.Application.Interfaces;

public interface IMessageService
{
    Task PublishAsync(string messageSubject, DomainEvent domainEvent);
}
