using Microsoft.AspNetCore.Mvc;
using ProniaOnion.Application.Abstraction.Services;
using ProniaOnion.Application.DTOs.Colors;
using ProniaOnion.Domain.Extentions;
using ProniaOnion.Domain.Enums;

namespace ProniaOnion.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [AuthorizeRoles(UserRole.Admin, UserRole.Moderator)]

    public class ColorsController : ControllerBase
    {
        private readonly IColorService _service;

        public ColorsController(IColorService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task <IActionResult> Get(int page=1, int take=3)
        {
            return Ok(await _service.GetAllAsync(page, take));
        }

        [HttpGet("{Id}")]

        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0) return StatusCode(StatusCodes.Status400BadRequest);
            return Ok(await _service.GetByIdAsync(id));
        }

        [HttpGet("Order/{id}")]

        public async Task<IActionResult> GetByOrder(string data,bool isDescending = false,int page=1,int take=3)
        {
            var result = await _service.GetAllOrderByAsync(data,isDescending,page,take,false);
            return Ok(result);
        }

        [HttpPost]

        public async Task<IActionResult> Create([FromForm]ColorCreateDto dto)
        {
            await _service.CreateAsync(dto);
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpDelete("SoftDelete/{id}")]

        public async Task<IActionResult> SoftDelete(int id)
        {
            if (id <= 0) return StatusCode(StatusCodes.Status400BadRequest);

            await _service.SoftDeleteAsync(id);

            return StatusCode(StatusCodes.Status200OK);

        }

        [HttpDelete("DeleteFromDb/{id}")]

        public async Task<IActionResult> DeleteFromDb(int id)
        {
            if (id <= 0) return StatusCode(StatusCodes.Status400BadRequest);
            await _service.HardDeleteAsync(id);
            return StatusCode(StatusCodes.Status200OK);
        }

        [HttpDelete("ReverseSoftDelete/{id}")]
        public async Task<IActionResult> ReverseDelete(int id)
        {
            if (id <= 0) return StatusCode(StatusCodes.Status400BadRequest);
            await _service.ReverseSoftDelete(id);
            return StatusCode(StatusCodes.Status200OK);
        }

    }
}
