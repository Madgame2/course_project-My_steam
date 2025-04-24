using Game_Net_DTOLib;
using My_steam_server.DTO_models;
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



        public async Task<NetResponse<string>> LoginAsync(LoginDto dto) 
        {
            string json = "";

            try
            {
                json = JsonSerializer.Serialize(dto);
                return await _comMannager.SendMessageRest<string>("api/auth/login", Protocol.Https, json);

            }
            catch(UndefinedProtocolException ex)
            {
                Debug.WriteLine("can't use HTTPs protocol, recomended to defind HTTPs protoclo");
                try
                {
                    return await _comMannager.SendMessageRest<string>("api/auth/login", Protocol.Http, json);
                }
                catch
                {
                    return new NetResponse<string> { Success = false, Message = ex.Message };
                }
            }
            catch(Exception ex) {

                return new NetResponse<string> { Success = false, Message = ex.Message };
            }
        }
    }
}
