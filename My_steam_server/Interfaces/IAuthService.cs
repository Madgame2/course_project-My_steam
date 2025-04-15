using My_steam_server.DTO_models;
using My_steam_server.Models;

namespace My_steam_server.Interfaces
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(RegisterDto dto);
        Task<string?> LoginAsync(LoginDto dto);
    }
}
