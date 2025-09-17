using System.ComponentModel.DataAnnotations.Schema;

namespace MotorcycleRental.Domain.Entities
{
    [Table("Notify")]
    public class NotifyEntity
    {
        public int InternalId { get; set; }
        public string Id { get; set; }
        public string Model { get; set; }
        public string Plate { get; set; }
        public int Year { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
