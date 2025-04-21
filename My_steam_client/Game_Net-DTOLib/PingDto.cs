using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Net_DTOLib
{
    public enum ServerStatus
    {
        Unknown = 0,
        Online =1,
        Service=2
    }

    public class PingDto
    {
        public DateTime date { get; set; }
        public ServerStatus status { get; set; }
    }
}
