using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProniaOnion.Application.Abstraction.Repositories;
using ProniaOnion.Application.Abstraction.Services;
using ProniaOnion.Application.DTOs.Colors;
using ProniaOnion.Application.DTOs.Products;
using ProniaOnion.Application.DTOs.Tags;
using ProniaOnion.Domain.Entities;
using System.Linq.Expressions;
using System.Xml;

namespace ProniaOnion.Persistence.Implementations.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IColorRepository _colorRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository repository,ICategoryRepository categoryRepository,ITagRepository tagRepository,IColorRepository colorRepository,IMapper mapper)
        {
            _repository = repository;
            _categoryRepository = categoryRepository;
            _tagRepository = tagRepository;
            _colorRepository = colorRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductItemDto>> GetAllPaginatedAsync(int page, int take)
        {
            IEnumerable<ProductItemDto> dtos = _mapper.Map<IEnumerable<ProductItemDto>>(await _repository.GetAllWhere(skip:(page-1)*take,take:take,isTracking:false).ToArrayAsync());
            return dtos;
        }

        public async Task<ProductGetDto> GetByIdAsync(int id)
        {

            if (id <= 0) throw new Exception("Bad Request");
            string[] includes = { $"{nameof(Product.Category)}",$"{nameof(Product.ProductColors)}",$"{nameof(Product.ProductTags)}",$"{nameof(Product.ProductColors)}.{nameof(ProductColors.Color)}",$"{nameof(Product.ProductTags)}",$"{nameof(ProductTags.Tag)}"};

            Product product = await _repository.GetByIdAsync(id, includes: includes)??throw new Exception("This Product doesn't exist");
            ProductGetDto dto = _mapper.Map<ProductGetDto>(product);
            dto.ProductColors = new List<IncludeColorDto>();
            dto.ProductTags = new List<IncludeTagDto>();

            foreach(ProductColors color in product.ProductColors)
            {
                dto.ProductColors.Add(_mapper.Map<IncludeColorDto>(color.Color));
            }

            foreach(ProductTags tag in product.ProductTags)
            {
                dto.ProductTags.Add(_mapper.Map<IncludeTagDto>(tag.Tag));
            }

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
           if(await _repository.IsExistAsync(c=>c.Name == dto.Name))
           {
                throw new Exception("This product is already exist");
           }

            if (!await _categoryRepository.IsExistAsync(c => c.Id == dto.CategoryId)) throw new Exception("This category doesn't exist");


            Product product = _mapper.Map<Product>(dto);
            product.ProductTags = new List<ProductTags>();
            foreach (var tagId in dto.TagIds)
            {
                if (!await _tagRepository.IsExistAsync(t => t.Id == tagId)) throw new Exception("This tag is not existed");
                product.ProductTags.Add(new ProductTags{TagId = tagId});
            }
            product.ProductColors = new List<ProductColors>();
            foreach (var colorId in dto.ColorIds)
            {
                if (!await _colorRepository.IsExistAsync(c => c.Id == colorId)) throw new Exception("This color is not existed");
                product.ProductColors.Add(new ProductColors { ColorId = colorId });
            }

            await _repository.AddAsync(product);
            await _repository.SaveChangesAsync();
        }

        public async Task SoftDelete(int id)
        {
            if (id <= 0) throw new Exception("This product is not exist");
            Product existed = await _repository.GetByIdAsync(id);
            _repository.SoftDelete(existed);
            await _repository.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            Product existed = await _repository.GetByIdAsync(id)??throw new Exception("This product is not found");
            if(existed.IsDeleted == true)
            {
                _repository.ReverseSoftDelete(existed);
            }

            _repository.Delete(existed);
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
