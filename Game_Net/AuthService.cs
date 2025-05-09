using Game_Net_DTOLib;
using My_steam_server.DTO_models;
using My_steam_server.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Game_Net
{
    public class AuthService
    {

        private readonly ComunitationMannageer _comMannager;

        public AuthService(ComunitationMannageer comMannager)
        {
            _comMannager = comMannager;
        }

        public async Task<NetResponse<bool>> RegisterUser(RegisterDto registerDto)
        {
            string json = JsonSerializer.Serialize(registerDto);

            try
            {
                return await _comMannager.SendMessageRest<bool>("api/auth/register", Protocol.Https, json);
            }
            catch (UndefinedProtocolException ex)
            {
                Debug.WriteLine($"HTTPS failed: {ex.Message}");

                try
                {
                    return await _comMannager.SendMessageRest<bool>("api/auth/register", Protocol.Http, json);
                }
                catch (Exception httpEx)
                {
                    return new NetResponse<bool> { Success = false, Message = $"Both protocols failed: {httpEx.Message}" };
                }
            }
            catch (Exception ex)
            {
                return new NetResponse<bool> { Success = false, Message = ex.Message };
            }
        }



        public async Task<NetResponse<LogInSecsessDto>> LoginAsync(LoginDto dto) 
        {
            string json = "";

            try
            {
                json = JsonSerializer.Serialize(dto);
                return await _comMannager.SendMessageRest<LogInSecsessDto>("api/auth/login", Protocol.Https, json);

            }
            catch(UndefinedProtocolException ex)
            {
                Debug.WriteLine("can't use HTTPs protocol, recomended to defind HTTPs protoclo");
                try
                {
                    return await _comMannager.SendMessageRest<LogInSecsessDto>("api/auth/login", Protocol.Http, json);
                }
                catch
                {
                    return new NetResponse<LogInSecsessDto> { Success = false, Message = ex.Message };
                }
            }
            catch(Exception ex) {

                return new NetResponse<LogInSecsessDto> { Success = false, Message = ex.Message };
            }
        }

        public async Task<NetResponse<long>> isValid_JWT_Token()
        {
            try
            {
                return await _comMannager.SendMessageRest<long>("api/auth/CheckToken", Protocol.Https);
            }
            catch (UndefinedProtocolException ex)
            {
                Debug.WriteLine($"HTTPS failed: {ex.Message}");

                try
                {
                    return await _comMannager.SendMessageRest<long>("api/auth/CheckToken", Protocol.Http);
                }
                catch (UndefinedProtocolException innerEx)
                {
                    Debug.WriteLine($"HTTP also failed: {innerEx.Message}");
                    return new NetResponse<long> { Success = false, Message = "Unable to connect via HTTPS or HTTP." };
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"General network error: {ex.Message}");
                throw new Exception("Network error");
            }
        }
        
        public async Task<NetResponse<LogInSecsessDto>> sendRefrashTokenAsync()
        {
            var requestBody = new RefreshTokenRequest
            {
                RefrashToken = _comMannager.RefrashToken
            };

            var json = JsonSerializer.Serialize(requestBody);

            try
            {
                var result = await _comMannager.SendMessageRest<LogInSecsessDto>("api/auth/refresh-token", Protocol.Https, json);

                return result;

            }
            catch (UndefinedProtocolException ex)
            {
                try
                {
                    var result = await _comMannager.SendMessageRest<LogInSecsessDto>("api/auth/refresh-token", Protocol.Http, json);

                    return result;
                }
                catch (UndefinedProtocolException)
                {
                    throw new Exception("No protocols to send data");

                }
            }
        }

        public async Task<NetResponse<bool>> LogOutAsync()
        {
            var json = JsonSerializer.Serialize(new LogOutDto { RefreshToken=_comMannager.RefrashToken});

            try
            {
                return await _comMannager.SendMessageRest<bool>("api/auth/log_out", Protocol.Https, json);
            }
            catch (UndefinedProtocolException)
            {
                return await _comMannager.SendMessageRest<bool>("api/auth/log_out", Protocol.Http, json);
            }
        }
    }
}
