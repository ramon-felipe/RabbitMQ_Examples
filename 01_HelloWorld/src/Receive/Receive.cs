using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

var factory = new ConnectionFactory() { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(queue: "hello",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

Console.WriteLine(" [*] Waiting for messages.");
Console.WriteLine(" Press any key to exit");

var consumer = new EventingBasicConsumer(channel);

consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);

    Console.WriteLine("[x] Message Received! Message: {0}", message);
};

channel.BasicConsume(queue: "hello", 
                        autoAck: true, 
                        consumer: consumer);

Console.ReadKey();