using My_steam_server.Interfaces;
using My_steam_server.Models;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace My_steam_server.Repositories.ADO
{
    public class ScreenshotsRepository : IScreenshotsRepository
    {
        private readonly string _connectionString;

        public ScreenshotsRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Screenshot>> GetAllAsync()
        {
            var screenshots = new List<Screenshot>();

            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string sql = "SELECT Id, ImageSource, GameId FROM Screenshots";

            using var cmd = new SqlCommand(sql, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                screenshots.Add(new Screenshot
                {
                    Id = reader.GetInt64(0),
                    ImageSource = reader.GetString(1),
                    GameId = reader.GetInt64(2)
                });
            }

            return screenshots;
        }

        public async Task<Screenshot?> GetByIdAsync(long id)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string sql = "SELECT Id, ImageSource, GameId FROM Screenshots WHERE Id = @Id";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Screenshot
                {
                    Id = reader.GetInt64(0),
                    ImageSource = reader.GetString(1),
                    GameId = reader.GetInt64(2)
                };
            }

            return null;
        }

        public async Task<IEnumerable<Screenshot>> GetByGameIdAsync(long gameId)
        {
            var screenshots = new List<Screenshot>();

            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string sql = "SELECT Id, ImageSource, GameId FROM Screenshots WHERE GameId = @GameId";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@GameId", gameId);

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                screenshots.Add(new Screenshot
                {
                    Id = reader.GetInt64(0),
                    ImageSource = reader.GetString(1),
                    GameId = reader.GetInt64(2)
                });
            }

            return screenshots;
        }

        public async Task AddAsync(Screenshot screenshot)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string sql = @"
                INSERT INTO Screenshots (ImageSource, GameId)
                VALUES (@ImageSource, @GameId);
                SELECT SCOPE_IDENTITY();";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@ImageSource", screenshot.ImageSource);
            cmd.Parameters.AddWithValue("@GameId", screenshot.GameId);

            screenshot.Id = Convert.ToInt64(await cmd.ExecuteScalarAsync());
        }

        public async Task UpdateAsync(Screenshot screenshot)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string sql = @"
                UPDATE Screenshots 
                SET ImageSource = @ImageSource, GameId = @GameId
                WHERE Id = @Id";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", screenshot.Id);
            cmd.Parameters.AddWithValue("@ImageSource", screenshot.ImageSource);
            cmd.Parameters.AddWithValue("@GameId", screenshot.GameId);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(long id)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string sql = "DELETE FROM Screenshots WHERE Id = @Id";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteByGameIdAsync(long gameId)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string sql = "DELETE FROM Screenshots WHERE GameId = @GameId";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@GameId", gameId);

            await cmd.ExecuteNonQueryAsync();
        }
    }
} 