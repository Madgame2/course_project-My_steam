using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Net_DTOLib
{
    public  class CartItemDto
    {
        public long CarItemtId {  get; set; }
        public string purchouseNmae {  get; set; }
        public string ImageLink {  get; set; }
        public float Price {  get; set; }
    }
}
