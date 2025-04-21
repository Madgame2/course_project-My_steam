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
            string json = "";
            try
            {
                json = JsonSerializer.Serialize(registerDto);
                
                NetResponse<bool> ressult = await _comMannager.SendMessageRest<bool>("api/auth/register", Protocol.Https, json);

                return ressult;
            }
            catch (UnDefindedProtocolExaption exaption)
            {
                Debug.WriteLine("can't use HTTPs protocol, recomended to defind HTTPs protoclo");
                try
                {
                    NetResponse<bool> ressult = await _comMannager.SendMessageRest<bool>("api/auth/register", Protocol.Http, json);
                    return ressult;
                }
                catch 
                {
                   return new NetResponse<bool> { Success = false,Message = "No definded protocols for net work" };
                }

            }
            catch (Exception ex)
            {
                return new NetResponse<bool> {Success = false,Message= ex.Message};
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
            catch(UnDefindedProtocolExaption ex)
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
