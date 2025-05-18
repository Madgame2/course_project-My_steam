using Game_Net_DTOLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Net
{
    public class PublisherService
    {
        private readonly Game_Net.ComunitationMannageer mannageer;

        public PublisherService(ComunitationMannageer mannageer)
        {
            this.mannageer = mannageer;
        }

        public async Task<List<ProjectDto>> GetMyProjects(string UserId)
        {
            NetResponse<List<ProjectDto>> response = null;
            try
            {
                response = await mannageer.SendMessageRest<List<ProjectDto>>($"api/Projects/{UserId}", Protocol.Https);
            }
            catch (UndefinedProtocolException)
            {
                response = await mannageer.SendMessageRest<List<ProjectDto>>($"api/Projects/{UserId}", Protocol.Http);

            }

            return response.data;
        }

        public async Task DeleteMyProject(string UserId, long GameId)
        {
            NetResponse<bool> response = null;
            try
            {
                response = await mannageer.SendMessageRest<bool>($"api/Projects/delite/{UserId}/{GameId}", Protocol.Https);
            }
            catch (UndefinedProtocolException)
            {
                response = await mannageer.SendMessageRest<bool>($"api/Projects/delite/{UserId}/{GameId}", Protocol.Http);

            }
        }
    }
}
