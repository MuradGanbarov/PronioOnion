using AutoMapper;
using ProniaOnion.Application.DTOs.Colors;
using ProniaOnion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProniaOnion.Application.MappingProfiles
{
    public class ColorProfile : Profile
    {
        public ColorProfile()
        {
            CreateMap<Color,ColorItemDto>().ReverseMap();
            CreateMap<ColorCreateDto,Color>().ReverseMap();
            CreateMap<ColorUpdateDto, Color>().ReverseMap();
            CreateMap<Color,IncludeColorDto>().ReverseMap();
            CreateMap<ProductColors, IncludeColorDto>().ReverseMap();
        }
    }
}
