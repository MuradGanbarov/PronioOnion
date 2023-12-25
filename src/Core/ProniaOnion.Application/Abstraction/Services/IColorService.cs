using ProniaOnion.Application.DTOs.Colors;

namespace ProniaOnion.Application.Abstraction.Services
{
    public interface IColorService
    {
        Task<ICollection<ColorItemDto>> GetAllAsync(int page, int take);
        Task<ColorItemDto> GetByIdAsync(int id);
        Task<ICollection<ColorItemDto>> GetAllOrderByAsync(string OrderBy, bool isDescending, int page, int take, bool isTracking);
        Task CreateAsync(ColorCreateDto colorDto);
        Task Update(int id, ColorUpdateDto colorDtos);
        Task SoftDeleteAsync(int id);
        Task HardDeleteAsync(int id);
        Task ReverseSoftDelete(int id);
    }
}
