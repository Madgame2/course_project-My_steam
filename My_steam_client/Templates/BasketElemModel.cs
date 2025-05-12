using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_steam_client.Templates
{
    public class BasketElemModel
    {
        public long GameId { get; set; }
        public string GameName { get; set; }
        public string ImageLink { get; set; }
        public float Price {  get; set; }
    }
}
