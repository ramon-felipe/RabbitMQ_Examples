using Api;
using Core;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IRabbitMqConnection, RabbitMqConnection>();
builder.Services.AddTransient<IMessageBus, RabbitMqBus>();
builder.Services.AddHostedService<WorkerTest>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/publish", ([FromServices] IMessageBus bus) =>
{
    var msg = new TestMessage("testing...");
    bus.Publish(msg);

    return "tested";
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();
