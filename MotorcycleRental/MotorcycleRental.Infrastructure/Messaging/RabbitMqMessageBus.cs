using Microsoft.Extensions.Options;
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

        public RabbitMqMessageBus(IOptions<RabbitMqOptions> options)
        {
            _options = options.Value;
        }


        public Task PublishAsync(string queueName, object message)
        {
            try
            {
                EnsureConnection();

                _channel!.QueueDeclare(queue: queueName,
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
                _channel.BasicPublish(exchange: "",
                                     routingKey: queueName,
                                     basicProperties: null,
                                     body: body);
            }
            catch (BrokerUnreachableException ex)
            {
                // RabbitMQ não está acessível, apenas loga e segue
                Console.WriteLine($"[WARN] RabbitMQ indisponível: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERRO] Falha ao publicar mensagem: {ex.Message}");
            }

            return Task.CompletedTask;
        }

        private void EnsureConnection()
        {
            if (_connection != null && _connection.IsOpen && _channel != null && _channel.IsOpen)
                return;

            var factory = new ConnectionFactory()
            {
                HostName = _options.HostName,
                UserName = _options.UserName,
                Password = _options.Password
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}
