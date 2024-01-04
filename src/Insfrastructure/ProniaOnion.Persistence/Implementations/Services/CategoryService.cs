using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProniaOnion.Application.Abstraction.Repositories;
using ProniaOnion.Application.Abstraction.Services;
using ProniaOnion.Application.DTOs.Categories;
using ProniaOnion.Domain.Entities;
using System.Linq.Expressions;

namespace ProniaOnion.Persistence.Implementations.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        public async Task<ICollection<CategoryItemDto>> GetAllAsync(int page, int take)
        {
            ICollection<Category> categories = await _repository.GetAllWhere(skip: (page - 1) * take, take: take, isTracking: false,ignoreQuery:true).ToListAsync();
            ICollection<CategoryItemDto> dtos = _mapper.Map<ICollection<CategoryItemDto>>(categories);
            return dtos;
        }

        public async Task CreateAsync(CategoryCreateDto categoryDto)
        {
            await _repository.AddAsync(_mapper.Map<Category>(categoryDto));
            await _repository.SaveChangesAsync();
        }

        public async Task Update(int id, CategoryUpdateDto updateCategoryDto)
        {
            Category category = await _repository.GetByIdAsync(id) ?? throw new Exception("This category didn't found");
            _mapper.Map(updateCategoryDto, category);
            _repository.Update(category);
            await _repository.SaveChangesAsync();
        }

        public async Task<ICollection<CategoryItemDto>> GetAllOrderByAsync(string OrderBy, bool isDescending, int page, int take, bool isTracking)
        {
            Expression<Func<Category, object>> expression = GetOrderExpression(OrderBy);
            ICollection<Category> categories = await _repository.GetAllOrderBy(expressionOrder: expression, isDescending: isDescending, skip: (page - 1) * take, take: take, isTracking: isTracking).ToListAsync();
            ICollection<CategoryItemDto> dtos = _mapper.Map<ICollection<CategoryItemDto>>(categories);
            return dtos;
        }

        public async Task SoftDeleteAsync(int id)
        {
            Category category = await _repository.GetByIdAsync(id) ?? throw new Exception("This category doesn't found");
            _repository.SoftDelete(category);
            await _repository.SaveChangesAsync();
        }

        public async Task ReverseSoftDelete(int id)
        {
            Category category = await _repository.GetByIdAsync(id) ?? throw new Exception("This category doesn't found");
            _repository.ReverseSoftDelete(category);
            await _repository.SaveChangesAsync();
        }
        public async Task Delete(int id)
        {
            Category category = await _repository.GetByIdAsync(id) ?? throw new Exception("This category doesn't found");
            if (category.IsDeleted == true)
            {
                _repository.ReverseSoftDelete(category);
                _repository.Delete(category);
            }
            else _repository.Delete(category);

            await _repository.SaveChangesAsync();
        }
        public Expression<Func<Category, object>> GetOrderExpression(string orderBy)
        {
            Expression<Func<Category, object>>? expression = null;
            switch (orderBy.ToLower())
            {
                case "name":
                    expression = c => c.Name;
                    break;
                case "id":
                    expression = c => c.Id;
                    break;
            }

            return expression;
        }

    }
}
