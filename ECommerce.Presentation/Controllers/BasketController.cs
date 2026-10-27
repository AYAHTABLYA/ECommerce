using ECommerce.ServicesAbstarction;
using ECommerce.Shared.DTOS.BasketDtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presentation.Controllers
{
    public class BasketController : ApiBaseController
    {
        private readonly IBasketServices _basketServices;

        public BasketController(IBasketServices basketServices)
        {
            _basketServices = basketServices;
        }

        #region Get Basket By Id

        [HttpGet]
        public async Task<ActionResult<BasketDto>> GetBasket(string id)
        {
            var Basket = await _basketServices.GetBasketAsync(id);
            return Ok(Basket);
        }

        #endregion

        #region Create or Update Basket

        [HttpPost]
        public async Task<ActionResult<BasketDto>> CreateOrUpdateBasket(BasketDto basket)
        {
            var updatedBasket = await _basketServices.CreateorUpdateBasketAsync(basket);
            return Ok(updatedBasket);
        }

        #endregion

        #region Delete Basket By Id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBasketById(string id)
        {
            var Result = await _basketServices.DeleteBasketAsync(id);
            return Ok(Result);
        }
        #endregion
    }
}
