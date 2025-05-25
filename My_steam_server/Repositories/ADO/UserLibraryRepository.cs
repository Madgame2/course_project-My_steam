using My_steam_server.Interfaces;
using My_steam_server.Models;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace My_steam_server.Repositories.ADO
{
    public class UserLibraryRepository : IUserLibraryRepository
    {
        private readonly string _connectionString;

        public UserLibraryRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<UserLibraryEntry>> GetByUserIdAsync(string userId)
        {
            var entries = new List<UserLibraryEntry>();

            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string sql = @"
                SELECT ul.Id, ul.UserId, ul.GameId, g.Name, g.DownloadSource
                FROM UserLibrary ul
                JOIN Games g ON ul.GameId = g.Id
                WHERE ul.UserId = @UserId";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@UserId", userId);

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                entries.Add(new UserLibraryEntry
                {
                    Id = reader.GetInt64(0),
                    UserId = reader.GetString(1),
                    GameId = reader.GetInt64(2),
                    GameName = reader.GetString(3),
                    DownloadSource = reader.GetString(4)
                });
            }

            return entries;
        }

        public async Task AddToLibraryAsync(string userId, long gameId)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            // Проверка наличия
            using (var checkCmd = new SqlCommand("SELECT COUNT(*) FROM UserLibrary WHERE UserId = @UserId AND GameId = @GameId", conn))
            {
                checkCmd.Parameters.AddWithValue("@UserId", userId);
                checkCmd.Parameters.AddWithValue("@GameId", gameId);

                var exists = (int)await checkCmd.ExecuteScalarAsync() > 0;
                if (exists)
                    throw new ArgumentException("Игра уже в библиотеке.");
            }

            // Добавление
            string sql = "INSERT INTO UserLibrary (UserId, GameId) VALUES (@UserId, @GameId)";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@GameId", gameId);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task RemoveFromLibraryAsync(string userId, long gameId)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string sql = "DELETE FROM UserLibrary WHERE UserId = @UserId AND GameId = @GameId";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@GameId", gameId);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<bool> IsInLibraryAsync(string userId, long gameId)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string sql = "SELECT COUNT(*) FROM UserLibrary WHERE UserId = @UserId AND GameId = @GameId";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@GameId", gameId);

            return (int)await cmd.ExecuteScalarAsync() > 0;
        }
    }
} 