using RabbitMQ.Client;

namespace Core;

public interface IRabbitMqConnection {
    IConnection Connection{ get; }
}
