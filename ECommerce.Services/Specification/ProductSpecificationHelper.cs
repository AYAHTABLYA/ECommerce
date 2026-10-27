using ECommerce.Domain.Entities.ProductModule;
using ECommerce.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.Specification
{
    public class ProductSpecificationHelper
    {
        public static Expression<Func<Product, bool>> GetProductCriteria(ProductQueryParms queryParms)
        {
            return (P => (!queryParms.BrandId.HasValue || P.BrandId == queryParms.BrandId.Value) &&
                  (!queryParms.TypeId.HasValue || P.TypeId == queryParms.TypeId.Value) &&
                  (string.IsNullOrEmpty(queryParms.Search) || P.Name.ToLower().Contains(queryParms.Search.ToLower()))
            );
        }

    }
}
