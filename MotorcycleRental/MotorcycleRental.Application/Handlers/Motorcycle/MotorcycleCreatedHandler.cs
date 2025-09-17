using MediatR;
using MotorcycleRental.Application.Events.Motorcycle;
using MotorcycleRental.Infrastructure.Messaging;

namespace MotorcycleRental.Application.Handlers.Motorcycle
{
    public class MotorcycleCreatedHandler : INotificationHandler<MotorcycleCreatedEvent>
    {
        private readonly IMessageBus _messageBus;

        public MotorcycleCreatedHandler(IMessageBus messageBus)
        {
            _messageBus = messageBus;
        }

        public async Task Handle(MotorcycleCreatedEvent notification, CancellationToken cancellationToken)
        {
            await _messageBus.PublishAsync("motorcycle.created", new
            {
                notification.Id,
                notification.Plate,
                notification.Model,
                notification.Year
            });
        }
    }
}
