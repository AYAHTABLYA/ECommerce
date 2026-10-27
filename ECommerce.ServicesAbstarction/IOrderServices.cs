using ECommerce.Shared.CommonResult;
using ECommerce.Shared.DTOS.OrderDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.ServicesAbstarction
{
    public interface IOrderServices
    {
        Task<Result<OrderToReturnDTO>> CreateOrderAsync(OrderDTO orderDTO, string Email);

        Task<Result<IEnumerable<OrderToReturnDTO>>> GetAllOrdersAsync(string Email);
        Task<Result<IEnumerable<DeliveryMethodDTO>>> GetDeliveryMethodsAsync();
        Task<Result<OrderToReturnDTO>> GetOrderbyIdAsync(Guid Id, string Email);
    }
}
