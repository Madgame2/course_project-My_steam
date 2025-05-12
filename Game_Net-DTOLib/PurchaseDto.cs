using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Net_DTOLib
{
    public class PurchaseDto
    {
        public string UserId {  get; set; }
        public List<long> purchouseIds { get; set; }
    }
}
