using AutoMapper;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.BasketModule;
using ECommerce.Domain.Entities.OrderModules;
using ECommerce.Domain.Entities.ProductModule;
using ECommerce.Services.Exceptions;
using ECommerce.ServicesAbstarction;
using ECommerce.Shared.DTOS.BasketDtos;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public PaymentService(IBasketRepository basketRepository, IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
        }
        public async Task<BasketDto> CreateOrUpdatePaymentIntentAsync(string BasketId)
        {
            // 1- Config Stripe
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];
            
            // 2- Get Basket Id from repo
            var Basket = await _basketRepository.GetBasketAsync(BasketId);
            if (Basket == null)
                throw new BasketNotFoundException(BasketId);

            // 3- Amount => Calculate shipping price 
            var Product = _unitOfWork.GetRepository<Domain.Entities.ProductModule.Product, int>();
            foreach (var item in Basket.Items)
            {
                var product = await Product.GetByIdAsync(item.Id) ?? throw new ProductNotFoundException(item.Id);
                
                item.Price = product.Price;
            }
            var DeliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(Basket.DeliveryMethodId.Value);

            Basket.ShippingPrice = DeliveryMethod.Price;

            var BasketAmount = (long) (Basket.Items.Sum(item => item.Quantity * item.Price) + DeliveryMethod.Price) * 100;
            
            // 4- Create or Update Payment Intent
            var PaymentService = new PaymentIntentService();
            if (Basket.PaymentIntentId is null)
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = BasketAmount,
                    Currency = "USD",
                    PaymentMethodTypes = ["card"]
                };
                var PaymentIntent = await PaymentService.CreateAsync(options);
                Basket.PaymentIntentId = PaymentIntent.Id;
                Basket.ClientSecret = PaymentIntent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = BasketAmount,
                };

                await PaymentService.UpdateAsync(Basket.PaymentIntentId, options);
            }

            await _basketRepository.CreateorUpdateBasketAsync(Basket);

            return _mapper.Map<CustomerBasket, BasketDto>(Basket);
        }
    }
}
