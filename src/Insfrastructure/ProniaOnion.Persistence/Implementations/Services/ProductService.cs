using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProniaOnion.Application.Abstraction.Repositories;
using ProniaOnion.Application.Abstraction.Services;
using ProniaOnion.Application.DTOs.Products;
using ProniaOnion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProniaOnion.Persistence.Implementations.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductItemDto>> GetAllPaginatedAsync(int page, int take)
        {
            IEnumerable<ProductItemDto> dtos = _mapper.Map<IEnumerable<ProductItemDto>>(await _repository.GetAllWhere(skip:(page-1)*take,take:take,isTracking:false).ToArrayAsync());
            return dtos;
        }

        public async Task<ProductGetDto> GetByIdAsync(int id)
        {
            Product product = await _repository.GetByIdAsync(id,includes:nameof(Product.Category))??throw new Exception("Not found");
            ProductGetDto dto = _mapper.Map<ProductGetDto>(product);
            return dto;


        }

        public async Task<ICollection<ProductItemDto>> GetAllOrderByAsync(string OrderBy, bool isDescending, int page, int take, bool isTracking)
        {
            Expression<Func<Product, object>> expression = GetOrderExpression(OrderBy);
            ICollection<Product> products = await _repository.GetAllOrderBy(expressionOrder: expression, isDescending: isDescending, skip: (page - 1) * take, take: take, isTracking: isTracking).ToListAsync();
            ICollection<ProductItemDto> dtos = _mapper.Map<ICollection<ProductItemDto>>(products);
            return dtos;
        }

        public async Task CreateAsync(ProductCreateDto dto)
        {
            Product product = await _repository.GetByIdAsync(dto.CategoryId)??throw new Exception("This product is already existed");
            await _repository.AddAsync(_mapper.Map<Product>(dto));
            await _repository.SaveChangesAsync();
        }


        public Expression<Func<Product, object>> GetOrderExpression(string orderBy)
        {
            Expression<Func<Product, object>>? expression = null;
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
