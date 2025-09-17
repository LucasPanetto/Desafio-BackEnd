using System.ComponentModel.DataAnnotations.Schema;

namespace MotorcycleRental.Domain.Entities
{
    [Table("DeliveryMan")]
    public class DeliveryManEntity
    {
        public int InternalId { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public string Cnpj { get; set; }
        public string CnhNumber { get; set; }
        public string CnhType { get; set; }
        public string CnhImagePath { get; set; }

    }
}
