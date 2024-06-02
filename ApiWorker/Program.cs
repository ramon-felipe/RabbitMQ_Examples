using ApiWorker;
using Core;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddSingleton<IRabbitMqConnection, RabbitMqConnection>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
