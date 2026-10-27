using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities.ProductModule
{
    public class Product : BaseEntity<int>
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string PictureUrl { get; set; } = null!;
        public decimal Price { get; set; }

        #region [1 - M] ProductBrand <=> Product
        //FK
        public int BrandId { get; set; }

        //Navigation Property
        public ProductBrand ProductBrand { get; set; }
        #endregion

        #region [1 - M] ProductType <=> Product
        //FK
        public int TypeId { get; set; }

        //Navigation Property
        public ProductType ProductType { get; set; }
        #endregion

    }
}
