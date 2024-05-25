namespace Send;

using RabbitMQ.Client;
using System;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        const string HOST_NAME = "localhost";
        const string QUEUE_NAME = "hello";
        // const string EXCHANGE_NAME = "MyHelloExchange";
        
        var factory = new ConnectionFactory() { HostName = HOST_NAME };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        // creates the Queue
        var x = channel.QueueDeclare(queue: QUEUE_NAME, durable: false, exclusive: false, autoDelete: false, arguments: null);

        for (int i = 0; i < 3; i++)
        {
            string message = $"Hello World {i}!";

            SendMessage(channel, message);
        }

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
    }

    private static void SendMessage(IModel channel, string message)
    {
        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(exchange: string.Empty, // no exchange, direct to the queue
                             routingKey: "hello",
                             basicProperties: null,
                             body: body);

        Console.WriteLine(" [x] Sent {0}", message);
    }
}
