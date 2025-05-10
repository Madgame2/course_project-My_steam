using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Net_DTOLib
{
    public class ProductFilterDto
    {
        public int PageSize { get; set; } = 10;
        public long? LastSeenId { get; set; }
        public string? Search {  get; set; }

        public decimal? minPrice {  get; set; }
        public decimal? maxPrice {  get; set; }
    }
}
