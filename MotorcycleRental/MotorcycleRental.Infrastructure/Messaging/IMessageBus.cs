namespace MotorcycleRental.Infrastructure.Messaging
{
    public interface IMessageBus
    {
        Task PublishAsync(string queueName, object message);
    }
}
