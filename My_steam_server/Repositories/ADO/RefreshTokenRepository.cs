using My_steam_server.Interfaces;
using My_steam_server.Models;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace My_steam_server.Repositories.ADO
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly string _connectionString;

        public RefreshTokenRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string sql = @"
                SELECT Id, Token, UserId, ExpiryDate, IsRevoked
                FROM RefreshTokens
                WHERE Token = @Token";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Token", token);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new RefreshToken
                {
                    Id = reader.GetInt64(0),
                    Token = reader.GetString(1),
                    UserId = reader.GetString(2),
                    ExpiryDate = reader.GetDateTime(3),
                    IsRevoked = reader.GetBoolean(4)
                };
            }

            return null;
        }

        public async Task<IEnumerable<RefreshToken>> GetByUserIdAsync(string userId)
        {
            var tokens = new List<RefreshToken>();

            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string sql = @"
                SELECT Id, Token, UserId, ExpiryDate, IsRevoked
                FROM RefreshTokens
                WHERE UserId = @UserId";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@UserId", userId);

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                tokens.Add(new RefreshToken
                {
                    Id = reader.GetInt64(0),
                    Token = reader.GetString(1),
                    UserId = reader.GetString(2),
                    ExpiryDate = reader.GetDateTime(3),
                    IsRevoked = reader.GetBoolean(4)
                });
            }

            return tokens;
        }

        public async Task AddAsync(RefreshToken token)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string sql = @"
                INSERT INTO RefreshTokens (Token, UserId, ExpiryDate, IsRevoked)
                VALUES (@Token, @UserId, @ExpiryDate, @IsRevoked);
                SELECT SCOPE_IDENTITY();";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Token", token.Token);
            cmd.Parameters.AddWithValue("@UserId", token.UserId);
            cmd.Parameters.AddWithValue("@ExpiryDate", token.ExpiryDate);
            cmd.Parameters.AddWithValue("@IsRevoked", token.IsRevoked);

            token.Id = Convert.ToInt64(await cmd.ExecuteScalarAsync());
        }

        public async Task UpdateAsync(RefreshToken token)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string sql = @"
                UPDATE RefreshTokens 
                SET Token = @Token, UserId = @UserId, 
                    ExpiryDate = @ExpiryDate, IsRevoked = @IsRevoked
                WHERE Id = @Id";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", token.Id);
            cmd.Parameters.AddWithValue("@Token", token.Token);
            cmd.Parameters.AddWithValue("@UserId", token.UserId);
            cmd.Parameters.AddWithValue("@ExpiryDate", token.ExpiryDate);
            cmd.Parameters.AddWithValue("@IsRevoked", token.IsRevoked);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(long id)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string sql = "DELETE FROM RefreshTokens WHERE Id = @Id";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteByUserIdAsync(string userId)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string sql = "DELETE FROM RefreshTokens WHERE UserId = @UserId";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@UserId", userId);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task RevokeTokenAsync(string token)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string sql = "UPDATE RefreshTokens SET IsRevoked = 1 WHERE Token = @Token";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Token", token);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task RevokeAllUserTokensAsync(string userId)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string sql = "UPDATE RefreshTokens SET IsRevoked = 1 WHERE UserId = @UserId";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@UserId", userId);

            await cmd.ExecuteNonQueryAsync();
        }
    }
} 