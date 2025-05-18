using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Net_DTOLib
{
    public class GamesDB_dto
    {
        public long GameId { get; set; }
        public string GameName { get; set; } = string.Empty;
        public string Descritption { get; set; } = string.Empty;
        public string HeaderImageSource { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string DownloadedSource { get; set; } = string.Empty;
        public Ratinng Rating { get; set; }
        public DateTime ReliseDate { get; set; }
        public float Price { get; set; }


        public bool IsNew { get; set; } = false;
    }
}
