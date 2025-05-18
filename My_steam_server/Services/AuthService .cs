using Game_Net_DTOLib;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using My_steam_server.DTO_models;
using My_steam_server.Interfaces;
using My_steam_server.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace My_steam_server.Services
{
    public class AuthService:IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(IUserRepository userRepository, IPasswordHasher<User> passwordHasher, IConfiguration configuration, IRefreshTokenRepository refreshTokenRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
            _refreshTokenRepository = refreshTokenRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<NetResponse<LogInSecsessDto?>> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user == null) {
                
                return new NetResponse<LogInSecsessDto?> { resultCode = ResultCode.UserNotfound,Success=false,Message=$"user email: {dto.Email} not found",data=default};
            }

            var result = _passwordHasher.VerifyHashedPassword(user,user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                //тут вернуть исключнение не верный паароль
                return new NetResponse<LogInSecsessDto?> {Success=false,resultCode=ResultCode.WrongPassword, Message=$"user email {dto.Email}\n is uncorect",data=default};
            }


            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var accessToken = tokenHandler.CreateToken(tokenDescriptor);
            var refreshToken = await GenerateRefreshTokenAsync(user);

            return new NetResponse<LogInSecsessDto?>
            {
                Success = true,
                data = new LogInSecsessDto 
                {id=user.Id,
                NickName = user.UserName,
                UserRole= user.Role,
                RegisterDate = user.RigisterDate,
                tokens = new { AccessToken = tokenHandler.WriteToken(accessToken), RefreshToken = refreshToken }.ToString() }
            };
        }

        public async Task<NetResponse<bool>> LogOutAsync(string RefrashToken)
        {
            var tokenHendler = await _refreshTokenRepository.GetByTokenAsync(RefrashToken);
            if(tokenHendler == null) return new NetResponse<bool> { Success = false };

            await _refreshTokenRepository.RemoveAsync(RefrashToken);

            return new NetResponse<bool> { Success = true, data = true };
        }

        public async Task<NetResponse<LogInSecsessDto?>> RefreshTokenAsync(string refreshToken)
        {
            var existingRefrashToken = await _refreshTokenRepository.GetByTokenAsync(refreshToken);
            if (existingRefrashToken == null || existingRefrashToken.IsExpired)
            {
                return new NetResponse<LogInSecsessDto?>
                {
                    Success = false,
                    resultCode = ResultCode.InvalidRefreshToken,
                    Message = "Refresh token is invalid or expired",
                    data = default
                };
            }

            var user = await _userRepository.GetByIdAsync(existingRefrashToken.UserId.ToString());
            var newAccessToken = await GenerateAccessTokenAsync(user);
            var newRefreshToken = await GenerateRefreshTokenAsync(user);

            return new NetResponse<LogInSecsessDto?>
            {
                Success = true,
                data = new LogInSecsessDto {
                    id=user.Id,
                    tokens= new { AccessToken = newAccessToken, RefreshToken = newRefreshToken }.ToString(),
                    RegisterDate = user.RigisterDate,
                    UserRole = user.Role,
                    NickName = user.UserName
                }
            };
        }

        public async Task<NetResponse<bool>> RegisterAsync(RegisterDto dto)
        {
            var existing = await _userRepository.GetByEmailAsync(dto.Email);
            if (existing != null)
            {
                // Тут в будушем помеестить исключение
                return new NetResponse<bool> {Success=false, resultCode = ResultCode.EmailAlredyTaken, Message= $"email {dto.Email} aredy taken", data=false};
            }

            var user = new User
            {
                UserName = dto.Username,
                Email = dto.Email
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);
            await _userRepository.AddUserAsync(user);
            await _userRepository.SaveChangesAsync();
            return new NetResponse<bool> {Success=true,data=true};
        }

        private async Task<string> GenerateAccessTokenAsync(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email)
            }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        private async Task<string> GenerateRefreshTokenAsync(User user)
        {
            var refreshToken = new RefreshToken
            {
                Token = Guid.NewGuid().ToString(),
                Created = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddDays(7),
                CreatedByIp = GetIpAddress(), // Получаем реальный IP
                UserId = user.Id
            };

            await _refreshTokenRepository.AddAsync(refreshToken);
            await _refreshTokenRepository.SaveChangesAsync();

            return refreshToken.Token;
        }

        private string GetIpAddress()
        {
            var ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString();

            if (string.IsNullOrWhiteSpace(ipAddress))
            {
                ipAddress = _httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"];
            }

            return ipAddress ?? "Unknown"; // Возвращаем "Unknown", если IP не удалось получить
        }
    }
}
