using ProniaOnion.Application.DTOs.Tokens;
using ProniaOnion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProniaOnion.Application.Abstraction.Services
{
    public interface ITokenHandlerService
    {
        TokenResponseDto CreateJwt(AppUser user,int minutes);
    }
}
