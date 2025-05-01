using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Net_DTOLib
{
    public class ProductDto
    {
        public List<PurchaseOption> PurchaseOptions { get; set; }
    }

    public class PurchaseOption
    {
        public string GameName { get; set; }
        public string Price { get; set; }
    }
}
