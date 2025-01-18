using RabbitMQ.Client;

namespace DeveloperStore.Sales.Domain.RabbitMQ;

public interface IRabbitMQConnection : IDisposable
{
    bool IsConnected { get; }
    bool TryConnect();
    IModel CreateModel();
}