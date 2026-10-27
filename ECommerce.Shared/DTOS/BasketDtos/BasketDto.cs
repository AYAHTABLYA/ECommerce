using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.DTOS.BasketDtos
{
    public record BasketDto(
        string Id,
        ICollection<BasketItemDto> items,
        string? ClientSecret,
        string? PaymentIntentId,
        int? DeliveryMethodId,
        decimal? ShippingPrice
    );
}
