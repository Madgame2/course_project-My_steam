using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Net_DTOLib
{
    public class ResivedGoodsDB_dto
    {
        public long Id { get; set; }
        public long GoodId { get; set; }
        public long PurhcaseOptinId { get; set; }

        public bool IsNew { get; set; } = false;
    }
}
