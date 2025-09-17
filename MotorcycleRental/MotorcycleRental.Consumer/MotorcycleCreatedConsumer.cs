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

            try
            {
                InitializeRabbitMq();
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Não foi possível conectar ao RabbitMQ: {Message}. O consumidor continuará em modo offline.", ex.Message);
                _connection = null;
                _channel = null;
            }
        }

        private void InitializeRabbitMq()
        {
            var factory = new ConnectionFactory
            {
                HostName = _options.HostName,
                UserName = _options.UserName,
                Port = _options.Port,
                Password = _options.Password,
                DispatchConsumersAsync = true
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: _options.QueueName,
                                  durable: true,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);

            _channel.BasicQos(0, _options.PrefetchCount, false);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (_channel == null)
            {
                _logger.LogWarning("RabbitMQ não disponível. Nenhuma mensagem será consumida.");
                return Task.CompletedTask;
            }

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                using var scope = _scopeFactory.CreateScope(); // cria scope para cada mensagem
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

                    _logger.LogInformation("Moto recebida: {Model} - {Plate} ({Id}) - {Year}",
                        modelName, plate, id, year);

                    // Inserção otimizada no banco
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

                    _channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao processar mensagem");
                    _channel?.BasicNack(ea.DeliveryTag, false, requeue: false);
                }
            };

            _channel.BasicConsume(queue: _options.QueueName, autoAck: false, consumer: consumer);

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
