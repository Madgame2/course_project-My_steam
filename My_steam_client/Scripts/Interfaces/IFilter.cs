using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_steam_client.Scripts.Interfaces
{
    public interface IFilter
    {
        IEnumerable<KeyValuePair<string, string>> ToQueryParameters();
    }
}
