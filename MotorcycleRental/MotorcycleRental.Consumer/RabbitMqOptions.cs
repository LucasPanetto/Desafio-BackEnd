namespace MotorcycleRental.Consumer
{
    public class RabbitMqOptions
    {
        public string HostName { get; set; } = "rabbitmq";
        public string UserName { get; set; } = "guest";
        public string Password { get; set; } = "guest";
        public string QueueName { get; set; } = "motorcycle.created";
        public int Port { get; set; } = 5672;
        public ushort PrefetchCount { get; set; } = 1;
    }
}
