namespace MotorcycleRental.Consumer.Data.Entities
{
    public class Notify
    {
        public string Id { get; set; }
        public string Model { get; set; }
        public string Plate { get; set; }
        public int Year { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
