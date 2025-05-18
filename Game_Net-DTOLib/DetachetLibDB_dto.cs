using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Net_DTOLib
{
    public class DetachetLibDB_dto
    {
        public string UserId { get; set; } = string.Empty;
        public long GameId { get; set; }
        public DateTime PurchaseDate { get; set; }


        public bool IsNew { get; set; } = false;
    }
}
