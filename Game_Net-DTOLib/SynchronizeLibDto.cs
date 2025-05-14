using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Net_DTOLib
{
    public class SynchronizeLibDto
    {
        public string UserId {  get; set; }
        public long GameId {  get; set; }
        public string Gamename {  get; set; }
        public string DownloadSource {  get; set; }

        public DateTime PurchaseDate { get; set; }
        public long SpaceRequered {  get; set; }
    }
}
