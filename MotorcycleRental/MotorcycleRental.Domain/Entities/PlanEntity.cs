using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRental.Domain.Entities
{
    [Table("Plan")]
    public class PlanEntity
    {
        public int InternalId { get; set; }
        public int Days { get; set; }
        public double Price { get; set; }
    }
}
