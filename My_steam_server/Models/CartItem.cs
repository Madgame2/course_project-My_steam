namespace My_steam_server.Models
{
    public class CartItem
    {
        public long CartItemId { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }


        public long PurchaseOptionId { get; set; }
        public PurchaseOption PurchaseOption { get; set; }
    }
}
