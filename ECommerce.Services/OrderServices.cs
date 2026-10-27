using AutoMapper;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.OrderModules;
using ECommerce.Domain.Entities.ProductModule;
using ECommerce.Services.Specification;
using ECommerce.ServicesAbstarction;
using ECommerce.Shared.CommonResult;
using ECommerce.Shared.DTOS.OrderDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services
{
    public class OrderServices : IOrderServices
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBasketRepository _basketRepository;

        public OrderServices(IMapper mapper, IUnitOfWork unitOfWork, IBasketRepository basketRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _basketRepository = basketRepository;
        }
        public async Task<Result<OrderToReturnDTO>> CreateOrderAsync(OrderDTO orderDTO, string Email)
        {
            // 1- Map AddressDTO => Address Entity
            var OrderAddress = _mapper.Map<OrderAddress>(orderDTO.Address);

            // 2- Get Basket by Id From Basket Repo
            var Basket = await _basketRepository.GetBasketAsync(orderDTO.BasketId);
            if (Basket is null)
                return Error.NotFound("Basket not found!");

            // Payment
            ArgumentException.ThrowIfNullOrEmpty(Basket.PaymentIntentId);
            var OrderReop = _unitOfWork.GetRepository<Order, Guid>();
            var spec = new OrderWithPaymentIntentSpecification(Basket.PaymentIntentId);
            var ExistOrder = await OrderReop.GetByIdAsync(spec);

            if (ExistOrder is not null) OrderReop.Remove(ExistOrder);


            // 3- Create List<OrderItems>
            List<OrderItem> orderItems = new List<OrderItem>();

            foreach (var item in Basket.Items)
            {
                var Product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(item.Id);
                if (Product is null)
                    return Error.NotFound($"Product with {item.Id} not found.");

               orderItems.Add(CreateOrderItem(item, Product));
            }


            // 4-Get Delivery Method
            var DeliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(orderDTO.DeliveryMethodId);
            if (DeliveryMethod is null)
                return Error.NotFound($"Delivery Method with {orderDTO.DeliveryMethodId} not found.");

            //5- Create Sub Total
            var SubTotal = orderItems.Sum(I => I.Price * I.Quantity);

            //6- Create Order
            var Order = new Order()
            {
                Address = OrderAddress,
                DeliveryMethod = DeliveryMethod,
                SubTotal = SubTotal,
                Items = orderItems,
                UserEmail = Email,
                PaymentIntentId = Basket.PaymentIntentId
            };

            // 7-Svae Order in Database
            await _unitOfWork.GetRepository<Order, Guid>().AddAsync(Order);

            var result = await _unitOfWork.SaveChangesAsync();
            if (result == 0)
                return Error.NotFound("Error in create order!");

            // 8- Map Order => OrderToReturnDTO
            return _mapper.Map<OrderToReturnDTO>(Order);
        }

        private static OrderItem CreateOrderItem(Domain.Entities.BasketModule.BasketItem item, Product Product)
        {
            return new OrderItem()
            {
                Product = new ProductItemOrder()
                {
                    ProductId = Product.Id,
                    ProductName = Product.Name,
                    PictureUrl = Product.PictureUrl
                },

                Price = Product.Price,
                Quantity = item.Quantity
            };
        }

        public async Task<Result<IEnumerable<OrderToReturnDTO>>> GetAllOrdersAsync(string Email)
        {
            var spec = new OrderSpecification(Email);
            var Orders = await _unitOfWork.GetRepository<Order, Guid>().GetAllAsync(spec);
            if(!Orders.Any())
                return Error.NotFound("Order Not Found");

            var Data = _mapper.Map<IEnumerable<OrderToReturnDTO>>(Orders);

            return Result<IEnumerable<OrderToReturnDTO>>.Ok(Data);
        }

        public async Task<Result<IEnumerable<DeliveryMethodDTO>>> GetDeliveryMethodsAsync()
        {
            var DeliveryMethods = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetAllAsync();
            if (!DeliveryMethods.Any())
                return Error.NotFound("No Delivery Methods Founded!");

            var Data = _mapper.Map<IEnumerable<DeliveryMethod>, IEnumerable<DeliveryMethodDTO>>(DeliveryMethods);
            return Result<IEnumerable<DeliveryMethodDTO>>.Ok(Data);
        }

        public async Task<Result<OrderToReturnDTO>> GetOrderbyIdAsync(Guid Id, string Email)
        {
            var spec = new OrderSpecification(Id, Email);

            var Order = await _unitOfWork.GetRepository<Order, Guid>().GetByIdAsync(spec);
            if (Order is null)
                return Error.NotFound("Order not Found!");

            return _mapper.Map<OrderToReturnDTO>(Order);
        }
    }
}
