using DeveloperStore.Sales.Application.Interfaces;
using DeveloperStore.Sales.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DeveloperStore.Sales.Application.Services;

public class DomainEventService : IDomainEventService
{

    private readonly ILogger<DomainEventService> _logger;
    private readonly IPublisher _mediator;

    public DomainEventService(ILogger<DomainEventService> logger, IPublisher mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task PublishAsync(IEnumerable<DomainEvent> domainEvents)
    {
        var publishableEvents = domainEvents.Where(i => !i.IsPublished)
                                            .ToList();

        _logger.LogInformation("{count} events will be published", publishableEvents.Count);

        foreach (var domainEvent in publishableEvents)
            await PublishAsync(domainEvent);
    }

    public async Task PublishAsync(DomainEvent domainEvent)
    {
        _logger.LogInformation("Publishing domain event. Event - {event}", domainEvent.GetType().Name);

        await _mediator.Publish(domainEvent);
        domainEvent.IsPublished = true;

        _logger.LogInformation("{event} published", domainEvent.GetType().Name);
    }
}
