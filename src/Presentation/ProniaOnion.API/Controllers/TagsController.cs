using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProniaOnion.Application.Abstraction.Services;
using ProniaOnion.Application.DTOs.Categories;
using ProniaOnion.Application.DTOs.Tags;

namespace ProniaOnion.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ITagService _service;

        public TagsController(ITagService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int take = 3)
        {
            var result = await _service.GetAllAsync(page, take);
            return Ok(result);
        }
        [HttpGet("[controller]/order")]
        public async Task<IActionResult> GetByOrder(string data, bool isDescending = false, int page = 1, int take = 3)
        {
            var result = await _service.GetAllOrderByAsync(data, isDescending, page, take, false);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] TagCreateDto tagDto)
        {
            await _service.CreateAsync(tagDto);
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] TagUpdateDto tagDto)
        {
            if (id <= 0) return StatusCode(StatusCodes.Status400BadRequest);
            await _service.Update(id, tagDto);
            return NoContent();

        }

    }
}
