using System.ComponentModel.DataAnnotations.Schema;

namespace MotorcycleRental.Domain.Entities
{
    [Table("Motorcycle")]
    public class MotorcycleEntity
    {
        public int InternalId { get; set; }
        public string Id { get; set; }
        public int Year { get; set; }
        public string Model { get; set; }
        public string Plate { get; set; }
    }
}
