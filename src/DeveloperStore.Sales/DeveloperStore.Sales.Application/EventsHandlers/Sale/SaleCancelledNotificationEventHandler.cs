using System.Text.Json;
using System.Text.Json.Serialization;
using DeveloperStore.Sales.Application.Interfaces;
using DeveloperStore.Sales.Domain.Events.Sales;
using MediatR;

namespace DeveloperStore.Sales.Application.EventsHandlers.Sale;

public class SaleCancelledNotificationEventHandler(IMessageService messageService) : INotificationHandler<SaleCancelled>
{
    public async Task Handle(SaleCancelled notification, CancellationToken cancellationToken)
    {
        var subject = JsonSerializer.Serialize(notification.Id, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.IgnoreCycles });

        await messageService.PublishAsync(subject, notification);
    }
}
