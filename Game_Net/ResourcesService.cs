using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Net
{
    public class ResourcesService
    {
        private readonly ComunitationMannageer _commManager;

        public ResourcesService( ComunitationMannageer commManager)
        {
            _commManager = commManager;
        }

        public async Task<string?> GetMarkdownStreamAsync(string filePath)
        {
            var result  = await _commManager.GetResourcesAsyc(filePath);

            if(result == null)
            {
                throw new NotFoundExaption("");
            }

            using (var reader = new StreamReader(result))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}
