namespace My_steam_server.Models
{
    public class UserLibraryEntry
    {
        public string UserId {  get; set; }
        public User User { get; set; }

        public long GameId {  get; set; }
        public Game Game { get; set; }

        public DateTime PurchaseDate { get; set; }

    }
}
