namespace My_steam_server.Interfaces
{
    public interface IBoughtService
    {
        Task<bool> buyUserCart(string userId);
    }
}
