﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProniaOnion.Application.Abstraction.Services;
using ProniaOnion.Application.DTOs.Users;

namespace ProniaOnion.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IAuthenticationService _service;

        public UsersController(IAuthenticationService service)
        {
            _service = service;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm]RegisterDto dto)
        {
            await _service.Register(dto);
            return NoContent();
        }

        [HttpPost("login")]

        public async Task<IActionResult> Login([FromForm]LoginDto dto)
        {
            return Ok(await _service.Login(dto));
        }

        [HttpPost("LoginByRefresh")]

        public async Task<IActionResult> LoginByRefresh(string refToken)
        {
            return Ok(await _service.LoginByRefreshToken(refToken));
        }

    }
}
