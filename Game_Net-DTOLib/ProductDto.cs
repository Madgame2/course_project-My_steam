using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Net_DTOLib
{
    public class ProductDto
    {
        public long GameId { get; set; }
        public string GameName { get; set; }
        public string Description { get; set; }

        public string MdFileSourcce { get; set; }
        public DateTime ReleaseDate { get; set; }
        public Ratinng rating { get; set; }

        public string ImageLink { get; set; }
        public List<string> imagesLinks {  get; set; }

        public List<PurchaseOption> PurchaseOptions { get; set; }
    }

    public enum Ratinng
    {
        Extrimly_positive,
        Very_positive,
        Positive,
        Mixed,
        Mostly_negative,
        Very_negative,
        Extrimly_negative
    }

    public class PurchaseOption
    {
        public int PurchaseId {  get; set; }

        public string GameName { get; set; }
        public string Price { get; set; }
    }
}
