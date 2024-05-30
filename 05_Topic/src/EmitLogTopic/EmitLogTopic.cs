﻿using System.Text;
using RabbitMQ.Client;

const string HOST_NAME = "localhost";
const string EXCHANGE_NAME = "topic_logs";

var factory = new ConnectionFactory() { HostName = HOST_NAME };

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

// creates the exchange
channel.ExchangeDeclare(EXCHANGE_NAME, ExchangeType.Topic);

var routingKey = (args.Length > 0) ? args[0] : "anonymous.info";
var message = args.Length > 1
                ? string.Join(" ", args.Skip(1).ToArray())
                : "Hello World!";
var body = Encoding.UTF8.GetBytes(message);

channel.BasicPublish(exchange: EXCHANGE_NAME,
                     routingKey: routingKey,
                     basicProperties: null,
                     body: body);

Console.WriteLine($" [x] Sent '{routingKey}':'{message}'");

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();