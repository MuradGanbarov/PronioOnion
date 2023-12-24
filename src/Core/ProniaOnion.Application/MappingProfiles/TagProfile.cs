using AutoMapper;
using ProniaOnion.Application.DTOs.Categories;
using ProniaOnion.Application.DTOs.Tags;
using ProniaOnion.Domain.Entities;


namespace ProniaOnion.Application.MappingProfiles
{
    public class TagProfile : Profile
    {
        public TagProfile()
        {
            CreateMap<Tag, TagItemDto>().ReverseMap();
            CreateMap<TagCreateDto, Tag>();
            CreateMap<TagUpdateDto, Tag>().ReverseMap();
            CreateMap<Tag, IncludeTagDto>().ReverseMap();
        }
    }
}
