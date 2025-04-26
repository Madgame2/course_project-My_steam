namespace My_steam_server.Models
{
    public abstract class Good
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public float Price { get; set; }
    }
}
