using Game_Net_DTOLib;

namespace My_steam_server.Interfaces
{
    public interface IGoodRepository<T>
    {
        Task<List<T>> GetAll();
        Task<T?> GetByIdAsync(long id);
        Task<bool> addAsync(T entity);

        Task<bool> HasObject(T entity);

        Task<List<T>> GetPagesAsync(ProductFilterDto filter);
    }
}
