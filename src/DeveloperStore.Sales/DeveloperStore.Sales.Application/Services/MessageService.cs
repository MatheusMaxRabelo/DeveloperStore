using System.Text;
using DeveloperStore.Sales.Application.Interfaces;
using DeveloperStore.Sales.Domain.Events;
using DeveloperStore.Sales.Domain.RabbitMQ;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace DeveloperStore.Sales.Application.Services;

public class MessageService : IMessageService
{
    private readonly ILogger<MessageService> _logger;
    private readonly IRabbitMQConnection _bus;

    private const string EXCHANGE_NAME = "sales-topic-exchange";
    private const string ROUTING_KEY_PREFIX = "sales.";

    public MessageService(ILogger<MessageService> logger, IRabbitMQConnection bus)
    {
        _logger = logger;
        _bus = bus;
    }

    public async Task PublishAsync(string messageSubject, DomainEvent domainEvent)
    {
        _logger.LogInformation("Publishing...");
        _logger.LogInformation($"Publishing to the queue the event: {domainEvent.GetType().Name}");

        using (var channel = _bus.CreateModel())
        {
            channel.ExchangeDeclare(exchange: EXCHANGE_NAME, type: ExchangeType.Topic);

            var body = Encoding.UTF8.GetBytes(messageSubject);
            var routingKey = domainEvent.GetType().Name;

            channel.BasicPublish(EXCHANGE_NAME, $"{ROUTING_KEY_PREFIX}{routingKey}", null, body);
        }

        await Task.CompletedTask;
    }
}
