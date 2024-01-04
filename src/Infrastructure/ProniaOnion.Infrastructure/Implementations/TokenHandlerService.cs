using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProniaOnion.Application.Abstraction.Services;
using ProniaOnion.Application.DTOs.Tokens;
using ProniaOnion.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
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
        public TokenResponseDto CreateJwt(AppUser user,IEnumerable<Claim> claims,int minutes)
        {
            
           

            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var credentionals = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken
                (
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    notBefore: DateTime.UtcNow,
                    expires: DateTime.UtcNow.AddMinutes(minutes),
                    signingCredentials : credentionals
                );
            JwtSecurityTokenHandler handler = new();
            TokenResponseDto dto = new TokenResponseDto(handler.WriteToken(token),token.ValidTo,user.UserName,CreateRefreshToken(),token.ValidTo.AddMinutes(3));
            return dto;
        }

        public string CreateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }


    }
}
