using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities.ProductModule
{
    public class ProductBrand : BaseEntity<int>
    {
        public string Name { get; set; } = null!;

        #region [1 - M] ProductBrand <=> Product

        //Navigation Property
        public ICollection<Product> Products { get; set; } = new HashSet<Product>();

        #endregion

    }
}
