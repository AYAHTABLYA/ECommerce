using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities.OrderModules
{
    public enum OrderStatus
    {
        Pandin = 0,
        PaymentReceived = 1,
        PaymentFailed = 2,
    }
}
