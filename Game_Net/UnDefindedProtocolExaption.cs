using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Net
{
    public class UnDefindedProtocolExaption:Exception
    {
        public UnDefindedProtocolExaption(Protocol protocol,string message) : base($"protocol: {protocol} is undefinded message:{message}") { }
    }
}
