namespace Core;

using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

public sealed class RabbitMqBus : IMessageBus
{
    private readonly IRabbitMqConnection _rabbitMqConnection;

    public RabbitMqBus(IRabbitMqConnection rabbitMqConnection)
    {
        _rabbitMqConnection = rabbitMqConnection;
    }

    public void Publish<T> (T message)
    {
        const string EXCHANGE_NAME = "exchange_test";

        using var channel = this._rabbitMqConnection.Connection.CreateModel();

        // creates the exchange
        channel.ExchangeDeclare(EXCHANGE_NAME, ExchangeType.Fanout);

        var deserializedMessage = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(deserializedMessage);

        channel.BasicPublish(exchange: EXCHANGE_NAME,
                            routingKey: string.Empty,
                            basicProperties: null,
                            body: body);
    }
}