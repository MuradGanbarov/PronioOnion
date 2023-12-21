
using ProniaOnion.Application.DTOs.Categories;

namespace ProniaOnion.Application.Abstraction.Services
{
    public interface ICategoryService
    {
        Task<ICollection<CategoryItemDto>> GetAllAsync(int page, int take);
        //Task<GetCategoryDto> GetByIdAsync(int id);
        Task<ICollection<CategoryItemDto>> GetAllOrderByAsync(string OrderBy, bool isDescending, int page, int take, bool isTracking);
        Task CreateAsync(CategoryCreateDto categoryDto);
        Task Update(int id,CategoryUpdateDto categoryUpdateDto);
        
    }
}
