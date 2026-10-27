using ECommerce.ServicesAbstarction;
using ECommerce.Shared.DTOS.OrderDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presentation.Controllers
{
    public class OrderController : ApiBaseController
    {
        private readonly IOrderServices _orderServices;

        public OrderController(IOrderServices orderServices)
        {
            _orderServices = orderServices;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<OrderToReturnDTO>> CreateOrder(OrderDTO orderDTO)
        {
            var Result = await _orderServices.CreateOrderAsync(orderDTO, GetEmailFromToken());
            return HandleResult(Result);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderToReturnDTO>>> GetOrders()
        {
            var Orders = await _orderServices.GetAllOrdersAsync(GetEmailFromToken());
            return HandleResult(Orders);
        }

        [Authorize]
        [HttpGet("{Id:guid}")]
        public async Task<ActionResult<OrderToReturnDTO>> GetOrderById(Guid Id)
        {
            var Orders = await _orderServices.GetOrderbyIdAsync(Id, GetEmailFromToken());
            return HandleResult(Orders);
        }

        [HttpGet("DeliverMethods")]
        public async Task<ActionResult<IEnumerable<DeliveryMethodDTO>>> GetDeliveryMethods(Guid Id)
        {
            var DeliveryMethods = await _orderServices.GetDeliveryMethodsAsync();
            return HandleResult(DeliveryMethods);
        }

    }
}
