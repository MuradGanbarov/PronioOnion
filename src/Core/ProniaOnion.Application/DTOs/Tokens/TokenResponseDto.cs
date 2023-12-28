using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProniaOnion.Application.DTOs.Tokens
{
    public record TokenResponseDto(string Token,DateTime ExpireTime,string UserName);
    
}
