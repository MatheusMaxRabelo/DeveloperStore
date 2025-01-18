using MediatR;

namespace DeveloperStore.Sales.Domain.Events;

public abstract class DomainEvent : INotification
{
    public bool IsPublished { get; set; }
    public DateTimeOffset DateOccured { get; set; }

    protected DomainEvent()
    {
        DateOccured = DateTimeOffset.UtcNow;
    }
}
