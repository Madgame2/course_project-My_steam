using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Net_DTOLib
{
    public class LogInSecsessDto
    {
        public string id {  get; set; }

        public string NickName {  get; set; }
        public DateTime RegisterDate { get; set; }
        public UserRole UserRole { get; set; }
        public string tokens { get; set; }
    }
}
