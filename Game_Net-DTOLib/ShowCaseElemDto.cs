using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Net_DTOLib
{
    public class ShowCaseElemDto
    {
        public long productId {  get; set; }
        public string headerImageSource { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public float price { get; set; }
    }
}
