using MotorcycleRental.Infrastructure.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System.Text;
using System.Text.Json;

namespace MotorcycleRental.Infrastructure.Messaging
{
    public class RabbitMqMessageBus : IMessageBus, IDisposable
    {
        private IConnection? _connection;
        private IModel? _channel;
        private readonly RabbitMqOptions _options;

        public RabbitMqMessageBus(RabbitMqOptions options)
        {
            _options = options;

            Console.WriteLine($"RabbitMQ HostName: {_options.HostName}"); // Deve mostrar rabbitmq
            Console.WriteLine($"RabbitMQ QueueName: {_options.QueueName}");
            Console.WriteLine($"RabbitMQ UserName: {_options.UserName}");
            Console.WriteLine($"RabbitMQ Password: {_options.Password}");
        }

        private void EnsureConnection()
        {
            if (_connection != null && _connection.IsOpen && _channel != null && _channel.IsOpen)
                return;

            var factory = new ConnectionFactory()
            {
                HostName = _options.HostName,
                UserName = _options.UserName,
                Password = _options.Password,
                Port = _options.Port
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // Declara a fila
            _channel.QueueDeclare(
                queue: _options.QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            // Configura prefetch
            _channel.BasicQos(0, _options.PrefetchCount, false);
        }

        public Task PublishAsync(string queueName, object message)
        {
            try
            {
                EnsureConnection();

                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
                _channel!.BasicPublish(exchange: "",
                                      routingKey: queueName,
                                      basicProperties: null,
                                      body: body);
            }
            catch (BrokerUnreachableException ex)
            {
                Console.WriteLine($"[WARN] RabbitMQ indisponível: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERRO] Falha ao publicar mensagem: {ex.Message}");
            }

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}
