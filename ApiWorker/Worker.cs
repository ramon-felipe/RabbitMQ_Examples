namespace ApiWorker;

using Core;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

public sealed class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IRabbitMqConnection _rabbitMqConnection;


    public Worker(ILogger<Worker> logger, IRabbitMqConnection rabbitMqConnection)
    {
        _logger = logger;
        _rabbitMqConnection = rabbitMqConnection;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        this._logger.LogInformation("Consuming...");
        const string EXCHANGE_NAME = "exchange_test";

        using var channel = this._rabbitMqConnection.Connection.CreateModel();

        this._logger.LogInformation("Channel created...");

        // creates the exchange
        channel.ExchangeDeclare(EXCHANGE_NAME, ExchangeType.Fanout);

        // creates the Queue - server named (random name)
        var queueName = channel.QueueDeclare().QueueName;

        // binds the queue and the exchange - connects them
        channel.QueueBind(queueName, exchange: EXCHANGE_NAME, routingKey: string.Empty);

        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($" [x] Received!\t{message}");
        };

        channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

        while (!stoppingToken.IsCancellationRequested)
        {
            this._logger.LogInformation("Waiting messages...");

            

            await Task.Delay(1000, stoppingToken);
        }

        this._logger.LogInformation("Consumes end.");
    }
}
