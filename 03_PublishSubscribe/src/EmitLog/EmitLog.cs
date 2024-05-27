using System.Text;
using RabbitMQ.Client;

const string HOST_NAME = "localhost";
const string EXCHANGE_NAME = "logs";

var factory = new ConnectionFactory() { HostName = HOST_NAME };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

// creates the exchange
channel.ExchangeDeclare(EXCHANGE_NAME, ExchangeType.Fanout);

var message = GetMessage(args);
var body = Encoding.UTF8.GetBytes(message);

channel.BasicPublish(exchange: EXCHANGE_NAME,
                     routingKey: string.Empty,
                     basicProperties: null,
                     body: body);

Console.WriteLine(" [x] Sent {0}", message);

static string GetMessage(string[] args)
{
    return (args.Length > 0) ? string.Join(" ", args) : "Hello World!";
}