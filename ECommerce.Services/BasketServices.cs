using AutoMapper;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.BasketModule;
using ECommerce.Services.Exceptions;
using ECommerce.ServicesAbstarction;
using ECommerce.Shared.DTOS.BasketDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services
{
    public class BasketServices : IBasketServices
    {
        private readonly IMapper _mapper;
        private readonly IBasketRepository _basketRepository;

        public BasketServices(IBasketRepository basketRepository, IMapper mapper)
        {
            _mapper = mapper;
            _basketRepository = basketRepository;
        }
        public async Task<BasketDto> CreateorUpdateBasketAsync(BasketDto basket)
        {
            var CustomerBasket = _mapper.Map<CustomerBasket>(basket);
            var CreatedOrUpdatedBasket = await _basketRepository.CreateorUpdateBasketAsync(CustomerBasket);
            return _mapper.Map<BasketDto>(CreatedOrUpdatedBasket);
        }

        public async Task<bool> DeleteBasketAsync(string id)
        {
            return await _basketRepository.DeleteBasketAsync(id);
        }

        public async Task<BasketDto> GetBasketAsync(string id)
        {
            var Basket = await _basketRepository.GetBasketAsync(id);
            if (Basket is null)
                throw new BasketNotFoundException(id);
            return _mapper.Map<BasketDto>(Basket);
        }
    }
}
