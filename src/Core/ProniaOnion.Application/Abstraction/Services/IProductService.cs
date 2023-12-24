using ProniaOnion.Application.DTOs.Categories;
using ProniaOnion.Application.DTOs.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProniaOnion.Application.Abstraction.Services
{
    public interface IProductService
    {

        Task<IEnumerable<ProductItemDto>> GetAllPaginatedAsync(int page, int take);
        Task<ProductGetDto> GetByIdAsync(int id);
        Task<ICollection<ProductItemDto>> GetAllOrderByAsync(string OrderBy, bool isDescending, int page, int take, bool isTracking);

        Task CreateAsync(ProductCreateDto productDto);


    }
}
