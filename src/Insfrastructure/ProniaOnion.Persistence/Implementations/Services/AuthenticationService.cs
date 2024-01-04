using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProniaOnion.Application.Abstraction.Services;
using ProniaOnion.Application.DTOs.Tokens;
using ProniaOnion.Application.DTOs.Users;
using ProniaOnion.Domain.Entities;
using ProniaOnion.Domain.Enums;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static System.Net.WebRequestMethods;

namespace ProniaOnion.Persistence.Implementations.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IMapper _mapper;
        private readonly ITokenHandlerService _handler;
        private readonly UserManager<AppUser> _userManager;
        

        public AuthenticationService(IMapper mapper,ITokenHandlerService handler ,UserManager<AppUser> userManager)
        {
            _mapper = mapper;
            _handler = handler;
            _userManager = userManager;
        }


        public async Task Register(RegisterDto dto)
        {
            AppUser user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == dto.UserName || u.Email == dto.Email);
            if (user is not null) throw new Exception("This user already registered");
            user = _mapper.Map<AppUser>(dto);
            var result = await _userManager.CreateAsync(user,dto.Password);
            if (!result.Succeeded)
            {
                StringBuilder message = new StringBuilder();
                foreach(var error in result.Errors)
                {
                    message.AppendLine(error.Description);
                }

                throw new Exception(message.ToString());
            }

            await _userManager.AddToRoleAsync(user,UserRole.Member.ToString());

        }
        public async Task<TokenResponseDto> Login(LoginDto dto)
        {
            AppUser user = await _userManager.FindByNameAsync(dto.UserNameOrEmail);
            if (user is null)
            {
                user = await _userManager.FindByEmailAsync(dto.UserNameOrEmail);
                if (user is null) throw new Exception("Password,Email or Password incorrect");
            }
            if (!await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                throw new Exception("Password,Email or Password incorrect");
            }

            ICollection<Claim> claims = await _createClaims(user);

            
            TokenResponseDto tokenDto = _handler.CreateJwt(user,claims,1);
            user.RefreshToken = tokenDto.RefreshToken;
            user.RefreshTokenExpireAt = tokenDto.RefreshTokenExpireAt;
            await _userManager.UpdateAsync(user);
            return tokenDto;

        }

        private async Task<ICollection<Claim>> _createClaims(AppUser user)
        {
            ICollection<Claim> claims = new List<Claim>()
            {
               new Claim(ClaimTypes.NameIdentifier, user.Id),
               new Claim(ClaimTypes.Name, user.UserName),
               new Claim(ClaimTypes.Email, user.Email),
               new Claim(ClaimTypes.GivenName, user.Name),
               new Claim(ClaimTypes.Surname, user.Surname)
            };
            foreach (var role in await _userManager.GetRolesAsync(user))
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }

        public async Task<TokenResponseDto> LoginByRefreshToken(string refToken)
        {
            AppUser? user = await _userManager.Users.SingleOrDefaultAsync(u=>u.RefreshToken==refToken);
            if (user is null) throw new Exception("This user coudn't found");
            if (user.RefreshTokenExpireAt < DateTime.UtcNow) throw new Exception("Expired refresh token");
          
            TokenResponseDto tokenDto = _handler.CreateJwt(user, await _createClaims(user), 60);
            user.RefreshToken = tokenDto.RefreshToken;
            user.RefreshTokenExpireAt = tokenDto.RefreshTokenExpireAt;
            await _userManager.UpdateAsync(user);
            return tokenDto;
            
        }

    }
}
