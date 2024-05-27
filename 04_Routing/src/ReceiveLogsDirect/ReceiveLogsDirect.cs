using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

const string HOST_NAME = "localhost";
const string EXCHANGE_NAME = "direct_logs";

var factory = new ConnectionFactory { HostName = HOST_NAME };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

// creates the exchange
channel.ExchangeDeclare(EXCHANGE_NAME, ExchangeType.Direct);

// creates the Queue - server named (random name)
var queueName = channel.QueueDeclare().QueueName;
channel.QueueDeclare(queue: "LogErrors", durable: false, exclusive: false, autoDelete: false, arguments: null);

if (args.Length < 1)
{
    Console.Error.WriteLine("Usage: {0} [info] [warning] [error]", Environment.GetCommandLineArgs()[0]);
    Console.WriteLine(" Press [enter] to exit.");
    Console.ReadLine();
    Environment.ExitCode = 1;
    return;
}

channel.QueueBind("LogErrors", exchange: EXCHANGE_NAME, routingKey: "error");

foreach (var severity in args){
    // binds the queue and the exchange - connects them
    channel.QueueBind(queueName, exchange: EXCHANGE_NAME, routingKey: severity);
}

Console.WriteLine(" [*] Waiting for messages.");

var consumer = new EventingBasicConsumer(channel);

consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    var routingKey = ea.RoutingKey;
    Console.WriteLine($" [x] Received '{routingKey}':'{message}'");
};

channel.BasicConsume(queue: queueName,
                     autoAck: true,
                     consumer: consumer);

channel.BasicConsume(queue: "LogErrors",
                     autoAck: true,
                     consumer: consumer);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();