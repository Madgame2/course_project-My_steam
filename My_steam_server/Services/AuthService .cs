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
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IConfiguration configuration, IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _passwordHasher = passwordHasher;
        }

        public async Task<NetResponse<string>> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user == null) {
                
                return new NetResponse<string> { resultCode = ResultCode.UserNotfound,Success=false,Message=$"user email: {dto.Email} not found",data=string.Empty};
            }

            var result = _passwordHasher.VerifyHashedPassword(user,user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                //тут вернуть исключнение не верный паароль
                return new NetResponse<string> {Success=false,resultCode=ResultCode.WrongPassword, Message=$"user email {dto.Email}\n is uncorect",data=string.Empty};
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

            var token  = tokenHandler.CreateToken(tokenDescriptor);
            return new NetResponse<string> {Success=true,  data=tokenHandler.WriteToken(token) };
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
    }
}
