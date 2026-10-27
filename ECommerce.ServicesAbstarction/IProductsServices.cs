using ECommerce.Shared;
using ECommerce.Shared.CommonResult;
using ECommerce.Shared.DTOS.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.ServicesAbstarction
{
    public interface IProductsServices
    {
        Task<PaginatedResult<ProductDTO>> GetAllProductsAsync(ProductQueryParms queryParms);
        Task<Result<ProductDTO>> GetProductByIdAsync(int id);
        Task<IEnumerable<BrandDTO>> GetAllBrandsAsync();
        Task<IEnumerable<TypeDTO>> GetAllTypesAsync();
    }
}
