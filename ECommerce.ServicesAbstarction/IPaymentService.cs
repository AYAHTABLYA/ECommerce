using ECommerce.Shared.DTOS.BasketDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.ServicesAbstarction
{
    public interface IPaymentService
    {
        Task<BasketDto> CreateOrUpdatePaymentIntentAsync(string BasketId);
    }
}
