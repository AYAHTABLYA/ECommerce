using ECommerce.Domain.Entities.OrderModules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.Specification
{
    public class OrderSpecification : BaseSpecification<Order, Guid>
    {
        public OrderSpecification(string Email) : base(O => O.UserEmail == Email)
        {
            AddInclude(O => O.DeliveryMethod);
            AddInclude(O => O.Items);
            AddOrderByDesc(O => O.OrderDate);
        }

        public OrderSpecification(Guid id, string email): base(O => O.Id == id
                                 && (string.IsNullOrEmpty(email) || O.UserEmail.ToLower() == email.ToLower())
                                 )
        {
            AddInclude(O => O.DeliveryMethod);
            AddInclude(O => O.Items);
        }
    }
}
