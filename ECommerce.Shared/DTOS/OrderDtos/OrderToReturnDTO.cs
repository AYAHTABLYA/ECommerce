using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.DTOS.OrderDtos
{
    public class OrderToReturnDTO
    {
        public Guid Id { get; set; }
        public string UserEmail { get; set; } = null!;
        public DateTimeOffset OrderDate { get; set; }
        public AddressDTO Address { get; set; }
        public string DeliverMethod { get; set; } = null!;
        public string OrderStatus { get; set; } = null!;
        public ICollection<OrderItemDTO> Items { get; set; } = [];
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
    }
}
