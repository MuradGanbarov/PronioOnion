using AutoMapper;
using ProniaOnion.Application.DTOs.Colors;
using ProniaOnion.Application.DTOs.Products;
using ProniaOnion.Domain.Entities;

namespace ProniaOnion.Application.MappingProfiles
{
    internal class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product,ProductItemDto>().ReverseMap();
            CreateMap<Product,ProductGetDto>().ReverseMap().ForMember(x=>x.ProductTags, opt=>opt.Ignore()).ForMember(x=>x.ProductColors, opt=>opt.Ignore());
            CreateMap<ProductCreateDto, Product>().ReverseMap();
            CreateMap<Product, IncludeColorDto>();
        }
    }
}
