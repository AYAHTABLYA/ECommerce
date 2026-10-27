using ECommerce.Domain.Entities.ProductModule;
using ECommerce.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.Specification
{
    public class ProductWithTypeAndBrandSpec : BaseSpecification<Product, int>
    {
        public ProductWithTypeAndBrandSpec(ProductQueryParms queryParms, bool forDashboard = false) 
            : base(ProductSpecificationHelper.GetProductCriteria(queryParms))
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);

            switch (queryParms.Sorting)
            {
                case ProductSortingOptions.NameAsc:
                    AddOrderBy(p => p.Name);
                    break;
                case ProductSortingOptions.NameDesc:
                    AddOrderByDesc(p => p.Name);
                    break;
                case ProductSortingOptions.PriceAsc:
                    AddOrderBy(p => p.Price);
                    break;
                case ProductSortingOptions.PriceDesc:
                    AddOrderByDesc(p => p.Price);
                    break;
                default:
                    AddOrderBy(p => p.Id);
                    break;
            }
            if(!forDashboard)
                ApplyyPagination(queryParms._PageSize, queryParms._PageIndex);
        }

        public ProductWithTypeAndBrandSpec(int id) : base(p => p.Id == id)
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);
        }

    }
}
