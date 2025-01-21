using System.Text.Json;
using System.Text.Json.Serialization;
using DeveloperStore.Sales.Application.Interfaces;
using DeveloperStore.Sales.Domain.Events.Sales;
using MediatR;

namespace DeveloperStore.Sales.Application.EventsHandlers.Sale;

public class SaleCreatedNotificationEventHandler(IMessageService messageService) : INotificationHandler<SaleCreated>
{
    public async Task Handle(SaleCreated notification, CancellationToken cancellationToken)
    {
        var subject = JsonSerializer.Serialize(notification.Sale, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.IgnoreCycles });

        await messageService.PublishAsync(subject, notification);
    }
}
