using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.ProductModule;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Dashboard.Controllers
{
    public class ProductBrandsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductBrandsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            var brands = await _unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync();
            return View(brands);
        }
        public async Task<IActionResult> Create(ProductBrand brand)
        {
            try
            {
                await _unitOfWork.GetRepository<ProductBrand, int>().AddAsync(brand);
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (System.Exception)
            {
                ModelState.AddModelError("Name", "Please enter new name");
                return View("Index", await _unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync());
            }
        }
        public async Task<IActionResult> Delete(int id)
        {
            var brand = await _unitOfWork.GetRepository<ProductBrand, int>().GetByIdAsync(id);
            _unitOfWork.GetRepository<ProductBrand, int>().Remove(brand);
            await _unitOfWork.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
