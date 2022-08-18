using RabbitMQ.Client;
using System;
using System.Text;

namespace Send
{
    class Program
    {
        static void Main(string[] args)
        {
            const string QUEUE_NAME = "hello";
            const string EXCHANGE_NAME = "MyHelloExchange";

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            // creates the Queue
            channel.QueueDeclare(queue: QUEUE_NAME, durable: false, exclusive: false, autoDelete: false, arguments: null);

            // creates the Exchange
            channel.ExchangeDeclare(EXCHANGE_NAME, ExchangeType.Direct, durable: false, autoDelete: false, null);

            // Binds the Queue and the Exchange
            channel.QueueBind(QUEUE_NAME, EXCHANGE_NAME, "hello");

            for (int i = 0; i < 10; i++)
            {
                string message = $"Hello World {i}!";

                SendMessage(channel, message);
            }


            Console.WriteLine("Press [enter] to exit");
            Console.ReadKey();
        }

        private static void SendMessage(IModel channel, string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "MyHelloExchange",
                                 routingKey: "hello",
                                 basicProperties: null,
                                 body: body);

            Console.WriteLine(" [x] Sent {0}", message);
        }
    }
}
