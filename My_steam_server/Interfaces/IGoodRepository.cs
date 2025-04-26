namespace My_steam_server.Interfaces
{
    public interface IGoodRepository<T>
    {
        Task<List<T>> GetAll();
        Task<T?> GetByIdAsync(int id);
        Task<bool> addAsync(T entity);

    }
}
