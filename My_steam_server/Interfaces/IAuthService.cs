using Game_Net_DTOLib;
using My_steam_server.DTO_models;
using My_steam_server.Models;

namespace My_steam_server.Interfaces
{
    public interface IAuthService
    {
        Task<NetResponse<bool>> RegisterAsync(RegisterDto dto);
        Task<NetResponse<LogInSecsessDto>> LoginAsync(LoginDto dto);
        Task<NetResponse<LogInSecsessDto>> RefreshTokenAsync(string refreshToken);

        Task<NetResponse<bool>> LogOutAsync(string RefrashToken);

    }
}
