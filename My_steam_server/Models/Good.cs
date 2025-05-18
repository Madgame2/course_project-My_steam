namespace My_steam_server.Models
{
    public abstract class Good
    {
        public long Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? UserId { get; set; } = string.Empty;

        public float Price { get; set; }
    }
}
