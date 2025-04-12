using My_steam_server.Models;

namespace My_steam_server.Interfaces
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAll();
        User? GetById(int id);
        void addUser(User user);
        void SaveChanges();
    }
}
