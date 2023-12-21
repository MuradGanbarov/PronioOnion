﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProniaOnion.Application.Abstraction.Repositories;
using ProniaOnion.Application.Abstraction.Services;
using ProniaOnion.Application.DTOs.Categories;
using ProniaOnion.Application.DTOs.Tags;
using ProniaOnion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProniaOnion.Persistence.Implementations.Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _repository;
        private readonly IMapper _mapper;

        public TagService(ITagRepository repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task CreateAsync(TagCreateDto tagDto)
        {
            await _repository.AddAsync(_mapper.Map<Tag>(tagDto));
            await _repository.SaveChangesAsync();
        }

        public async Task<ICollection<TagItemDto>> GetAllAsync(int page, int take)
        {
            ICollection<Tag> tags = await _repository.GetAllAsync(skip: (page - 1) * take, take: take, isTracking: false).ToListAsync();
            ICollection<TagItemDto> dtos = _mapper.Map<ICollection<TagItemDto>>(tags);
            return dtos;
        }

        public async Task<ICollection<TagItemDto>> GetAllOrderByAsync(string OrderBy, bool isDescending, int page, int take, bool isTracking)
        {

            Expression<Func<Tag, object>> expression = GetOrderExpression(OrderBy);
            var result = await _repository.GetAllAsyncOrderBy(expressionOrder: expression, isDescending: isDescending, skip: (page - 1) * take, take: take, isTracking: isTracking).ToListAsync();
            ICollection<TagItemDto> resultList = new List<TagItemDto>();
            foreach (Tag resultitem in result)
            {
                resultList.Add(new TagItemDto(resultitem.Id, resultitem.Name));

            }
            return resultList;
        }

        public async Task Update(int id, TagUpdateDto tagDtos)
        {

            Tag tag = await _repository.GetByIdAsync(id) ?? throw new Exception("Not found");
            tag.Name = tagDtos.Name;
            _repository.Update(tag);
            await _repository.SaveChangesAsync();
        }

        public Expression<Func<Tag, object>> GetOrderExpression(string orderBy)
        {
            Expression<Func<Tag, object>>? expression = null;
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
