using MediatR;

namespace MotorcycleRental.Application.Events.Motorcycle
{
    public class MotorcycleCreatedEvent : INotification
    {
        public int InternalId { get; }
        public string Id { get; }
        public string Plate { get; }
        public string Model { get; }
        public int Year { get; }

        public MotorcycleCreatedEvent(int internalId, string id, string plate, string model, int year)
        {
            InternalId = internalId;
            Id = id;
            Plate = plate;
            Model = model;
            Year = year;
        }
    }
}
