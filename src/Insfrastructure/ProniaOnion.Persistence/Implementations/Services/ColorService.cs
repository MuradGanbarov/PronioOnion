using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProniaOnion.Application.Abstraction.Repositories;
using ProniaOnion.Application.Abstraction.Services;
using ProniaOnion.Application.DTOs.Colors;
using ProniaOnion.Application.DTOs.Tags;
using ProniaOnion.Domain.Entities;
using System.Linq.Expressions;

namespace ProniaOnion.Persistence.Implementations.Services
{
    public class ColorService : IColorService
    {
        private readonly IColorRepository _repository;
        private readonly IMapper _mapper;

        public ColorService(IColorRepository repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task CreateAsync(ColorCreateDto colorDto)
        {
            await _repository.AddAsync(_mapper.Map<Color>(colorDto));
            await _repository.SaveChangesAsync();

        }

        public async Task<ICollection<ColorItemDto>> GetAllAsync(int page, int take)
        {
            ICollection<Color> colors = await _repository.GetAllWhere(skip: (page - 1) * take, take: take, ignoreQuery: false, isTracking: false).ToListAsync();
            ICollection<ColorItemDto> dtos = _mapper.Map<ICollection<ColorItemDto>>(colors);
            return dtos;

        }

        public async Task<ICollection<ColorItemDto>> GetAllOrderByAsync(string OrderBy, bool isDescending, int page, int take, bool isTracking)
        {
            Expression<Func<Color, object>> expression = GetOrderExpression(OrderBy);
            ICollection<Color> colors = await _repository.GetAllOrderBy(expressionOrder: expression, isDescending: isDescending, skip: (page - 1) * take, take: take, isTracking: isTracking).ToListAsync();
            ICollection<ColorItemDto> dtos = _mapper.Map<ICollection<ColorItemDto>>(colors);
            return dtos;
        }

        public async Task SoftDeleteAsync(int id)
        {
            Color color = await _repository.GetByIdAsync(id);
            if (color is null) throw new Exception("Not found");
            _repository.SoftDelete(color);
            await _repository.SaveChangesAsync();
        }

        public async Task Update(int id, ColorUpdateDto colorDtos)
        {
            Color color = await _repository.GetByIdAsync(id) ?? throw new Exception("Not found");
            _mapper.Map(colorDtos, color);
            _repository.Update(color);
            await _repository.SaveChangesAsync();

        }

        public Expression<Func<Color,object>> GetOrderExpression(string OrderBy)
        {
            Expression<Func<Color,object>>? expression = null;

            switch(OrderBy.ToLower())
            {
                case "Name":
                    expression = e => e.Name;
                        break;

                case "Id":
                    expression = e => e.Id;
                    break;
            }
            return expression;
        }

        public async Task<ColorItemDto> GetByIdAsync(int id)
        {
            Color existed = await _repository.GetByIdAsync(id)??throw new Exception("This color is not exist");
            ColorItemDto dto = _mapper.Map<ColorItemDto>(existed);
            return dto;
        }

        public async Task HardDeleteAsync(int id)
        {
            Color color = await _repository.GetByIdAsync(id) ?? throw new Exception("This color doesn't found");
            
            if(color.IsDeleted == true)
            {
                _repository.ReverseSoftDelete(color);
                _repository.Delete(color);
            }

            else _repository.Delete(color);

            await _repository.SaveChangesAsync();
        }

        public async Task ReverseSoftDelete(int id)
        {

            Color color = await _repository.GetByIdAsync(id) ?? throw new Exception("This Color doesn't found");
            _repository.ReverseSoftDelete(color);
            await _repository.SaveChangesAsync();
        }
    }
}
