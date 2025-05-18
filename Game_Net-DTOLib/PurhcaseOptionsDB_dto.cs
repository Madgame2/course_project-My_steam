using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Net_DTOLib
{
    public class PurhcaseOptionsDB_dto
    {
        public long PurhcaseId { get; set; }
        public float Price { get; set; }
        public string PurhcaseName { get; set; } = string.Empty;
        public string imageLinnk { get; set; } = string.Empty;
        public long GameID { get; set; }


        public bool IsNew { get; set; } = false;
    }
}
