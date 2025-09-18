using Microsoft.EntityFrameworkCore;
using MotorcycleRental.Consumer;
using MotorcycleRental.Consumer.Data;
using System;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration
       .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
       .AddEnvironmentVariables();

builder.Services.Configure<RabbitMqOptions>(options =>
{
    options.HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST")
                       ?? builder.Configuration["RabbitMq:HostName"]
                       ?? "localhost";

    options.Port = int.TryParse(Environment.GetEnvironmentVariable("RABBITMQ_PORT"), out var envPort)
                   ? envPort
                   : (builder.Configuration.GetValue<int>("RabbitMq:Port") > 0
                      ? builder.Configuration.GetValue<int>("RabbitMq:Port")
                      : 5672);

    options.UserName = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME")
                       ?? builder.Configuration["RabbitMq:UserName"]
                       ?? "guest";

    options.Password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD")
                       ?? builder.Configuration["RabbitMq:Password"]
                       ?? "guest";

    options.QueueName = builder.Configuration["RabbitMq:QueueName"] ?? "motorcycle.created";
    options.PrefetchCount = builder.Configuration.GetValue<ushort>("RabbitMq:PrefetchCount", 1);
});

builder.Services.AddDbContext<NotifyDbContext>(options =>
    options.UseNpgsql(
        Environment.GetEnvironmentVariable("DEFAULT_CONNECTION")
        ?? builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

builder.Services.AddHostedService<MotorcycleCreatedConsumer>();

var host = builder.Build();
host.Run();
