using ECommerce.Presentation.Attributes;
using ECommerce.ServicesAbstarction;
using ECommerce.Shared;
using ECommerce.Shared.DTOS.ProductDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presentation.Controllers
{
    public class ProductsController : ApiBaseController
    {
        private readonly IProductsServices _productsServices;

        public ProductsController(IProductsServices productsServices)
        {
            _productsServices = productsServices;
        }

        #region Get All Products

        [Authorize]
        [HttpGet]
        [RedisCache]
        public async Task<ActionResult<PaginatedResult<ProductDTO>>> GetAllProductsAsync([FromQuery] ProductQueryParms parms)
        {
            var Products = await _productsServices.GetAllProductsAsync(parms);
            return Ok(Products);
        }

        #endregion

        #region Get Product By Id

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProductByIdAsync(int id)
        {
            var Result = await _productsServices.GetProductByIdAsync(id);
            return HandleResult<ProductDTO>(Result);
        }

        #endregion

        #region Get All Brands

        [HttpGet("brands")]
        public async Task<ActionResult<IEnumerable<BrandDTO>>> GetAllBrandsAsync()
        {
            var Brands = await _productsServices.GetAllBrandsAsync();
            return Ok(Brands);
        }
        #endregion

        #region Get All Types

        [HttpGet("types")]
        public async Task<ActionResult<IEnumerable<BrandDTO>>> GetAllTypesAsync()
        {
            var Types = await _productsServices.GetAllTypesAsync();
            return Ok(Types);
        }
        #endregion
    }
}
