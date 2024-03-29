﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProniaOnion.Application.Abstraction.Services;
using ProniaOnion.Application.DTOs.Products;
using ProniaOnion.Domain.Enums;
using ProniaOnion.Domain.Extentions;

namespace ProniaOnion.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [AuthorizeRoles(UserRole.Admin,UserRole.Moderator)]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> Get(int page=1,int take = 3)
        {
            HttpContextAccessor contextAccessor = new HttpContextAccessor();
            return Ok(await _service.GetAllPaginatedAsync(page,take));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _service.GetByIdAsync(id));
        }

        [HttpGet("order/{id}")]
        public async Task<IActionResult> GetByOrder(string data, bool isDescending = false, int page = 1, int take = 3)
        {
            var result = await _service.GetAllOrderByAsync(data, isDescending, page, take, false);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] ProductCreateDto dto)
        {
            await _service.CreateAsync(dto);
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPut]

        public async Task<IActionResult> Update(int id, [FromForm] ProductUpdateDto dto)
        {
            if(id<=0) return StatusCode(StatusCodes.Status400BadRequest);
            await _service.UpdateAsync(id,dto);
            return StatusCode(StatusCodes.Status204NoContent);
        }

        [HttpDelete("SoftDelete/{id}")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            if (id <= 0) return StatusCode(StatusCodes.Status400BadRequest);
            await _service.SoftDeleteAsync(id);
            return StatusCode(StatusCodes.Status200OK);
        }

        [HttpDelete("ReverseDelete/{id}")]

        public async Task<IActionResult> ReverseDelete(int id)
        {
            if (id <= 0) return StatusCode(StatusCodes.Status400BadRequest);
            await _service.ReverseSoftDelete(id);
            return StatusCode(StatusCodes.Status200OK);
        }
        
        [HttpDelete("DeleteFromDb/{id}")]
        [AuthorizeRoles(UserRole.Admin)]
        public async Task<IActionResult> HardDelete(int id)
        {
            if (id <= 0) return StatusCode(StatusCodes.Status400BadRequest);
            await _service.Delete(id);
            return StatusCode(StatusCodes.Status200OK);
        }


    }
}
