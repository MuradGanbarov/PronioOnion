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
            string[] includes = {"Category","ProductColors.Color","ProductTags.Tag"};

            Product product = await _repository.GetByIdAsync(id, includes: includes);
                if(product is null)
                   throw new Exception("This Product doesn't exist");

            ProductGetDto dto = _mapper.Map<ProductGetDto>(product);
            dto.Colors = new List<IncludeColorDto>();
            dto.Tags = new List<IncludeTagDto>();

            foreach (ProductColors color in product.ProductColors)
            {
                dto.Colors.Add(_mapper.Map<IncludeColorDto>(color.Color));
            }

            foreach (ProductTags tag in product.ProductTags)
            {
                dto.Tags.Add(_mapper.Map<IncludeTagDto>(tag.Tag));
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

        

        public async Task UpdateAsync(int id,ProductUpdateDto dto)
        {
            string[] includes = {$"{nameof(Product.ProductColors)}", $"{nameof(Product.ProductTags)}"};
            Product existed = await _repository.GetByIdAsync(id,includes:includes);
            if (existed is null) throw new Exception("Product is not exist");

            if(dto.CategoryId != existed.CategoryId)
            {
                if (!await _categoryRepository.IsExistAsync(c => c.Id == dto.CategoryId)) throw new Exception("Category didn't found");
                 
            }

            existed = _mapper.Map(dto, existed);
            existed.ProductColors = existed.ProductColors.Where(pc => dto.ColorIds.Any(colId => pc.ColorId == colId)).ToList();
            existed.ProductTags = existed.ProductTags.Where(pt => dto.TagIds.Any(tagId => pt.TagId == tagId)).ToList();
            foreach(var cId in dto.ColorIds)
            {
                if(!await _colorRepository.IsExistAsync(c=>c.Id == cId))
                    throw new Exception("This color is not existed");
                if (!existed.ProductColors.Any(pc => pc.ColorId == cId))
                {
                    existed.ProductColors.Add(new ProductColors { ColorId = cId });
                }
            }

            foreach(var tId in dto.TagIds)
            {
                if(!await _tagRepository.IsExistAsync(t=>t.Id==id))
                    throw new Exception("This tag is not existed");
                if(!existed.ProductTags.Any(pt=>pt.TagId == tId))
                {
                    existed.ProductTags.Add(new ProductTags { TagId = tId });
                }
            }

            _repository.Update(existed);
            await _repository.SaveChangesAsync();

        }


        public async Task SoftDeleteAsync(int id)
        {
            Product product = await _repository.GetByIdAsync(id) ?? throw new Exception("This product doesn't found");
            _repository.SoftDelete(product);
            await _repository.SaveChangesAsync();
        }

        public async Task ReverseSoftDelete(int id)
        {
            Product product = await _repository.GetByIdAsync(id) ?? throw new Exception("This product doesn't found");
            _repository.ReverseSoftDelete(product);
            await _repository.SaveChangesAsync();
        }
        public async Task Delete(int id)
        {
            Product product = await _repository.GetByIdAsync(id) ?? throw new Exception("This product doesn't found");
            if (product.IsDeleted == true)
            {
                _repository.ReverseSoftDelete(product);
                _repository.Delete(product);
            }
            else _repository.Delete(product);

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
