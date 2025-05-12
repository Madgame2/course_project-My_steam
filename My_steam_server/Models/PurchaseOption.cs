namespace My_steam_server.Models
{
    public class PurchaseOption
    {
        public long OptionId { get; set; }
        public float Price { get; set; }

        public List<GoodReceived> GoodsReceived { get; set; } = new();
    }

    public class GoodReceived
    {
        public long Id { get; set; }        // Первичный ключ
        public long GoodId { get; set; }    // Идентификатор полученного товара

        // Внешний ключ к PurchaseOption
        public long PurchaseOptionId { get; set; }
        public PurchaseOption PurchaseOption { get; set; }
    }
}
