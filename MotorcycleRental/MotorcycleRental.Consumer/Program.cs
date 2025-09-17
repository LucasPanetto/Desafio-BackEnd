using Microsoft.EntityFrameworkCore;
using MotorcycleRental.Consumer;
using MotorcycleRental.Consumer.Data;
using System;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<RabbitMqOptions>(
    builder.Configuration.GetSection("RabbitMq"));

builder.Services.AddDbContext<NotifyDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHostedService<MotorcycleCreatedConsumer>();

var host = builder.Build();
host.Run();
