using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProniaOnion.Application.Abstraction.Services;
using ProniaOnion.Application.DTOs.Tokens;
using ProniaOnion.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace ProniaOnion.Infrastructure.Implementations
{
    public class TokenHandlerService : ITokenHandlerService
    {
        private readonly IConfiguration _configuration;

        public TokenHandlerService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public TokenResponseDto CreateJwt(AppUser user,int minutes)
        {
            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName,user.Name),
                new Claim(ClaimTypes.Surname,user.Surname)
            };

            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var credentionals = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken
                (
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiresInMinutes"])),
                    signingCredentials: credentionals
                );
            JwtSecurityTokenHandler handler = new();
            TokenResponseDto dto = new TokenResponseDto(handler.WriteToken(token),token.ValidTo,user.UserName);
            return dto;
        }
    }
}
