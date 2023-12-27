using ProniaOnion.Application.DTOs.Users;

namespace ProniaOnion.Application.Abstraction.Services
{
    public interface IAuthenticationService
    {
        Task Register(RegisterDto dto);
    }
}
