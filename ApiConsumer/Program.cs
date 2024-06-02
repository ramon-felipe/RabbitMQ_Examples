// See https://aka.ms/new-console-template for more information

using Core;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

Console.WriteLine("Hello, World!");

var conn = new RabbitMqConnection();

Console.WriteLine("Consuming...");
const string EXCHANGE_NAME = "exchange_test";

using var channel = conn.Connection.CreateModel();

Console.WriteLine("Channel created...");
// creates the exchange
channel.ExchangeDeclare(EXCHANGE_NAME, ExchangeType.Fanout);

// creates the Queue - server named (random name)
var queueName = channel.QueueDeclare().QueueName;

// binds the queue and the exchange - connects them
channel.QueueBind(queueName, exchange: EXCHANGE_NAME, routingKey: string.Empty);

Console.WriteLine(" [*] Waiting for messages.");

var consumer = new EventingBasicConsumer(channel);

consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($" [x] Received!\t{message}");
};

channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();