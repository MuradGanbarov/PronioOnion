using AutoMapper;
using ProniaOnion.Application.DTOs.Colors;
using ProniaOnion.Application.DTOs.Products;
using ProniaOnion.Domain.Entities;

namespace ProniaOnion.Application.MappingProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product,ProductItemDto>().ReverseMap();
            CreateMap<Product, ProductGetDto>().ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.ProductTags.Select(pt => pt.Tag).ToList()))
                .ForMember(dest => dest.Colors, opt => opt.MapFrom(src => src.ProductColors.Select(pt => pt.Color).ToList())).ReverseMap();
            //.ForMember(x => x.ProductTags, opt => opt.Ignore()).ForMember(x => x.ProductColors, opt => opt.Ignore());
            CreateMap<ProductCreateDto, Product>().ReverseMap();
            CreateMap<ProductUpdateDto, Product>().ReverseMap();
            CreateMap<Product, IncludeColorDto>().ReverseMap();

        }
    }
}
