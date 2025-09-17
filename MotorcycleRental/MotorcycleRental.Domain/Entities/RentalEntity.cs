using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotorcycleRental.Domain.Entities
{
    [Table("Rental")]
    public class RentalEntity
    {
        public int InternalId { get; set; }
        public string DeliveryManId { get; set; }
        public string MotorcycleId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime ExpectedEndDate { get; set; }
        public DateTime? DevolutionDate { get; set; }
        public double? TotalValue { get; set; }
        public int Plan { get; set; }
    }
}
