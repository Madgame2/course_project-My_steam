using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_steam_server
{
    static class CommonMethods
    {

        public static string SanitizeFolderName(string name)
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            foreach (var ch in invalidChars)
            {
                name = name.Replace(ch, '_');
            }
            return name;
        }
    }
}
