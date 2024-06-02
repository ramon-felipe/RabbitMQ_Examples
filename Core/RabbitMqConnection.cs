using System.Diagnostics;
using RabbitMQ.Client;

namespace Core;

public sealed class RabbitMqConnection : IRabbitMqConnection, IDisposable
{
    private readonly IConnection _connection;
    public IConnection Connection => _connection;

    public RabbitMqConnection()
    {
        Console.WriteLine("Ctor rabbitMqConn");
        this._connection = CreateConnection();
    }

    private static IConnection CreateConnection()
    {
        Console.WriteLine("Creating rabbitMQ connection...");

        const string HOST_NAME = "localhost";

        var factory = new ConnectionFactory { HostName = HOST_NAME };
        
        return  factory.CreateConnection();
    }

    public void Dispose()
    {
        Console.WriteLine("Disposing rabbit connection...");
        this._connection.Dispose();
    }
}