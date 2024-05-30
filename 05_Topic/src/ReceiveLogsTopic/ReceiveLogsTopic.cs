using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

const string HOST_NAME = "localhost";
const string EXCHANGE_NAME = "topic_logs";

var factory = new ConnectionFactory { HostName = HOST_NAME };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

// creates the exchange
channel.ExchangeDeclare(EXCHANGE_NAME, ExchangeType.Topic);

// creates the Queue - server named (random name)
var queueName = channel.QueueDeclare().QueueName;

if (args.Length < 1)
{
    Console.Error.WriteLine("Usage: {0} [binding_key...]", Environment.GetCommandLineArgs()[0]);
    Console.WriteLine(" Press [enter] to exit.");
    Console.ReadLine();
    Environment.ExitCode = 1;
    return;
}

foreach (var bindingKey in args){
    // binds the queue and the exchange - connects them
    channel.QueueBind(queueName, exchange: EXCHANGE_NAME, routingKey: bindingKey);
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

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();