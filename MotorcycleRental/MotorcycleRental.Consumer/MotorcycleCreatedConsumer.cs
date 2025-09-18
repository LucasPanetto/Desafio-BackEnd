using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MotorcycleRental.Consumer.Data;
using MotorcycleRental.Consumer.Data.Entities;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace MotorcycleRental.Consumer
{
    public class MotorcycleCreatedConsumer : BackgroundService
    {
        private readonly ILogger<MotorcycleCreatedConsumer> _logger;
        private readonly RabbitMqOptions _options;
        private readonly IServiceScopeFactory _scopeFactory;
        private IConnection? _connection;
        private IModel? _channel;

        public MotorcycleCreatedConsumer(
            IOptions<RabbitMqOptions> options,
            ILogger<MotorcycleCreatedConsumer> logger,
            IServiceScopeFactory scopeFactory)
        {
            _options = options.Value;
            _logger = logger;
            _scopeFactory = scopeFactory;

            InitializeRabbitMqWithRetry();
        }

        private void InitializeRabbitMqWithRetry()
        {
            var hostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? _options.HostName ?? "localhost";
            var port = int.TryParse(Environment.GetEnvironmentVariable("RABBITMQ_PORT"), out var envPort) ? envPort : _options.Port;
            var userName = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME") ?? _options.UserName ?? "guest";
            var password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD") ?? _options.Password ?? "guest";
            var queueName = Environment.GetEnvironmentVariable("RABBITMQ_QUEUENAME") ?? _options.QueueName ?? "guest";

            var factory = new ConnectionFactory
            {
                HostName = hostName,
                Port = port,
                UserName = userName,
                Password = password,
                DispatchConsumersAsync = true
            };

            int attempts = 0;
            const int maxAttempts = 10;
            const int delayMs = 3000;

            while (attempts < maxAttempts)
            {
                try
                {
                    _connection = factory.CreateConnection();
                    _channel = _connection.CreateModel();

                    _channel.QueueDeclare(
                        queue: queueName,
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    _channel.BasicQos(0, _options.PrefetchCount, false);

                    _logger.LogInformation("Conectado ao RabbitMQ em {Host}:{Port}", hostName, port);
                    return;
                }
                catch (Exception ex)
                {
                    attempts++;
                    _logger.LogWarning("Falha ao conectar RabbitMQ (tentativa {Attempt}/{MaxAttempts}): {Message}", attempts, maxAttempts, ex.Message);
                    Thread.Sleep(delayMs);
                }
            }

            _logger.LogError("Não foi possível conectar ao RabbitMQ após {MaxAttempts} tentativas.", maxAttempts);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var queueName = Environment.GetEnvironmentVariable("RABBITMQ_QUEUENAME") ?? _options.QueueName ?? "motorcycle.created";

            if (_channel == null)
            {
                _logger.LogWarning("RabbitMQ não disponível. Nenhuma mensagem será consumida.");
                return Task.CompletedTask;
            }

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                using var scope = _scopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<NotifyDbContext>();

                try
                {
                    var body = ea.Body.ToArray();
                    var json = Encoding.UTF8.GetString(body);

                    var message = JsonSerializer.Deserialize<JsonElement>(json);

                    var id = message.GetProperty("Id").GetString();
                    var plate = message.GetProperty("Plate").GetString();
                    var modelName = message.GetProperty("Model").GetString();
                    var year = message.GetProperty("Year").GetInt32();

                    if(year == 2024)
                    {
                        _logger.LogInformation("Moto recebida: {Model} - {Plate} ({Id}) - {Year}", modelName, plate, id, year);

                        var notify = new Notify
                        {
                            Id = id,
                            Model = modelName,
                            Plate = plate,
                            Year = year,
                            CreatedAt = DateTime.UtcNow
                        };

                        dbContext.Notify.Add(notify);
                        await dbContext.SaveChangesAsync(stoppingToken);
                    }
                    

                    _channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao processar mensagem");
                    _channel?.BasicNack(ea.DeliveryTag, false, requeue: false);
                }
            };

            _channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
            base.Dispose();
        }
    }
}
