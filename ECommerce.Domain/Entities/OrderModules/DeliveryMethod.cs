using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities.OrderModules
{
    public class DeliveryMethod : BaseEntity<int>
    {
        public string ShortName { get; set; }
        public string Description { get; set; }
        public string DeliveryTime { get; set; }
        public decimal Price { get; set; }
    }
}
