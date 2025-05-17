namespace My_steam_server.Models
{
    public class Screenshot
    {
        public int Id { get; set; }
        public string Path { get; set; } = "";


        public long GameId { get; set; }
        public Game Game { get; set; } = null!;
    }
}
