using Admin.Dashboard.Models.Products;
using AutoMapper;
using ECommerce.Domain.Entities.ProductModule;

namespace Admin.Dashboard.Helpers
{
    public class MapsProfile : Profile
    {
        public MapsProfile()
        {
            CreateMap<Product, ProductViewModel>().ReverseMap();
        }

    }
}
