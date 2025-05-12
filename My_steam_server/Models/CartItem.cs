using System.Text.Json.Serialization;

namespace My_steam_server.Models
{
    public class CartItem
    {
        public long CartItemId { get; set; }
        public string UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }


        public long PurchaseOptionId { get; set; }
        [JsonIgnore]
        public PurchaseOption PurchaseOption { get; set; }

        public DateTime PurchaseDate { get; set; }
    }
}
