using My_steam_server.Interfaces;
using My_steam_server.Models;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace My_steam_server.Repositories.ADO
{
    public class ReportsRepository : IReportsRepository
    {
        private readonly string _connectionString;

        public ReportsRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<ReportmessgeModel>> GetAllAsync()
        {
            var reports = new List<ReportmessgeModel>();

            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string sql = @"
                SELECT r.Id, r.UserId, r.GameId, r.Message, r.Date, 
                       u.Email as UserEmail, g.Name as GameName
                FROM Reports r
                JOIN Users u ON r.UserId = u.Id
                JOIN Games g ON r.GameId = g.Id
                ORDER BY r.Date DESC";

            using var cmd = new SqlCommand(sql, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                reports.Add(new ReportmessgeModel
                {
                    Id = reader.GetInt64(0),
                    UserId = reader.GetString(1),
                    GameId = reader.GetInt64(2),
                    Message = reader.GetString(3),
                    Date = reader.GetDateTime(4),
                    UserEmail = reader.GetString(5),
                    GameName = reader.GetString(6)
                });
            }

            return reports;
        }

        public async Task<ReportmessgeModel?> GetByIdAsync(long id)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string sql = @"
                SELECT r.Id, r.UserId, r.GameId, r.Message, r.Date, 
                       u.Email as UserEmail, g.Name as GameName
                FROM Reports r
                JOIN Users u ON r.UserId = u.Id
                JOIN Games g ON r.GameId = g.Id
                WHERE r.Id = @Id";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new ReportmessgeModel
                {
                    Id = reader.GetInt64(0),
                    UserId = reader.GetString(1),
                    GameId = reader.GetInt64(2),
                    Message = reader.GetString(3),
                    Date = reader.GetDateTime(4),
                    UserEmail = reader.GetString(5),
                    GameName = reader.GetString(6)
                };
            }

            return null;
        }

        public async Task<IEnumerable<ReportmessgeModel>> GetByUserIdAsync(string userId)
        {
            var reports = new List<ReportmessgeModel>();

            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string sql = @"
                SELECT r.Id, r.UserId, r.GameId, r.Message, r.Date, 
                       u.Email as UserEmail, g.Name as GameName
                FROM Reports r
                JOIN Users u ON r.UserId = u.Id
                JOIN Games g ON r.GameId = g.Id
                WHERE r.UserId = @UserId
                ORDER BY r.Date DESC";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@UserId", userId);

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                reports.Add(new ReportmessgeModel
                {
                    Id = reader.GetInt64(0),
                    UserId = reader.GetString(1),
                    GameId = reader.GetInt64(2),
                    Message = reader.GetString(3),
                    Date = reader.GetDateTime(4),
                    UserEmail = reader.GetString(5),
                    GameName = reader.GetString(6)
                });
            }

            return reports;
        }

        public async Task<IEnumerable<ReportmessgeModel>> GetByGameIdAsync(long gameId)
        {
            var reports = new List<ReportmessgeModel>();

            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string sql = @"
                SELECT r.Id, r.UserId, r.GameId, r.Message, r.Date, 
                       u.Email as UserEmail, g.Name as GameName
                FROM Reports r
                JOIN Users u ON r.UserId = u.Id
                JOIN Games g ON r.GameId = g.Id
                WHERE r.GameId = @GameId
                ORDER BY r.Date DESC";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@GameId", gameId);

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                reports.Add(new ReportmessgeModel
                {
                    Id = reader.GetInt64(0),
                    UserId = reader.GetString(1),
                    GameId = reader.GetInt64(2),
                    Message = reader.GetString(3),
                    Date = reader.GetDateTime(4),
                    UserEmail = reader.GetString(5),
                    GameName = reader.GetString(6)
                });
            }

            return reports;
        }

        public async Task AddAsync(ReportmessgeModel report)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string sql = @"
                INSERT INTO Reports (UserId, GameId, Message, Date)
                VALUES (@UserId, @GameId, @Message, @Date);
                SELECT SCOPE_IDENTITY();";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@UserId", report.UserId);
            cmd.Parameters.AddWithValue("@GameId", report.GameId);
            cmd.Parameters.AddWithValue("@Message", report.Message);
            cmd.Parameters.AddWithValue("@Date", report.Date);

            report.Id = Convert.ToInt64(await cmd.ExecuteScalarAsync());
        }

        public async Task DeleteAsync(long id)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string sql = "DELETE FROM Reports WHERE Id = @Id";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteByUserIdAsync(string userId)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string sql = "DELETE FROM Reports WHERE UserId = @UserId";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@UserId", userId);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteByGameIdAsync(long gameId)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string sql = "DELETE FROM Reports WHERE GameId = @GameId";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@GameId", gameId);

            await cmd.ExecuteNonQueryAsync();
        }
    }
} 