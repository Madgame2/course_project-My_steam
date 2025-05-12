using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Net_DTOLib
{
    public class DeleteFromCartDTO
    {
        public long UserId {  get; set; }
        public long CartId { get; set; }
    }
}
