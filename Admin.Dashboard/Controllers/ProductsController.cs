using Admin.Dashboard.Helpers;
using Admin.Dashboard.Models.Products;
using AutoMapper;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.ProductModule;
using ECommerce.Services.Specification;
using ECommerce.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Dashboard.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var productRepo = _unitOfWork.GetRepository<Product, int>();

            var queryParams = new ProductQueryParms();
            var specification = new ProductWithTypeAndBrandSpec(queryParams, true);

            var products = await productRepo.GetAllAsync(specification);

            var productsViewModel = products.Select(product => new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                PictureUrl = product.PictureUrl,
                Price = product.Price,
                BrandId = product.BrandId,
                TypeId = product.TypeId,
                Brand = product.ProductBrand,
                Type = product.ProductType

            });

            return View(productsViewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Image is not null)
                    model.PictureUrl = PictureSettings.UploadFile(model.Image, "products");
                //else
                // adding default image

                var mappedProduct = new Product
                {
                    Name = model.Name,
                    Description = model.Description,
                    BrandId = model.BrandId,
                    TypeId = model.TypeId,
                    Price = model.Price,
                    PictureUrl = model.PictureUrl!
                };

                var productRepo = _unitOfWork.GetRepository<Product, int>();
                await productRepo.AddAsync(mappedProduct);
                await _unitOfWork.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(id);
            var mappedProduct = _mapper.Map<Product, ProductViewModel>(product);
            return View(mappedProduct);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ProductViewModel model)
        {
            if (id != model.Id)
                return NotFound();
            if (ModelState.IsValid)
            {
                if (model.Image != null)
                {
                    if (model.PictureUrl != null)
                    {
                        PictureSettings.DeleteFile(model.PictureUrl, "products");
                    }
                    model.PictureUrl = PictureSettings.UploadFile(model.Image, "products");
                }
                var mappedProduct = _mapper.Map<ProductViewModel, Product>(model);
                _unitOfWork.GetRepository<Product, int>().Update(mappedProduct);
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(model);
        }


        public async Task<IActionResult> Delete(int id)
        {
            var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(id);
            var mappedProduct = _mapper.Map<Product, ProductViewModel>(product);

            return View(mappedProduct);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id, ProductViewModel model)
        {
            if (id != model.Id)
                return NotFound();
            try
            {
                var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(id);
                if (product.PictureUrl != null)
                    PictureSettings.DeleteFile(product.PictureUrl, "products");
                _unitOfWork.GetRepository<Product, int>().Remove(product);
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (System.Exception)
            {
                return View(model);
            }
        }

    }
}
