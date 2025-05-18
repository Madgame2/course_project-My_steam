using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Net_DTOLib
{
    public class SynchronizeDto
    {
        public List<UserDB_dto>? Users { get; set; }
        public List<GamesDB_dto>? Games { get; set; }
        public List<PurhcaseOptionsDB_dto>? Purhcases { get; set; }
        public List<DetachetLibDB_dto>? AttachetLib { get; set; }
    }
}
