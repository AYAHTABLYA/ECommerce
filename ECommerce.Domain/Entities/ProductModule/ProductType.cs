using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities.ProductModule
{
    public class ProductType : BaseEntity<int>
    {
        public string Name { get; set; } = null!;

        #region [1 - M] ProductType <=> Product
        //Navigation Property
        public ICollection<Product> Products { get; set; } = new HashSet<Product>();
        #endregion

    }
}
