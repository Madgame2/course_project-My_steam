using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Net_DTOLib
{


    public class PingDto
    {
        public DateTime date { get; set; }
        public ServerStatus status { get; set; }
    }
}
