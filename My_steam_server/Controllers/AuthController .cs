﻿using Game_Net_DTOLib;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using My_steam_server.DTO_models;
using My_steam_server.Interfaces;
using My_steam_server.Models;
using My_steam_server.Services;
using System.Security.Claims;

namespace My_steam_server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController: ControllerBase
    {

        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var result = await _authService.RegisterAsync(dto);
            if(!result.Success)
                return Ok(result);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);
            if(result == null)
            {
                return Unauthorized("Invalid credentials");
            }

            return Ok(result);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] Game_Net_DTOLib.RefreshTokenRequest refreshToken)
        {
            var result  = await _authService.RefreshTokenAsync(refreshToken.RefrashToken);
            if (result.Success)
            {
                return Ok(result);
            }

            return Unauthorized(result);
        }

        [HttpGet("CheckToken")]
        [Authorize]
        public IActionResult IsValidToken()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if(userIdClaim != null && long.TryParse(userIdClaim.Value, out var userId)){
                return Ok(new Game_Net_DTOLib.NetResponse<long> { Success = true, data = userId});

            }
            return Unauthorized();
        }

        [HttpPost("log_out")]
        [Authorize]
        public async Task<IActionResult> LogOut([FromBody] LogOutDto request)
        {
            var result = await _authService.LogOutAsync(request.RefreshToken);

            return Ok(result);
        }
    }
}
