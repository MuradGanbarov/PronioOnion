using ProniaOnion.Application.DTOs.Categories;
using ProniaOnion.Application.DTOs.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProniaOnion.Application.Abstraction.Services
{
    public interface ITagService
    {
        Task<ICollection<TagItemDto>> GetAllAsync(int page, int take);
        //Task<GetCategoryDto> GetByIdAsync(int id);
        Task<ICollection<TagItemDto>> GetAllOrderByAsync(string OrderBy, bool isDescending, int page, int take, bool isTracking);
        Task CreateAsync(TagCreateDto tagDto);
        Task Update(int id, TagUpdateDto tagUpdateDto);
        Task SoftDeleteAsync(int id);
    }
}
