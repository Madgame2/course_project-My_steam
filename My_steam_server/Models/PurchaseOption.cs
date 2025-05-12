namespace My_steam_server.Models
{
    public class PurchaseOption
    {
        public long OptionId { get; set; }
        public float Price { get; set; }
        public string PurchaseName {  get; set; }

        // Один вариант может включать много товаров (в том числе саму игру)
        public List<GoodReceived> GoodsReceived { get; set; } = new();

        // Внешний ключ к игре, к которой этот вариант покупки относится
        public long GameId { get; set; }
        public Game Game { get; set; }
    }

    public class GoodReceived
    {
        public long Id { get; set; }        // Первичный ключ
        public long GoodId { get; set; }    // Идентификатор полученного товара

        public long PurchaseOptionId { get; set; }
        public PurchaseOption PurchaseOption { get; set; }
    }
}
