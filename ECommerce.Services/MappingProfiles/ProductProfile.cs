using AutoMapper;
using ECommerce.Domain.Entities.ProductModule;
using ECommerce.Shared.DTOS.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.MappingProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductBrand, BrandDTO>();
            CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.ProductTye, opt => opt.MapFrom(src => src.ProductType.Name))
                .ForMember(dest => dest.ProductBrand, opt => opt.MapFrom(src => src.ProductBrand.Name));

            CreateMap<ProductType, TypeDTO>();
        }
    }
}
