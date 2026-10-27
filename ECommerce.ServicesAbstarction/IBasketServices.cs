using ECommerce.Shared.DTOS.BasketDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.ServicesAbstarction
{
    public interface IBasketServices
    {
        Task<BasketDto> GetBasketAsync(string id);
        Task<BasketDto> CreateorUpdateBasketAsync(BasketDto basket);
        Task<bool> DeleteBasketAsync(string id);
    }
}
