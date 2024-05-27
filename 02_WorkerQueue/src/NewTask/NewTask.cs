using System.Text;
using RabbitMQ.Client;

const string HOST_NAME = "localhost";

var factory = new ConnectionFactory() { HostName = HOST_NAME };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

// creates the Queue
channel.QueueDeclare(queue: "task_queue",
                     durable: true,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

var message = GetMessage(args);
var body = Encoding.UTF8.GetBytes(message);
var properties = channel.CreateBasicProperties();
properties.Persistent = true;

channel.BasicPublish(exchange: string.Empty, // no exchange, direct to the queue
                     routingKey: "task_queue",
                     basicProperties: properties,
                     body: body);

Console.WriteLine(" [x] Sent {0}", message);

static string GetMessage(string[] args)
{
    return (args.Length > 0) ? string.Join(" ", args) : "Hello World!";
}