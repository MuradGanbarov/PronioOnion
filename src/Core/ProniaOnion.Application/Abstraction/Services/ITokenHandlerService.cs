using ProniaOnion.Application.DTOs.Tokens;
using ProniaOnion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProniaOnion.Application.Abstraction.Services
{
    public interface ITokenHandlerService
    {
        TokenResponseDto CreateJwt(AppUser user,IEnumerable<Claim> claim,int minutes);
        string CreateRefreshToken();
    }
}
