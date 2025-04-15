using My_steam_server.Interfaces;
using My_steam_server.Models;
using System.Text.Json;

namespace My_steam_server.Repositories
{
    public class JsonUserRepository : IUserRepository
    {
        private readonly string _filePath = "";
        private List<User> _Users;

        public JsonUserRepository(string File_path)
        {
            _filePath = File_path;

            if (File.Exists(File_path))
            {

                var json = File.ReadAllText(File_path);
                _Users = JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
            }
            else
            {
                _Users = new List<User>();
            }
        }

        public void addUser(User user)
        {
            user.Id =Convert.ToString( _Users.Count>0 ? _Users.Max(u=>u.Id)+1: 1);
            _Users.Add(user);
        }

        public IEnumerable<User> GetAll() => _Users;

        public User? GetByEmail(string email)
        {
            return _Users.FirstOrDefault(u => u.Email == email);
        }

        public User? GetById(int id) => _Users.FirstOrDefault(U => Convert.ToUInt32(U.Id) == id);

        public void SaveChanges()
        {
            var json = JsonSerializer.Serialize(_Users, new JsonSerializerOptions { WriteIndented =true});
            File.WriteAllText(_filePath, json);
        }
    }
}
