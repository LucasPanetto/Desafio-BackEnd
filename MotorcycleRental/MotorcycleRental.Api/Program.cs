using Microsoft.EntityFrameworkCore;
using MotorcycleRental.Api.Middlewares;
using MotorcycleRental.Application.Handlers.DeliveryMan;
using MotorcycleRental.Infrastructure.Interfaces;
using MotorcycleRental.Infrastructure.Messaging;
using MotorcycleRental.Infrastructure.Persistence;
using MotorcycleRental.Infrastructure.Repositories;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Detecta se está rodando dentro do container
var isDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";

// Configura URLs
builder.WebHost.UseUrls(isDocker ? "http://+:5000" : "http://+:5000;https://+:5001");

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlPath = Path.Combine(AppContext.BaseDirectory,
        $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
    c.IncludeXmlComments(xmlPath);
});

// DbContext
var connectionString = Environment.GetEnvironmentVariable("DEFAULT_CONNECTION")
                      ?? builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<MotorcycleRentalDbContext>(options =>
    options.UseNpgsql(connectionString)
);

// Repositórios
builder.Services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();
builder.Services.AddScoped<IDeliveryManRepository, DeliveryManRepository>();
builder.Services.AddScoped<IRentalRepository, RentalRepository>();
builder.Services.AddScoped<IPlanRepository, PlanRepository>();

// MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
    typeof(CreateRentalHandler).Assembly
));

// RabbitMQ
var rabbitConfig = builder.Configuration.GetSection("RabbitMq").Get<RabbitMqOptions>() ?? new RabbitMqOptions();
rabbitConfig.HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? rabbitConfig.HostName;
rabbitConfig.UserName = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME") ?? rabbitConfig.UserName;
rabbitConfig.Password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD") ?? rabbitConfig.Password;
rabbitConfig.QueueName = Environment.GetEnvironmentVariable("RABBITMQ_QUEUENAME") ?? rabbitConfig.QueueName;
var prefetchEnv = Environment.GetEnvironmentVariable("RABBITMQ_PREFETCH");
if (ushort.TryParse(prefetchEnv, out var prefetch))
{
    rabbitConfig.PrefetchCount = prefetch;
}

// Registra RabbitMQOptions e MessageBus
builder.Services.AddSingleton(rabbitConfig);
builder.Services.AddScoped<IMessageBus, RabbitMqMessageBus>();

var app = builder.Build();

// Aplica migrations
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MotorcycleRentalDbContext>();
    db.Database.Migrate();
}

// Middleware e Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!isDocker)
{
    app.UseHttpsRedirection();
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthorization();
app.MapControllers();

app.Run();
